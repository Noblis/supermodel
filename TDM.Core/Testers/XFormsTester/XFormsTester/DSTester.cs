using Supermodel.Mobile.Runtime.Common.Services;

namespace XFormsTester
{
    public static class DSTester
    {
        public static void DSTest()
        {
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
        }
    }
}
