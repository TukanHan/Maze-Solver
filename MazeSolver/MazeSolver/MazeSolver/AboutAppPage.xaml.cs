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
	public partial class AboutAppPage : ContentPage
	{
		public AboutAppPage ()
		{
			InitializeComponent ();
            linkLabel.GestureRecognizers.Add(new TapGestureRecognizer((view) => Hyperlink("https://github.com/TukanHan/Maze-Solver")));
		}

        private void Hyperlink(string link)
        {
            Device.OpenUri(new Uri(link));
        }
    }
}