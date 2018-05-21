using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class MazeImageConstructor : ImageConstructor
    {
        private Color pathColor = Color.ParseColor("#f4424b");

        public MazeImageConstructor(bool[,] pixelArray, bool[,] route, int scale) : base(pixelArray.GetLength(1), pixelArray.GetLength(0))
        {
            ConstructBitmap(pixelArray);
            DrawRoute(route);
            ScaleBitmap(scale);
            ConstructImage();
        }

        public MazeImageConstructor(bool[,] pixelArray, int scale) : base(pixelArray.GetLength(1), pixelArray.GetLength(0))
        {
            ConstructBitmap(pixelArray);
            ScaleBitmap(scale);
            ConstructImage();
        }

        protected void ScaleBitmap(int scale)
        {
            bitmap = Bitmap.CreateScaledBitmap(bitmap, bitmap.Width * scale, bitmap.Height * scale, false);
        }

        protected void DrawRoute(bool[,] route)
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    for (int y = 0; y < 5; ++y)
                    {
                        for (int x = 0; x < 5; ++x)
                        {
                            if (y > 0 && y < 4 && x > 0 && x < 4 && route[i, j])
                                bitmap.SetPixel(j * 5 + y, i * 5 + x, pathColor);
                        }
                    }

                    if (route[i, j])
                        MarkSides(j, i, route, pathColor);
                }
            }
        }

        protected override void ConstructBitmap(bool[,] pixelArray)
        {
            bitmap = Bitmap.CreateBitmap(width*5, height*5, Bitmap.Config.Rgb565);

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    for(int y=0; y< 5; ++y)
                    {
                        for (int x = 0; x < 5; ++x)
                        {
                            if(y>0 && y<4 && x > 0 && x < 4 && pixelArray[i, j])
                                bitmap.SetPixel(j * 5 + y, i * 5 + x, mazeColor);
                            else
                                bitmap.SetPixel(j*5 + y, i*5 + x, transparentColor);
                        }
                    }

                    if (pixelArray[i, j])
                        MarkSides(j, i, pixelArray, mazeColor);
                }           
            }           
        }

        private void MarkSides(int x, int y, bool[,] array, Color color)
        {
            if (IsPointInArray(x - 1, y) && array[y, x - 1])
            {
                bitmap.SetPixel(x * 5, y * 5 + 1, color);
                bitmap.SetPixel(x * 5, y * 5 + 2, color);
                bitmap.SetPixel(x * 5, y * 5 + 3, color);
            }
            if (IsPointInArray(x + 1, y) && array[y, x + 1])
            {
                bitmap.SetPixel(x * 5 + 4, y * 5 + 1, color);
                bitmap.SetPixel(x * 5 + 4, y * 5 + 2, color);
                bitmap.SetPixel(x * 5 + 4, y * 5 + 3, color);
            }
            if (IsPointInArray(x, y - 1) && array[y - 1, x])
            {
                bitmap.SetPixel(x * 5 + 1, y * 5, color);
                bitmap.SetPixel(x * 5 + 2, y * 5, color);
                bitmap.SetPixel(x * 5 + 3, y * 5, color);
            }
            if (IsPointInArray(x, y + 1) && array[y + 1, x])
            {
                bitmap.SetPixel(x * 5 + 1, y * 5 + 4, color);
                bitmap.SetPixel(x * 5 + 2, y * 5 + 4, color);
                bitmap.SetPixel(x * 5 + 3, y * 5 + 4, color);
            }
        }

        private bool IsPointInArray(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}
