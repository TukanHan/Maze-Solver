using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MazeSolver
{
    class MazeSolver
    {
        public bool[,] Route { get; private set; }

        private readonly bool[,] maze;
        private readonly int width;
        private readonly int height;

        private Point start;
        private Point end;

        public MazeSolver(bool[,] maze)
        {
            this.maze = maze;
            height = maze.GetLength(0);
            width = maze.GetLength(1);

            FindStartAndEndPoints();

            List<List<Point>> possibleMoves = new List<List<Point>>();

            Route = new bool[height, width];

            if (FindRoute(possibleMoves))
                MarkRoute(possibleMoves);
        }

        private void FindStartAndEndPoints()
        {
            Point[] points = new Point[2];
            int currentPointIndex = 0;

            for (int i = 1; i < width - 1; ++i)
            {
                if (!maze[0, i])
                    points[currentPointIndex++] = new Point(i, 0);
                if (!maze[height - 1, i])
                    points[currentPointIndex++] = new Point(i, height - 1);
            }

            for (int i = 1; i < height - 1; ++i)
            {
                if (!maze[i, 0])
                    points[currentPointIndex++] = new Point(0, i);
                if (!maze[i, width - 1])
                    points[currentPointIndex++] = new Point(width - 1, i);
            }

            start = points[0];
            end = points[1];
        }

        private int[,] ObstacleMap()
        {
            int[,] pathArray = new int[height, width];

            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    if (maze[i, j])
                        pathArray[i, j] = -1;

            return pathArray;
        }

        private bool IsPointInArray(Point point)
        {
            return point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
        }

        private bool FindRoute(List<List<Point>> possibleMoves)
        {
            int[,] pathArray = ObstacleMap();

            possibleMoves.Add(new List<Point>());
            possibleMoves[0].Add(start);
            pathArray[start.Y, start.X] = 1;

            bool foundRoute = false;
            int index = 0;

            while (!foundRoute && possibleMoves[index].Count>0)
            {
                possibleMoves.Add(new List<Point>());

                for (int i = 0; i < possibleMoves[index].Count; ++i)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        Point tempPoint;

                        if (j == 3)
                            tempPoint = new Point(possibleMoves[index][i].X, possibleMoves[index][i].Y + 1);
                        else if (j == 2)
                            tempPoint = new Point(possibleMoves[index][i].X, possibleMoves[index][i].Y - 1);
                        else if (j == 1)
                            tempPoint = new Point(possibleMoves[index][i].X + 1, possibleMoves[index][i].Y);
                        else
                            tempPoint = new Point(possibleMoves[index][i].X - 1, possibleMoves[index][i].Y); 

                        if (IsPointInArray(tempPoint) && pathArray[tempPoint.Y, tempPoint.X] == 0)
                        {
                            pathArray[tempPoint.Y, tempPoint.X] = index + 2;
                            possibleMoves[index + 1].Add(tempPoint);

                            if (tempPoint.Equals(end))
                                foundRoute = true;
                        }
                    }
                }

                ++index;
            }

            return foundRoute;
        }

        private void MarkRoute(List<List<Point>> possibleMoves)
        {
            Point lastPoint = end;
            Route[lastPoint.Y, lastPoint.X] = true;

            for (int index = possibleMoves.Count - 2; index >= 0; --index)
            {
                foreach (Point point in possibleMoves[index])
                {
                    if (new Point(point.X, point.Y + 1).Equals(lastPoint) ||
                        new Point(point.X, point.Y - 1).Equals(lastPoint) ||
                        new Point(point.X + 1, point.Y).Equals(lastPoint) ||
                        new Point(point.X - 1, point.Y).Equals(lastPoint))
                    {
                        lastPoint = point;
                        Route[lastPoint.Y, lastPoint.X] = true;
                        break;
                    }
                }
            }
        }
    }
}
