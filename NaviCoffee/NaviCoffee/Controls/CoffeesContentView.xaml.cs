using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaviCoffee.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoffeesContentView : ContentView
    {
        
        public CoffeesContentView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<Coffee>), typeof(CoffeesContentView), null,
                                     BindingMode.Default, null, OnItemsSourceChanged);

        public ObservableCollection<Coffee> ItemsSource
        {
            get { return (ObservableCollection<Coffee>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            //System.Diagnostics.Debug.WriteLine("source changed");
        }

        private void SwipeItemView_Invoked(object sender, EventArgs e)
        {

        }
    }
}