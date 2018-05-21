using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Graphics;
using Java.Lang;
using Android.Media;

namespace MazeSolver.Droid
{
    [Activity(Label = "Maze Solver", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private enum PhotoSource { Camera, Gallery};

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                Bitmap bitmap = null;

                if (requestCode == (int)PhotoSource.Camera)
                    bitmap = (Bitmap)data.Extras.Get("data");
                else if (requestCode == (int)PhotoSource.Gallery)
                    bitmap = GetThumbnail(data.Data, 150);
                else
                    return;

                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new EditMazeTabPage(bitmap));
            }
        }

        public void TakeAPhoto()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, (int)PhotoSource.Camera);
        }

        public void FindAPhoto()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select photo"), (int)PhotoSource.Gallery);
        }

        public Bitmap GetThumbnail(Android.Net.Uri uri, int thumbnailSize)
        {
            var input = this.ContentResolver.OpenInputStream(uri);

            BitmapFactory.Options onlyBoundsOptions = new BitmapFactory.Options();
            onlyBoundsOptions.InJustDecodeBounds = true;
            onlyBoundsOptions.InDither = true; //optional
            onlyBoundsOptions.InPreferredConfig = Bitmap.Config.Argb8888; //optional
            BitmapFactory.DecodeStream(input, null, onlyBoundsOptions);
            input.Close();

            if ((onlyBoundsOptions.OutWidth == -1) || (onlyBoundsOptions.OutHeight == -1))
                return null;

            bool willBeRotated = onlyBoundsOptions.OutHeight > onlyBoundsOptions.OutWidth;

            int originalSize = (onlyBoundsOptions.OutHeight > onlyBoundsOptions.OutWidth)
                ? onlyBoundsOptions.OutHeight : onlyBoundsOptions.OutWidth;

            double ratio = (originalSize > thumbnailSize) ? (originalSize / thumbnailSize) : 1.0;

            BitmapFactory.Options bitmapOptions = new BitmapFactory.Options();
            bitmapOptions.InSampleSize = GetPowerOfTwoForSampleRatio(ratio);
            bitmapOptions.InDither = true; //optional
            bitmapOptions.InPreferredConfig = Bitmap.Config.Argb8888; //optional
            input = this.ContentResolver.OpenInputStream(uri);
            Bitmap bitmap = BitmapFactory.DecodeStream(input, null, bitmapOptions);
            input.Close();

            return bitmap;
        }

        private static int GetPowerOfTwoForSampleRatio(double ratio)
        {
            int k = Integer.HighestOneBit((int)System.Math.Floor(ratio));
            if (k == 0) return 1;
            else return k;
        }
    }
}