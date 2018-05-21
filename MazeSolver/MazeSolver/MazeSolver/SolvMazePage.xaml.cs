using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazeSolver
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SolvMazePage : ContentPage
	{
        private readonly bool[,] maze;

		public SolvMazePage(bool[,] maze)
		{
            this.maze = maze;
			InitializeComponent();
            SetImage();
		}

        private void SetImage()
        {
            MazeSolver mazeSolver = new MazeSolver(maze);
            MazeImageConstructor imageConstructor = new MazeImageConstructor(maze, mazeSolver.Route, 3);
            MazePhoto.Source = imageConstructor.ImageSource;
        }
	}
}