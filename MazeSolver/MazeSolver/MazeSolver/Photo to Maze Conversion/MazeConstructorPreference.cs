using System;
using System.Collections.Generic;
using System.Text;

namespace MazeSolver
{
    struct MazeConstructorPreference
    {
        public double Thresholding { get; }
        public int RepeirIteration { get; }
        public int MinimalShapeSize { get; }
        public int LineThicknes { get; }
        public int FieldLenght { get; }

        public MazeConstructorPreference(double thresholding, int repeirIteration, int minimalShapeSize,
                                            int lineThicknes, int fieldLenght)
        {
            Thresholding = thresholding;
            RepeirIteration = repeirIteration;
            MinimalShapeSize = minimalShapeSize;
            LineThicknes = lineThicknes;
            FieldLenght = fieldLenght;
        }
    }
}
