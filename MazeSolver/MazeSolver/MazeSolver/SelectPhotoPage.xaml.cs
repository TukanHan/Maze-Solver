using MazeSolver.Droid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MazeSolver
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectPhotoPage : ContentPage
	{
        MainActivity activity = (MainActivity)Forms.Context;

        public SelectPhotoPage ()
		{
			InitializeComponent ();
		}    

        private void TakeAPhoto(object sender, EventArgs e)
        {
            activity.TakeAPhoto();
        }

        private void FindAPhoto(object sender, EventArgs e)
        {
            activity.FindAPhoto();
        }

        private async void AboutApp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutAppPage());
        }

        private async void Instruction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructionPage());
        }
    }
}