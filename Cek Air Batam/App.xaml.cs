using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: ExportFont("Product Sans Bold Italic.ttf")]
[assembly: ExportFont("Product Sans Bold.ttf")]
[assembly: ExportFont("Product Sans Italic.ttf")]
[assembly: ExportFont("Product Sans Regular.ttf")]
namespace Cek_Air_Batam
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
