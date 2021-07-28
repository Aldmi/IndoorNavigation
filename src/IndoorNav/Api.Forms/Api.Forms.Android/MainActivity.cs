using Android.App;
using Android.Content.PM;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "Api.Forms.Startup",
    XamarinFormsAppTypeName = "Api.Forms.App"
)]


namespace Api.Forms.Android
{
    
    [Activity(Label = "Api.Form.General",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        //Shiny generate this code, when mark class paprtial.
        
        // protected override void OnCreate(Bundle savedInstanceState)
        // {
        //     base.OnCreate(savedInstanceState);
        //
        //     Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        //     global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
        //     LoadApplication(new App());
        // }
        // public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        // {
        //     Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //
        //     base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        // }
    }
}