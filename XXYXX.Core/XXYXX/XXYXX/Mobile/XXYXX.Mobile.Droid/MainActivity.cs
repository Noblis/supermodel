﻿using Android.App;
using Android.OS;

namespace XXYXX.Mobile.Droid
{
    [Activity(Label = "XXYXX.Mobile.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

