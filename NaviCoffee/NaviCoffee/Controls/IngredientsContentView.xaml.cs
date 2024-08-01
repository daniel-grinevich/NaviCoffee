using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Controls
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngredientsContentView : ContentView
    {
        public static readonly BindableProperty IngredientsVisibleProperty = BindableProperty.Create(nameof(IngredientsVisible), typeof(bool), typeof(IngredientsContentView), false);

        public IngredientsContentView()
        {
            InitializeComponent();
        }

        #region Properties
        public bool IngredientsVisible
        {
            get => (bool)GetValue(IngredientsVisibleProperty);
            set => SetValue(IngredientsVisibleProperty, value);
        }


        #endregion
    }
}