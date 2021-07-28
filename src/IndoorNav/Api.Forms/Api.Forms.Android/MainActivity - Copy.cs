// using System;
// using Android.App;
// using Android.Content;
// using Android.Content.PM;
// using Android.Runtime;
// using Android.OS;
// using Api.Forms.Android.PlatformDependency;
// using ApplicationCore.App.PlatformServices;
// using Prism.DryIoc;
// using Prism.Ioc;
// using Shiny;
//
// // [assembly: Shiny.ShinyApplication(
// //     ShinyStartupTypeName = "Api.Forms.Startup",
// //     XamarinFormsAppTypeName = "Api.Forms.App"
// // )]
//
// namespace Api.Forms.Android
// {
//     // Android App
//     [Application]
//     public partial class MainApplication : Application
//     {
//         public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) {}
//
//         public override void OnCreate()
//         {
//             PrismContainerExtension.Current.Register<IMyAndroidService, MyAndroidService>();//TODO: написать метод расширения на IcontainerExtensions
//             this.ShinyOnCreate(new Startup());
//             Xamarin.Essentials.Platform.Init(this);
//             base.OnCreate();
//         }
//     }
//     
//     
//     [Activity(Label = "Api.Forms",
//         Theme = "@style/MainTheme",
//         MainLauncher = true,
//         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
//     public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
//     {
//         protected override void OnCreate(Bundle savedInstanceState)
//         {
//             this.ShinyOnCreate();
//             TabLayoutResource = Resource.Layout.Tabbar;
//             ToolbarResource = Resource.Layout.Toolbar;
//             base.OnCreate(savedInstanceState);
//             Xamarin.Forms.Forms.Init(this, savedInstanceState);
//             
//             LoadApplication(new App());
//            //LoadApplication(new App(new PrismInit()));
//         }
//         
//         protected override void OnNewIntent(Intent intent)
//         {
//             base.OnNewIntent(intent);
//             this.ShinyOnNewIntent(intent);
//         }
//         
//         protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
//         {
//             base.OnActivityResult(requestCode, resultCode, data);
//             this.ShinyOnActivityResult(requestCode, resultCode, data);
//         }
//         
//         
//         public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
//         {
//             base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//             this.ShinyOnRequestPermissionsResult(requestCode, permissions, grantResults);
//             Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//         }
//         
//     }
// }