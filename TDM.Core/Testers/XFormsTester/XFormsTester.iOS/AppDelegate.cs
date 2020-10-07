using Foundation;
using Supermodel.Mobile.Runtime.Common.Services;
using Supermodel.Mobile.Runtime.iOS.Services;
using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace XFormsTester.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App());

            // ReSharper disable once UnusedVariable
            var tmp = new DeviceInformation();

            // ReSharper disable once UnusedVariable
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name).ToList();

            DSTester.DSTest();

            var deviceInfo = SharedService.Instantiate<IDeviceInformation>();
            // ReSharper disable UnusedVariable
            var isEmulator = deviceInfo.IsRunningOnEmulator();
            var isJailbroken = deviceInfo.IsJailbroken(true);
            var isIsSecured = deviceInfo.IsDeviceSecuredByPasscode(true);
            // ReSharper restore UnusedVariable

            var deviceInfo2 = SharedService.Instantiate<IDeviceInformation>();
            // ReSharper disable UnusedVariable
            var isEmulator2 = deviceInfo2.IsRunningOnEmulator();
            var isJailbroken2 = deviceInfo2.IsJailbroken(true);
            var isIsSecured2 = deviceInfo2.IsDeviceSecuredByPasscode(true);
            // ReSharper restore UnusedVariable

            return base.FinishedLaunching(app, options);
        }
    }
}
