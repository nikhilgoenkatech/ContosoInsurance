using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

using HockeyApp.Android;
using HockeyApp.Android.Metrics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

// AndroidX replacements
using AndroidX.AppCompat.App;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ContosoInsurance.Droid
{
    [Activity(
        Label = "Contoso Insurance",
        Icon = "@drawable/icon",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public static MainActivity instance;
        private string HOCKEYAPP_APPID = Settings.Current.MobileHockeyAppId;

        protected override void OnCreate(Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            Window.AddFlags(WindowManagerFlags.Fullscreen);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            App.UIContext = this;
            LoadApplication(new ContosoInsurance.App());

            instance = this;

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            if (SupportActionBar != null)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            toolbar.SetNavigationIcon(Resource.Drawable.navmenu);

            try
            {
                // Register the crash manager before initializing the trace writer
                CrashManager.Register(this, HOCKEYAPP_APPID);

                // Register with the Metrics Manager
                MetricsManager.Register(Application, HOCKEYAPP_APPID);
                MetricsManager.EnableUserMetrics();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }

            // IMPORTANT:
            // GCM (Gcm.Client) is obsolete and the old component store library is not available.
            // You must migrate push notifications to Firebase Cloud Messaging (FCM).
            // For now, the GCM registration code has been removed so the project can compile.
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public static MainActivity DefaultService
        {
            get { return instance; }
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}
