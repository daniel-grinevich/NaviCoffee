using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoffeeContentPage : ContentPage
    {
        public CoffeeContentPage()
        {
            InitializeComponent();
        }

        private void SwipeItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}