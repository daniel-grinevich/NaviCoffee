using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaviCoffee.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Controls
{
    [DesignTimeVisible(true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoffeeContentView : ContentView
    {
        public static readonly BindableProperty CoffeeIdProperty = BindableProperty.Create(nameof(CoffeeId), typeof(string), typeof(CoffeeContentView), "-1");
        public static readonly BindableProperty CoffeeNameProperty = BindableProperty.Create(nameof(CoffeeName), typeof(string), typeof(CoffeeContentView), "Latte");
        public static readonly BindableProperty CoffeeImageProperty = BindableProperty.Create(nameof(CoffeeImage), typeof(string), typeof(CoffeeContentView), "whitecoffeecup600.jpg");
        public static readonly BindableProperty CoffeePriceProperty = BindableProperty.Create(nameof(CoffeePrice), typeof(string), typeof(CoffeeContentView), "$2.5");
        public static readonly BindableProperty CoffeeSubtitleProperty = BindableProperty.Create(nameof(CoffeeSubtitle), typeof(string), typeof(CoffeeContentView), "Swipe left");
        public static readonly BindableProperty IngredientsVisibleProperty = BindableProperty.Create(nameof(IngredientsVisible), typeof(bool), typeof(CoffeeContentView), false);
        public static readonly BindableProperty HasSyrupProperty = BindableProperty.Create(nameof(HasSyrup), typeof(bool), typeof(CoffeeContentView), false);
        public static readonly BindableProperty HasMilkProperty = BindableProperty.Create(nameof(HasMilk), typeof(bool), typeof(CoffeeContentView), false);

        private double _containerHeight;
        private bool _containerVisible = false; 

        public CoffeeContentView()
        {
            InitializeComponent();

            _containerHeight = IngredentsFrame.Height;
            IngredentsFrame.HeightRequest = -50;

            BindingContext = this;
        }

        #region Properties
       
        public string CoffeeId
        {
            get => (string)GetValue(CoffeeIdProperty);
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

        public bool HasSyrup
        {
            get => (bool)GetValue(HasSyrupProperty);
            set
            {
                SetValue(HasSyrupProperty, value);

                addSyrup.IsToggled = value;
                removeSyrup.IsToggled = !addSyrup.IsToggled;
                MessagingCenter.Send(new Coffee { CoffeeId = int.Parse(CoffeeId), Name = CoffeeName, HasSyrup = this.HasSyrup, HasMilk = this.HasMilk }, Constants.MessagingCenterMessages.Ingredient);
                OnPropertyChanged();                
            }
        }

        public bool HasMilk
        {
            get => (bool)GetValue(HasMilkProperty);
            set
            {
                SetValue(HasMilkProperty, value);

                addMilk.IsToggled = value;
                removeMilk.IsToggled = !addMilk.IsToggled;
                MessagingCenter.Send(new Coffee { CoffeeId = int.Parse(CoffeeId), Name = CoffeeName, HasSyrup = this.HasSyrup, HasMilk = this.HasMilk }, Constants.MessagingCenterMessages.Ingredient); OnPropertyChanged();                
            }
        }

        void OnButtonToggled(object sender, ToggledEventArgs args)
        {

            ToggleButton toggleButton = sender as ToggleButton;

            if (toggleButton.Id == "addSyrup")
            {
                removeSyrup.IsToggled = !toggleButton.IsToggled;
                HasSyrup = addSyrup.IsToggled;
            }
            else if (toggleButton.Id == "addMilk")
            {
                removeMilk.IsToggled = !toggleButton.IsToggled;
                HasMilk = addMilk.IsToggled;
            }
            else if (toggleButton.Id == "removeSyrup")
            {
                addSyrup.IsToggled = !toggleButton.IsToggled;
                HasSyrup = addSyrup.IsToggled;
            }
            else if (toggleButton.Id == "removeMilk")
            {
                addMilk.IsToggled = !toggleButton.IsToggled;
                HasMilk = addMilk.IsToggled;
            }
        }

        void ShowIngredients_Tapped(System.Object sender, System.EventArgs e)
        {
            //IngredentsFrame.IsVisible = !IngredentsFrame.IsVisible;

            double start, end;
            this.AbortAnimation("ShowHideIngredients");

            // you can also check for `switch.IsToggled` if you prefer.

            if (_containerVisible)
            {
                _containerVisible = false;
                start = _containerHeight;
                end = -50;
            }
            else
            {
                _containerVisible = true;
                start = -50;
                end = _containerHeight;
            }

            var animation = new Animation(v => IngredentsFrame.HeightRequest = v, start, end, Easing.SinInOut);
            animation.Commit(this, "ShowHideIngredients");
        }

        #endregion

    }
}