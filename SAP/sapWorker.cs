// Worker class to handle SAP functions in a separate thread
using SAPFunctionsOCX;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

public sealed class SapWorker : IDisposable
{
    private readonly BlockingCollection<Action> _queue = new();
    private Thread _thread;
    private SAPFunctions? _sapFuncs;
    private Exception? _initException;
    private readonly ManualResetEventSlim _inited = new(false);

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
        // COM created on THIS thread
        try
        {
            _sapFuncs = new SAPFunctions();
        }
        catch (Exception ex)
        {
            _initException = ex;
            System.Diagnostics.Debug.WriteLine($"SAPFunctions init failed: {ex}");
            // Stop accepting new work and signal initialization complete with failure
            _queue.CompleteAdding();
            _inited.Set();
            return;
        }

        // Initialization succeeded
        _inited.Set();

        foreach (var action in _queue.GetConsumingEnumerable())
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // Ensure worker thread doesn't die on unexpected action exceptions
                System.Diagnostics.Debug.WriteLine($"SapWorker action threw: {ex}");
            }
        }
    }

    public Task<T> InvokeAsync<T>(Func<SAPFunctions, T> func)
    {
        var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Wait for initialization to complete (short timeout to avoid hanging callers)
        // If initialization isn't complete within the timeout, fail fast.
        const int initTimeoutMs = 5000;
        if (!_inited.Wait(initTimeoutMs))
        {
            tcs.SetException(new InvalidOperationException("SAP worker initialization timed out."));
            return tcs.Task;
        }

        if (_initException != null)
        {
            tcs.SetException(new InvalidOperationException("SAP worker initialization failed.", _initException));
            return tcs.Task;
        }

        // At this point _sapFuncs should be non-null
        _queue.Add(() =>
        {
            if (_sapFuncs == null)
            {
                tcs.SetException(new InvalidOperationException("SAPFunctions not initialized."));
                return;
            }

            try
            {
                var result = func(_sapFuncs);
                tcs.SetResult(result);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        });

        return tcs.Task;
    }

    public void Dispose()
    {
        try
        {
            _queue.CompleteAdding();
            _thread.Join();
            if (_sapFuncs != null)
            {
                try { Marshal.FinalReleaseComObject(_sapFuncs); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Failed to release SAPFunctions COM object: {ex}"); }
                _sapFuncs = null;
            }
        }
        finally
        {
            _inited.Dispose();
        }
    }
}
