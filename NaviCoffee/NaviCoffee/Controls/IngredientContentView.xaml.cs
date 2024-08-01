using NaviCoffee.Models;
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
    public partial class IngredientContentView : ContentView
    {
        public static readonly BindableProperty IngredientNameProperty = BindableProperty.Create(nameof(IngredientName), typeof(string), typeof(IngredientContentView), Constants.Images.IngredientName);

        public IngredientContentView()
        {
            InitializeComponent();
        }

        #region Properties
        public string IngredientName
        {
            get => (string)GetValue(IngredientNameProperty);
            set => SetValue(IngredientNameProperty, value);
        }

       
        #endregion

    }
}