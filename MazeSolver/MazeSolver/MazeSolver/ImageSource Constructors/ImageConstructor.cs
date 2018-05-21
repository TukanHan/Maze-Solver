using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MazeSolver
{
    class ImageConstructor
    {
        public ImageSource ImageSource { get; protected set; }

        protected Android.Graphics.Color mazeColor = Android.Graphics.Color.ParseColor("#353333");
        protected Android.Graphics.Color transparentColor = Android.Graphics.Color.ParseColor("#f2f1ed");

        protected Bitmap bitmap;
        protected readonly int width;
        protected readonly int height;

        public ImageConstructor(bool[,] pixelArray) : this(pixelArray.GetLength(1), pixelArray.GetLength(0))
        {
            ConstructBitmap(pixelArray);
            ConstructImage();
        }

        protected ImageConstructor(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        protected virtual void ConstructBitmap(bool[,] pixelArray)
        {
            bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Rgb565);

            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    bitmap.SetPixel(j, i, pixelArray[i,j] ? mazeColor : transparentColor);
        }

        protected void ConstructImage()
        {
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                bitmapData = stream.ToArray();
            }

            ImageSource = ImageSource.FromStream(() => new MemoryStream(bitmapData));
        }
    }
}