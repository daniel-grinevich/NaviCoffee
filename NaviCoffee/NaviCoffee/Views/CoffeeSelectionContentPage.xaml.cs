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
    public partial class CoffeeSelectionContentPage : ContentPage
    {

        private CoffeeSelectionViewModel _viewModel;

        public CoffeeSelectionContentPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new CoffeeSelectionViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Coffees.Count == 0)
            {
                _viewModel.LoadItemsCommand.Execute(null);
            }
            SubscribeToMessages();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeFromMessages();
        }

        private void UnsubscribeFromMessages()
        {
            MessagingCenter.Unsubscribe<Coffee>(this, Constants.MessagingCenterMessages.Added);
            MessagingCenter.Unsubscribe<string>(this, Constants.MessagingCenterMessages.TopMenuSwiped);            
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<Coffee>(this, Constants.MessagingCenterMessages.Added, (coffee) =>
            {                
                _viewModel.Cart.Coffee.Add(coffee);
                DisplayAlert("Added to your order", $"{coffee.Label} has been added to your order.  Swipe up on 'Customize' to add ingredients.", "Ok");
            });

            MessagingCenter.Subscribe<string>(this, Constants.MessagingCenterMessages.TopMenuSwiped, (message) =>
            {
                DisplayAlert("Ooops", $"There's nothing to do here yet.", "Noted");
            });

        }


        void TopSwipeItem_Invoked(System.Object sender, System.EventArgs e)
        {
            DisplayAlert("Ooops", $"There's nothing to do here yet.", "Noted");
        }

        async void BottomSwipeItem_Invoked(System.Object sender, System.EventArgs e)
        {
            if (!_viewModel.Cart.Coffee.Any())
            {
                await DisplayAlert("Ooops", $"Please add some coffee to your order first.", "Ok");
                return;
            }

            CoffeeCustomizationViewModel vm = new CoffeeCustomizationViewModel(_viewModel.Cart);
            CoffeeCustomizationView view = new CoffeeCustomizationView(vm);            
            
            await Navigation.PushAsync(view);
        }
    }
}
