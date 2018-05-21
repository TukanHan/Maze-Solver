using Android.Graphics;
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
    public partial class EditMazeTabPage : TabbedPage
    {
        private Bitmap bitmap;
        private bool[,] maze;

        public double Thresholding { get; set; } = 100;
        public int RepeirIteration { get; set; } = 5;
        public int MinimalShapeSize { get; set; } = 10;
        public int LineThicknes { get; set; } = 8;
        public int FieldLenght { get; set; } = 8;

        public EditMazeTabPage (Bitmap bitmap)
        {
            this.bitmap = bitmap;

            BindingContext = this;
            InitializeComponent();

            SetImages();
        }

        private void SetImages()
        {
            MazeConstructorPreference preference = new MazeConstructorPreference(Thresholding, RepeirIteration, MinimalShapeSize, LineThicknes, FieldLenght);
            MazeConstructor mazeConstructor = new MazeConstructor(bitmap, preference);

            maze = mazeConstructor.IsMazeCorrect ? mazeConstructor.StepMaze : null;

            AcceptImageButton.IsEnabled = mazeConstructor.IsMazeCorrect;

            ImageConstructor imageConstructor = new MazeImageConstructor(mazeConstructor.StepMaze, 3);
            Photo1.Source = imageConstructor.ImageSource;

            imageConstructor = new ImageConstructor(mazeConstructor.StepThresholding);
            Photo2.Source = imageConstructor.ImageSource;

            imageConstructor = new ImageConstructor(mazeConstructor.StepRepair);
            Photo3.Source = imageConstructor.ImageSource;

            imageConstructor = new ImageConstructor(mazeConstructor.StepDestroyShape);
            Photo4.Source = imageConstructor.ImageSource;

            imageConstructor = new ImageConstructor(mazeConstructor.StepGeometry);
            Photo5.Source = imageConstructor.ImageSource;
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            SetImages();
        }

        private async void SolvMaze(object sender, EventArgs e)
        {
            if(maze != null)
                await Navigation.PushAsync(new SolvMazePage(maze));
        }
    }
}