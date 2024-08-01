using NaviCoffee.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Controls
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwipeableCoffee : ContentView
    {
        public static readonly BindableProperty CoffeeIdProperty = BindableProperty.Create(nameof(CoffeeId), typeof(int), typeof(SwipeableCoffee), 0);
        public static readonly BindableProperty CoffeeNameProperty = BindableProperty.Create(nameof(CoffeeName), typeof(string), typeof(SwipeableCoffee), "Latte");
        public static readonly BindableProperty CoffeeImageProperty = BindableProperty.Create(nameof(CoffeeImage), typeof(string), typeof(SwipeableCoffee), "whitecoffeecup600.jpg");
        public static readonly BindableProperty CoffeePriceProperty = BindableProperty.Create(nameof(CoffeePrice), typeof(string), typeof(SwipeableCoffee), "$2.5");
        public static readonly BindableProperty CoffeeSubtitleProperty = BindableProperty.Create(nameof(CoffeeSubtitle), typeof(string), typeof(SwipeableCoffee), "Swipe left");
        public static readonly BindableProperty IngredientsVisibleProperty = BindableProperty.Create(nameof(IngredientsVisible), typeof(bool), typeof(SwipeableCoffee), false);


        public SwipeableCoffee()
        {
            InitializeComponent();
        }

        #region Properties        
        public int CoffeeId
        {
            get => (int)GetValue(CoffeeIdProperty);
            set => SetValue(CoffeeIdProperty, value);
        }

        public string CoffeeName
        {
            get => (string)GetValue(CoffeeNameProperty);
            set => SetValue(CoffeeNameProperty, value);
        }

        public string CoffeeImage
        {
            get => (string)GetValue(CoffeeImageProperty);
            set => SetValue(CoffeeImageProperty, value);
        }

        public string CoffeePrice
        {
            get => (string)GetValue(CoffeePriceProperty);
            set => SetValue(CoffeePriceProperty, value);
        }

        public string CoffeeSubtitle
        {
            get => (string)GetValue(CoffeeSubtitleProperty);
            set => SetValue(CoffeeSubtitleProperty, value);
        }

        public bool IngredientsVisible
        {
            get => (bool)GetValue(IngredientsVisibleProperty);
            set => SetValue(IngredientsVisibleProperty, value);
        }

        #endregion

        private void SwipeItemView_Invoked(object sender, EventArgs e)
        {
            MessagingCenter.Send(new Coffee
            {
                CoffeeId = CoffeeId, Label = CoffeeName, Name = CoffeeName, Image = CoffeeImage,
                Price = decimal.Parse(CoffeePrice, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint)
            },
            Constants.MessagingCenterMessages.Added);
        }

        void SwipeItemView_Delete_Invoked(System.Object sender, System.EventArgs e)
        {
        }
    }
}