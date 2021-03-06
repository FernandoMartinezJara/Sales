﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using ImageCircle.Forms.Plugin.Droid;

namespace Sales.Droid
{
    [Activity(
        Label = "Sales", 
        Icon = "@drawable/ic_launcher", 
        Theme = "@style/MainTheme", 
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            //Inicializo Plugin Imagen Circular
            ImageCircleRenderer.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //inicializo Mapas
            global::Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(
            int requestCode, 
            string[] permissions, 
            Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

