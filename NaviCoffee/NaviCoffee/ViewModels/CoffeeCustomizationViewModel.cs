using NaviCoffee.Models;
using NaviCoffee.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NaviCoffee.ViewModels
{
    public class CoffeeCustomizationViewModel : BaseViewModel
    {
        #region Private members       
        ObservableCollection<Coffee> _coffee;
        Cart _cart;
        #endregion

        #region Constructors  
        public CoffeeCustomizationViewModel(Cart cart)
        {
            Cart = cart;
            Order = new Order();            
            LoadOrder();

            CheckoutCommand = new Command(async () => await ExecuteCheckoutCommand());
            BackCommand = new Command(() => ExecuteBackCommand());
            ShowHideIngredientsCommand = new Command(() => ExecuteShowHideIngredientsCommand());

        }
        #endregion

        #region Properties
        public Order Order { get; set; }

        public Cart Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Coffee> Coffees
        {
            get { return _coffee; }
            set
            {
                _coffee = value;
                OnPropertyChanged("Coffees");
            }
        }

        #endregion
        #region Commands
        public Command BackCommand { get; set; }
        void ExecuteBackCommand()
        {
            Shell.Current.GoToAsync($"//coffeeMenu");
        }

        public Command ShowHideIngredientsCommand { get; set; }
        void ExecuteShowHideIngredientsCommand()
        {
            foreach (var c in _coffee)
            {
                c.IsVisibleIngredients = true;
                OnPropertyChanged("Coffees");

            }
        }

        public Command CheckoutCommand { get; set; }
        async Task ExecuteCheckoutCommand()
        {
            Order = new Order
            {
                CustomerId = 1, // TODO: customer signup and authentication
                OrderId = Guid.NewGuid().ToString(),                
                Coffees = Coffees.ToList(),
                OrderDate = DateTime.UtcNow,
            };
            
            Coffee coffee = Coffees.First(); //TODO: Allow multiple coffees per order at machine.
            coffee.Barcode = Guid.NewGuid().ToString();

            Order = await CoffeeStore.AddOrderAsync(Order);
            if (Order.Status == Order.OrderStatus.SentToWebServer)
            {
                MessagingCenter.Send("checkout", Constants.MessagingCenterMessages.Checkout);
            }
            else
            {
                MessagingCenter.Send("checkout failed", Constants.MessagingCenterMessages.CheckoutFailed);
            }
        }
        #endregion

        #region Methods
        public void LoadOrder()
        {
            Coffees = new ObservableCollection<Coffee>();
            
            foreach (var coffee in Cart.Coffee)
            {
                Coffees.Add(coffee);                
            }
            OnPropertyChanged("Coffees");

        }

        internal void Reset()
        {
            Cart.Coffee.Clear();
        }
        #endregion
    }
}
