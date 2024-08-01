using NaviCoffee.Models;
using NaviCoffee.ViewModels;
using Newtonsoft.Json;
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
    public partial class CoffeeCustomizationView : ContentPage
    {
        public CoffeeCustomizationViewModel _viewModel;

        public CoffeeCustomizationView(CoffeeCustomizationViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public Cart Cart { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SubscribeToMessages();
            _viewModel.LoadOrder();
        }

        protected override void OnDisappearing()
        {
            UnsubscribeFromMessages();
            base.OnDisappearing();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, Constants.MessagingCenterMessages.Checkout, (message) =>
            {
                CoffeePickupContentPage view = new CoffeePickupContentPage(_viewModel);                
                Navigation.PushAsync(view);
            });

            MessagingCenter.Subscribe<Coffee>(this, Constants.MessagingCenterMessages.Ingredient, (message) =>
            {
                var coffee = _viewModel.Coffees.FirstOrDefault(c => c.Name == message.Name);
                if (coffee != null)
                {
                    coffee.HasSyrup = message.HasSyrup;
                    coffee.HasMilk = message.HasMilk;
                }
            });

            MessagingCenter.Subscribe<string>(this, Constants.MessagingCenterMessages.CheckoutFailed, (message) =>
            {
                DisplayAlert("Uhoh", $"Sorry but an error occured on the server.  Please try again.", "I'll try");
            });            
        }

        private void UnsubscribeFromMessages()
        {
            MessagingCenter.Unsubscribe<string>(this, Constants.MessagingCenterMessages.Checkout);
            MessagingCenter.Unsubscribe<Coffee>(this, Constants.MessagingCenterMessages.Ingredient);
            MessagingCenter.Unsubscribe<string>(this, Constants.MessagingCenterMessages.CheckoutFailed);
        }


        async void TopSwipeItem_Invoked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();

        }

        void BottomSwipeItem_Invoked(System.Object sender, System.EventArgs e)
        {
            _viewModel.CheckoutCommand.Execute(this);
        }

    }
}