using System.Threading;
using System.Windows;
using NUnit.Framework;
using Shazzam;

//namespace ShazzamUnitTests.Helpers; // Keep no namespace, so that `[SetUpFixture]` can run before any test method.

[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var manualResetEventSlim = new ManualResetEventSlim(initialState:false);
        // Ensure the WPF application running
        var wpfThread = new Thread(() =>
        {
            var application = new Application();
            application.Startup += delegate
            {
                manualResetEventSlim.Set();
            };
            application.Run();
        })
        {
            IsBackground = true,
            Name = "WPF UI Thread"
        };
        wpfThread.SetApartmentState(ApartmentState.STA);
        wpfThread.Start();

        manualResetEventSlim.Wait();
        manualResetEventSlim.Dispose();
    }
}
