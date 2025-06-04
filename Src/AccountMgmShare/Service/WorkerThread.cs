using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace AccountMgm.Service;

public class WorkerThread : IDisposable
{
    private readonly Thread _thread;
    private readonly BlockingCollection<Action> _queue = new();

    public WorkerThread()
    {
        _thread = new Thread(WorkLoop) { Name = "MyWorkerThread", IsBackground = true };
        _thread.Start();
    }

    public virtual void Dispose()
    {
        _queue.CompleteAdding();
        _thread.Join();
        _queue.Dispose();
    }

    private void WorkLoop()
    {
        foreach (var action in _queue.GetConsumingEnumerable())
        {
            try { action(); }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        //while (_running)
        //{
        //    if (_queue.TryTake(out var action, Timeout.Infinite))
        //    {
        //        try { action(); }
        //        catch { /* Optionally log */ }
        //    }
        //}
    }

    public void Invoke(Action action)
    {
        Exception? error = null;

        var ready = new AutoResetEvent(false);
        _queue.Add(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                ready.Set();
            }
        });
        ready.WaitOne();
        if (error != null)
        {
            throw error;
        }
    }

    public T Invoke<T>(Func<T> func)
    {
        Exception? error = null;
        T? result = default;

        var ready = new AutoResetEvent(false);
        _queue.Add(() =>
        {
            try
            {
                result = func();
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                ready.Set();
            }
        });
        ready.WaitOne();
        if (error != null)
        {
            throw error;
        }

        return result!;
    }

    public Task<T> InvokeAsync<T>(Func<T> func)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        _queue.Add(() =>
        {
            try
            {
                var result = func();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }

    public IEnumerable<T> InvokeEnumerable<T>(Func<IEnumerable<T>> func)
    {
        return new WorkerEnumerable<T>(this, func);
    }

    public IAsyncEnumerable<T> InvokeAsyncEnumerable<T>(Func<IAsyncEnumerable<T>> func)
    {
        return new WorkerAsyncEnumerable<T>(this, func);
    }

    #region Enumerable

    private class WorkerEnumerable<T>(WorkerThread worker, Func<IEnumerable<T>> func) : IEnumerable<T>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() => new WorkerEnumerator<T>(worker, worker.Invoke(() => func().GetEnumerator()));
    }

    private class WorkerEnumerator<T>(WorkerThread worker, IEnumerator<T> inner) : IEnumerator<T>
    {
        public void Dispose() => inner.Dispose();
        public T Current => inner.Current;
        object IEnumerator.Current => Current!;
        public void Reset() => inner.Reset();
        public bool MoveNext() => worker.Invoke(() => inner.MoveNext());
    }

    #endregion

    #region AsyncEnumerable

    private class WorkerAsyncEnumerable<T>(WorkerThread worker, Func<IAsyncEnumerable<T>> func) : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new WorkerAsyncEnumerator<T>(worker, worker.Invoke(() => func().GetAsyncEnumerator(cancellationToken)));
    }

    private class WorkerAsyncEnumerator<T>(WorkerThread worker, IAsyncEnumerator<T> inner) : IAsyncEnumerator<T>
    {
        public ValueTask DisposeAsync() => inner.DisposeAsync();
        public T Current => inner.Current;
        public ValueTask<bool> MoveNextAsync() => worker.Invoke(() => inner.MoveNextAsync());
    }

    #endregion
}
