using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MazeSolver
{
    struct Line
    {
        public Point A;
        public Point B;

        public Line(Point a, Point b)
        {
            A = a;
            B = b;
        }

        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
        }

        public int GetPosXMidle()
        {
            return (A.X + B.X) >> 1;
        }

        public int GetPosYMidle()
        {
            return (A.Y + B.Y) >> 1;
        }
    }

    class LineDetector
    {
        private enum Horizontal { Up = -1, Down = 1 };
        private enum Vertical { Left = -1, Right = 1};

        public bool[,] StepGeometry { get; private set; }

        public List<Line> VerticalLines { get; private set; } = new List<Line>();
        public List<Line> HorizontalLines { get; private set; } = new List<Line>();

        private readonly MazeConstructorPreference preference;

        private bool[,] currentSource;
        private readonly int width;
        private readonly int height;

        public LineDetector(bool[,] source, MazeConstructorPreference preference)
        {
            this.preference = preference;

            height = source.GetLength(0);
            width = source.GetLength(1);

            currentSource = (bool[,])source.Clone();
            ReadVerticalLines();

            currentSource = (bool[,])source.Clone();
            ReadHorizontalLines();

            GenerateMaze();
        }

        private void ReadVerticalLines()
        {
            for(int i=0; i<width; ++i)
            {
                for(int j=0; j<height; ++j)
                {
                    if(currentSource[j,i])
                    {
                        Line line = GetVerticalLine(new Point(i + (preference.LineThicknes>>1), j));
                        if (line.GetLength() > preference.FieldLenght * 1.5)
                        {
                            line.A.X = line.B.X = GetVerticalLineMidle(line);
                            VerticalLines.Add(line);
                            DestroyVerticalLine(line.A, line.B.Y - line.A.Y);
                        }
                    }
                }
            }
        }

        private void ReadHorizontalLines()
        {
            for (int j = 0; j < height; ++j)     
            {
                for (int i = 0; i < width; ++i)
                {
                    if (currentSource[j, i])
                    {
                        Line line = GetHorizontalLine(new Point(i, j + (preference.LineThicknes >> 1)));
                        if (line.GetLength() > preference.FieldLenght * 1.5)
                        {
                            line.A.Y = line.B.Y = GetHorizontalLineMidle(line);
                            HorizontalLines.Add(line);
                            DestroyHorizontalLine(line.A, line.B.X - line.A.X);
                        }
                    }
                }
            }
        }

        private Line GetVerticalLine(Point point)
        {
            Point A = GetNextVerticalPoint(point, Horizontal.Up);
            Point B = GetNextVerticalPoint(point, Horizontal.Down);

            return new Line(A,B);
        }

        private Line GetHorizontalLine(Point point)
        {
            Point A = GetNextHorizontalPoint(point, Vertical.Left);
            Point B = GetNextHorizontalPoint(point, Vertical.Right);

            return new Line(A, B);
        }

        private Point GetNextVerticalPoint(Point point, Horizontal UpOrDown)
        {
            Point nextPoint = new Point(point.X, point.Y);

            bool isLeftPoint, isMidPoint, isRightPoint;

            do
            {
                Point leftPoint = new Point(nextPoint.X - 1, nextPoint.Y + (int)UpOrDown);
                isLeftPoint = IsPoinInArray(leftPoint) && currentSource[leftPoint.Y, leftPoint.X];

                Point midlePoint = new Point(nextPoint.X, nextPoint.Y + (int)UpOrDown);
                isMidPoint = IsPoinInArray(midlePoint) && currentSource[midlePoint.Y, midlePoint.X];

                Point rightPoint = new Point(nextPoint.X + 1, nextPoint.Y + (int)UpOrDown);
                isRightPoint = IsPoinInArray(rightPoint) && currentSource[rightPoint.Y, rightPoint.X];

                if (nextPoint.X == point.X)
                {
                    if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isLeftPoint)
                        nextPoint = leftPoint;
                    else if (isRightPoint)
                        nextPoint = rightPoint;
                }
                else if(nextPoint.X > point.X)
                {
                    if (isLeftPoint)
                        nextPoint = leftPoint;
                    else if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isRightPoint && nextPoint.X - point.X <= preference.LineThicknes / 3)
                        nextPoint = rightPoint;
                    else break;
                }
                else if (nextPoint.X < point.X)
                {
                    if (isRightPoint)
                        nextPoint = rightPoint;
                    else if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isLeftPoint && point.X - nextPoint.X <= preference.LineThicknes / 3)
                        nextPoint = leftPoint;
                    else
                        break;
                }
            }
            while (isLeftPoint || isMidPoint || isRightPoint);

            return nextPoint;
        }

        private Point GetNextHorizontalPoint(Point point, Vertical LeftOrRight)
        {
            Point nextPoint = new Point(point.X, point.Y);

            bool isUpPoint, isMidPoint, isDownPoint;

            do
            {
                Point upPoint = new Point(nextPoint.X + (int)LeftOrRight , nextPoint.Y - 1);
                isUpPoint = IsPoinInArray(upPoint) && currentSource[upPoint.Y, upPoint.X];

                Point midlePoint = new Point(nextPoint.X + (int)LeftOrRight, nextPoint.Y);
                isMidPoint = IsPoinInArray(midlePoint) && currentSource[midlePoint.Y, midlePoint.X];

                Point downPoint = new Point(nextPoint.X + (int)LeftOrRight, nextPoint.Y + 1);
                isDownPoint = IsPoinInArray(downPoint) && currentSource[downPoint.Y, downPoint.X];

                if (nextPoint.Y == point.Y)
                {
                    if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isUpPoint)
                        nextPoint = upPoint;
                    else if (isDownPoint)
                        nextPoint = downPoint;
                }
                else if (nextPoint.Y > point.Y)
                {
                    if (isUpPoint)
                        nextPoint = upPoint;
                    else if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isDownPoint && nextPoint.Y - point.Y <= preference.LineThicknes / 3)
                        nextPoint = downPoint;
                    else break;
                }
                else if (nextPoint.Y < point.Y)
                {
                    if (isDownPoint)
                        nextPoint = downPoint;
                    else if (isMidPoint)
                        nextPoint = midlePoint;
                    else if (isUpPoint && point.Y - nextPoint.Y <= preference.LineThicknes / 3)
                        nextPoint = upPoint;
                    else
                        break;
                }
            }
            while (isUpPoint || isMidPoint || isDownPoint);

            return nextPoint;
        }

        private int GetVerticalLineMidle(Line line)
        {
            int height = line.B.Y - line.A.Y;
            int part = (int)Math.Round((double)height / preference.FieldLenght)<<1;

            int leftSum = 0, leftCount = 0, rightSum = 0, rightCount = 0;

            int X = line.GetPosXMidle();

            for (int i=1; i<part; ++i)
            {
                int Y = line.A.Y + (height / part * i);

                int pointToRight, pointToLeft = pointToRight = currentSource[Y, X] ? X : X + preference.LineThicknes / 3;
                
                while (IsPoinInArray(new Point(pointToLeft-1,Y)) && currentSource[Y, pointToLeft - 1])
                    --pointToLeft;

                if(X- pointToLeft < preference.LineThicknes)
                {
                    leftSum += pointToLeft;
                    ++leftCount;
                }

                while (IsPoinInArray(new Point(pointToRight + 1, Y)) && currentSource[Y, pointToRight + 1])
                    ++pointToRight;

                if (pointToRight - X < preference.LineThicknes)
                {
                    rightSum += pointToRight;
                    ++rightCount;
                }
            }

            if(leftCount == 0 && rightCount ==0)
                return X;
            else if(leftCount == 0)
                return (rightSum / rightCount) - (preference.LineThicknes >> 1);
            else if (rightCount == 0)
                return (leftSum / leftCount) + (preference.LineThicknes >> 1);
            else
                return ((leftSum / leftCount) + (rightSum / rightCount)) >> 1;
        }

        private int GetHorizontalLineMidle(Line line)
        {
            int height = line.B.X - line.A.X;
            int part = (int)(Math.Round((double)height / preference.FieldLenght)*2);

            int upSum = 0, upCount = 0, downSum = 0, downCount = 0;

            int Y = line.GetPosYMidle();

            for (int i = 1; i < part; ++i)
            {
                int X = line.A.X + (height / part * i);

                int pointToDown, pointToUp = pointToDown = currentSource[Y, X] ? Y : Y + preference.LineThicknes / 3;

                while (IsPoinInArray(new Point(X, pointToUp - 1)) && currentSource[pointToUp - 1, X])
                    --pointToUp;

                if (Y - pointToUp < preference.LineThicknes)
                {
                    upSum += pointToUp;
                    ++upCount;
                }

                while (IsPoinInArray(new Point(X, pointToDown + 1)) && currentSource[pointToDown + 1, X])
                    ++pointToDown;

                if (pointToDown - Y < preference.LineThicknes)
                {
                    downSum += pointToDown;
                    ++downCount;
                }
            }

            if (upCount == 0 && downCount == 0)
                return Y;
            else if (upCount == 0)
                return (downSum / downCount) - (preference.LineThicknes >> 1);
            else if (downCount == 0)
                return (upSum / upCount) + (preference.LineThicknes >> 1);
            else
                return ((upSum / upCount) + (downSum / downCount)) >> 1;
        }

        private void DestroyVerticalLine(Point start, int height)
        {
            int startX = start.X - preference.LineThicknes;
            if (startX < 0) startX = 0;

            int endX = start.X + preference.LineThicknes;
            if (endX >= width) endX = width - 1;

            for (int i= start.Y; i<= start.Y + height; ++i)
                for(int j=startX; j<=endX; ++j)
                    currentSource[i, j] = false;
        }

        private void DestroyHorizontalLine(Point start, int height)
        {
            int startY = start.Y - preference.LineThicknes;
            if (startY < 0) startY = 0;

            int endY = start.Y + preference.LineThicknes;
            if (endY >= width) endY = height - 1;

            for (int i = start.X; i <= start.X + height; ++i)
                for (int j = startY; j <= endY; ++j)
                    currentSource[j, i] = false;
        }

        private bool IsPoinInArray(Point point)
        {
            return point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
        }

        private void GenerateMaze()
        {
            StepGeometry = new bool[height, width];

            foreach (Line line in VerticalLines)
            {
                int startX = line.A.X - (preference.LineThicknes>>1);
                if (startX < 0) startX = 0;

                int endX = line.A.X + (preference.LineThicknes >> 1);
                if (endX >= width) endX = width - 1;

                for (int i= line.A.Y; i< line.B.Y; ++i)
                    for (int j = startX; j <= endX; ++j)
                        StepGeometry[i, j] = true;
            }

            foreach (Line line in HorizontalLines)
            {
                int startY = line.A.Y - (preference.LineThicknes >> 1);
                if (startY < 0) startY = 0;

                int endY = line.A.Y + (preference.LineThicknes >> 1);
                if (endY >= width) endY = height - 1;

                for (int i = line.A.X; i < line.B.X; ++i)
                    for (int j = startY; j <= endY; ++j)
                        StepGeometry[j, i] = true;
            }
        }
    }
}
