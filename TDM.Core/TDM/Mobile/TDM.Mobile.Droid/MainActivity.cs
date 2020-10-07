using Android.App;
using Supermodel.Mobile.Runtime.Droid.App;
using TDM.Mobile.AppCore;

namespace TDM.Mobile.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : DroidFormsApplication<TDMApp> { }
}