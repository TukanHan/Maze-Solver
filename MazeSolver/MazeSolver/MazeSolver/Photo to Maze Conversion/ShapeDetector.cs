using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MazeSolver
{
    class ShapeDetector
    {
        public bool[,] ConvertedSource { get; private set; }

        private readonly MazeConstructorPreference preference;

        private readonly int width;
        private readonly int height;
        private bool[,] markedField;

        //Queue on array
        private Point[] queue;
        private int firstStart;
        private int start = 0;
        private int end = 0;

        public ShapeDetector(bool[,] source, MazeConstructorPreference preference)
        {
            this.preference = preference;

            height = source.GetLength(0);
            width = source.GetLength(1);

            ConvertedSource = (bool[,])source.Clone();

            markedField = new bool[height, width];
            queue = new Point[source.Length];

            DeleteDisturbingShape();
        }

        #region Queue Action
        private void AddToQueue(Point point)
        {
            queue[end++] = point;
        }

        private Point GetFromQueue()
        {
            return queue[start++];
        }

        private int GetQueueLenght()
        {
            return end - start;
        }
        #endregion

        private bool IsPoinInArray(Point point)
        {
            return point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
        }

        private bool IsPointOnEdge(Point point)
        {
            return point.X == 0 || point.X == width - 1 || point.Y == 0 || point.Y == height - 1;
        }

        private void AddPointIfIsCorrect(Point point)
        {
            if (IsPoinInArray(point) && ConvertedSource[point.Y, point.X] && !markedField[point.Y, point.X])
            {
                AddToQueue(point);
                markedField[point.Y, point.X] = true;
            }
        }

        private void DeleteDisturbingShape()
        {
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    if (ConvertedSource[i, j] && !markedField[i, j])
                        FindShape(i, j);
        }

        private void FindShape(int i, int j)
        {
            firstStart = start;

            AddToQueue(new Point(j, i));
            markedField[i, j] = true;

            bool isPointOnEdge = false;

            while (GetQueueLenght() > 0)
            {
                Point point = GetFromQueue();

                if (!isPointOnEdge)
                    isPointOnEdge = IsPointOnEdge(point);

                AddPointIfIsCorrect(new Point(point.X + 1, point.Y));
                AddPointIfIsCorrect(new Point(point.X - 1, point.Y));
                AddPointIfIsCorrect(new Point(point.X, point.Y - 1));
                AddPointIfIsCorrect(new Point(point.X, point.Y + 1));
            }

            if (isPointOnEdge || end - firstStart < preference.MinimalShapeSize)
            {
                for(int l= firstStart; l<end; ++l)
                    ConvertedSource[queue[l].Y, queue[l].X] = false;
            }
        }
    }
}
