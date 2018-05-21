using Android.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    class MazeConstructor
    {
        public bool[,] StepThresholding { get; private set; }
        public bool[,] StepRepair { get; private set; }
        public bool[,] StepDestroyShape { get; private set; }
        public bool[,] StepGeometry { get; private set; }
        public bool[,] StepMaze { get; private set; }
        public bool IsMazeCorrect { get; private set; }

        private readonly MazeConstructorPreference preference;

        public MazeConstructor(Bitmap bitmap, MazeConstructorPreference preference)
        {
            this.preference = preference;

            Thresholding(bitmap);
            Repeir();

            ShapeDetector shapeDetector = new ShapeDetector(StepRepair,preference);
            StepDestroyShape = shapeDetector.ConvertedSource;

            LineDetector lineDetector = new LineDetector(StepDestroyShape, preference);
            StepGeometry = lineDetector.StepGeometry;

            try
            {
                LineMerger lineMerger = new LineMerger(lineDetector.VerticalLines, lineDetector.HorizontalLines, preference);
                StepMaze = lineMerger.LastStep;
                IsMazeCorrect = CheckIsMazeCorrect();
            }
            catch
            {
                StepMaze = new bool[1, 1];
            }
            
        }

        #region Thresholding

        private bool IsPixelSelected(int pixel)
        {
            int avg = (Color.GetBlueComponent(pixel) + Color.GetBlueComponent(pixel) + Color.GetBlueComponent(pixel)) / 3;
            return avg <= preference.Thresholding;
        }

        private void Thresholding(Bitmap bitmap)
        {
            StepThresholding = new bool[bitmap.Height,bitmap.Width];

            for(int i=0; i<bitmap.Height; ++i)
                for(int j=0; j<bitmap.Width; ++j)
                    StepThresholding[i, j] = IsPixelSelected(bitmap.GetPixel(j, i));
        }

        #endregion

        #region Cellular algorithm

        private void Repeir()
        {
            StepRepair = (bool[,])StepThresholding.Clone();

            for (int i = 0; i < preference.RepeirIteration; ++i)
                CellularAlgorithm();
        }

        private void CellularAlgorithm()
        {
            bool[,] newCellularArray = new bool[StepRepair.GetLength(0), StepRepair.GetLength(1)];

            for (int i = 0; i < StepRepair.GetLength(0); ++i)
            {
                for (int j = 0; j < StepRepair.GetLength(1); ++j)
                {
                    int neighbors = NeighborsCount(j, i);

                    if (StepRepair[i, j])
                        newCellularArray[i, j] = (neighbors >= 2);
                    else
                        newCellularArray[i, j] = neighbors > 4;
                }
            }

            StepRepair = newCellularArray;
        }

        private int NeighborsCount(int x, int y)
        {
            int neighbors = 0;

            if (y - 1 >= 0 && StepRepair[y - 1, x]) //N
                ++neighbors;
            if (y + 1 < StepRepair.GetLength(0) && StepRepair[y + 1, x]) //S
                ++neighbors;
            if (x - 1 >= 0 && StepRepair[y, x - 1]) //W
                ++neighbors;
            if (x + 1 < StepRepair.GetLength(1) && StepRepair[y, x + 1]) //E
                ++neighbors;

            if (y - 1 >= 0 && x - 1 > 0 && StepRepair[y - 1, x - 1]) //NW
                ++neighbors;
            if (y - 1 >= 0 && x + 1 < StepRepair.GetLength(1) && StepRepair[y - 1, x + 1]) //NE
                ++neighbors;
            if (y + 1 < StepRepair.GetLength(0) && x - 1 >= 0 && StepRepair[y + 1, x - 1]) //SW
                ++neighbors;
            if (y + 1 < StepRepair.GetLength(0) && x + 1 < StepRepair.GetLength(1) && StepRepair[y + 1, x + 1]) //SE
                ++neighbors;

            return neighbors;
        }

        #endregion

        private bool CheckIsMazeCorrect()
        {
            if (StepMaze.GetLength(0) <= 3 || StepMaze.GetLength(1) <= 3)
                return false;

            int countOfEnterance = 0;

            for (int i = 1; i < StepMaze.GetLength(1) - 1; ++i)
            {
                if (!StepMaze[0, i])
                    ++countOfEnterance;
                if (!StepMaze[StepMaze.GetLength(0) - 1, i])
                    ++countOfEnterance;
            }

            for (int i = 1; i < StepMaze.GetLength(0) - 1; ++i)
            {
                if (!StepMaze[i, 0])
                    ++countOfEnterance;
                if (!StepMaze[i, StepMaze.GetLength(1) - 1])
                    ++countOfEnterance;
            }

            return countOfEnterance == 2;
        }
    }
}