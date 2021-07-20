using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "Api.Forms.Startup",
    XamarinFormsAppTypeName = "Api.Forms.App"
)]

namespace Api.Forms.Android
{
    [Activity(Label = "Api.Forms",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // protected override void OnCreate(Bundle savedInstanceState)
        // {
        //     // TabLayoutResource = Resource.Layout.Tabbar;
        //     // ToolbarResource = Resource.Layout.Toolbar;
        //     //
        //     // base.OnCreate(savedInstanceState);
        //     // global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
        //     // LoadApplication(new App());
        // }
        
    }
}