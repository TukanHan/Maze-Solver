using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace MazeSolver
{
    class MyReference
    {
        public Func<Point> GetPoint;
        public Action<Point> SetPoint;

        public int X
        {
            get { return GetPoint().X; }
            set { SetPoint(new Point(value, GetPoint().Y)); }
        }

        public int Y
        {
            get { return GetPoint().Y; }
            set { SetPoint(new Point(GetPoint().X, value)); }
        }
    }

    class LineMerger
    {
        public bool[,] LastStep { get; private set; }

        private List<Line> verticalLines;
        private List<Line> horizontalLines;

        private readonly MazeConstructorPreference preference;

        public LineMerger(List<Line> verticalLines, List<Line> horizontalLines, MazeConstructorPreference preference)
        {
            this.preference = preference;
            this.verticalLines = new List<Line>(verticalLines);
            this.horizontalLines = new List<Line>(horizontalLines);

            GenerateMaze();
        }

        private void GenerateMaze()
        {
            PrepareXAxis();
            PrepareYAxis();

            GenerateArray();
        }

        private void PrepareXAxis()
        {
            List<MyReference> myPoints = GetPoints().OrderBy(o => o.X).ToList();

            int mazePassageIndex = 0;

            while(myPoints.Count>0)
            {
                int startingX = myPoints[0].X;

                double sum = 0;
                int index = 0;

                while (index< myPoints.Count)
                {
                    if (myPoints[index].X < startingX + preference.FieldLenght)
                        sum += myPoints[index++].X;
                    else
                        break;
                }

                double avg = sum / index;


                while (myPoints.Count>0)
                {
                    if (myPoints[0].X <= avg || Math.Abs(myPoints[0].X - avg) <= preference.FieldLenght/3*2)
                    {
                        myPoints[0].X = mazePassageIndex;
                        myPoints.RemoveAt(0);
                    }
                    else
                        break;
                }

                mazePassageIndex+=2;
            }
        }

        private void PrepareYAxis()
        {
            List<MyReference> myPoints = GetPoints().OrderBy(o => o.Y).ToList();

            int mazePassageIndex = 0;

            while (myPoints.Count > 0)
            {
                int startingY = myPoints[0].Y;

                double sum = 0;
                int index = 0;

                while (index < myPoints.Count)
                {
                    if (myPoints[index].Y < startingY + preference.FieldLenght)
                        sum += myPoints[index++].Y;
                    else
                        break;
                }

                double avg = sum / index;


                while (myPoints.Count > 0)
                {
                    if (myPoints[0].Y <= avg ||  Math.Abs(myPoints[0].Y - avg) <= preference.FieldLenght / 3 * 2)
                    {
                        myPoints[0].Y = mazePassageIndex;
                        myPoints.RemoveAt(0);
                    }
                    else
                        break;
                }

                mazePassageIndex += 2;
            }
        }

        private List<MyReference> GetPoints()
        {
            List<MyReference> myPoints = new List<MyReference>();
            for (int i = 0; i < verticalLines.Count; ++i)
            {
                int temp = i;
                myPoints.Add(new MyReference()
                {
                    GetPoint = () => { return verticalLines[temp].A; },
                    SetPoint = (p) => { verticalLines[temp] = new Line(p, verticalLines[temp].B); }
                });
                myPoints.Add(new MyReference()
                {
                    GetPoint = () => { return verticalLines[temp].B; },
                    SetPoint = (p) => { verticalLines[temp] = new Line(verticalLines[temp].A, p); }
                });
            }

            for (int i = 0; i < horizontalLines.Count; ++i)
            {
                int temp = i;
                myPoints.Add(new MyReference()
                {
                    GetPoint = () => { return horizontalLines[temp].A; },
                    SetPoint = (p) => { horizontalLines[temp] = new Line(p, horizontalLines[temp].B); }
                });
                myPoints.Add(new MyReference()
                {
                    GetPoint = () => { return horizontalLines[temp].B; },
                    SetPoint = (p) => { horizontalLines[temp] = new Line(horizontalLines[temp].A, p); }
                });
            }

            return myPoints;
        }

        private void GenerateArray()
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = 0, maxY = 0;

            foreach (Line line in verticalLines)
            {
                if (line.A.Y < minY) minY = line.A.Y;
                if (line.B.Y > maxY) maxY = line.B.Y;
                if (line.A.X < minX) minX = line.A.X;
                if (line.A.X > maxX) maxX = line.A.X;
            }

            foreach (Line line in horizontalLines)
            {
                if (line.A.X < minX) minX = line.A.X;
                if (line.B.X > maxX) maxX = line.B.X;
                if (line.A.Y < minY) minY = line.A.Y;
                if (line.A.Y > maxY) maxY = line.A.Y;
            }

            LastStep = new bool[maxY + 1, maxX + 1];

            foreach (Line line in verticalLines)
            {
                int x =  line.A.X;
                int y1 = line.A.Y;
                int y2 = line.B.Y;

                for (int i = y1; i <= y2; ++i)
                    LastStep[i, x] = true;
            }

            foreach (Line line in horizontalLines)
            {
                int x1 = line.A.X;
                int x2 = line.B.X;
                int y =  line.A.Y;

                for (int i = x1; i <= x2; ++i)
                    LastStep[y, i] = true;
            }
        }
    }
}
