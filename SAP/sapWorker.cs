// Worker class to handle SAP functions in a separate thread
using SAPFunctionsOCX;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

public sealed class SapWorker : IDisposable
{
    private readonly BlockingCollection<Action> _queue = new();
    private Thread _thread;

    private SAPFunctions? _sapFuncs;

    public SapWorker()
    {
        _thread = new Thread(Run)
        {
            IsBackground = true
        };
        _thread.SetApartmentState(ApartmentState.STA);
        _thread.Start();
    }

    private void Run()
    {
        //  COM created on THIS thread
        _sapFuncs = new SAPFunctions();

        foreach (var action in _queue.GetConsumingEnumerable())
        {
            action();
        }
    }

    public Task<T> InvokeAsync<T>(Func<SAPFunctions, T> func)
    {
        var tcs = new TaskCompletionSource<T>();

        _queue.Add(() =>
        {
            try
            {
                var result = func(_sapFuncs);
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                try
                {
                    // Login and try again to be added here.
                    var result = func(_sapFuncs);
                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }
        });

        return tcs.Task;
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _thread.Join();
        Marshal.FinalReleaseComObject(_sapFuncs);
    }
}
