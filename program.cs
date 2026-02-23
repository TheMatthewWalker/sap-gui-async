using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Threading;
using Velopack;

namespace costing_tool
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Velopack MUST run before anything else
            VelopackApp.Build()
                .WithFirstRun(v =>
                {
                    // Runs once after first install - useful for shortcuts etc.
                })
                .WithAfterUpdateFastCallback(v =>
                {
                    // Runs after an update is applied
                })
                .Run();

            // Standard WinUI 3 startup continues as normal
            WinRT.ComWrappersSupport.InitializeComWrappers();
            Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(
                    DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                new App();
            });
        }
    }
}