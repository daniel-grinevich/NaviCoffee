using NaviCoffee.Models;
using NaviCoffee.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NaviCoffee.ViewModels
{
    public class CoffeeSelectionViewModel : BaseViewModel
    {
        #region Private members
        Cart _cart;
        ObservableCollection<Coffee> _coffee;
        #endregion

        #region Constructors  
        public CoffeeSelectionViewModel ()
        {
            Coffees = new ObservableCollection<Coffee>();
            Cart = new Cart();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddtoCartCommand = new Command<Coffee>(async (coffee) => await ExecuteAddToCartCommand(coffee));
            CustomizeCommand = new Command(() => ExecuteCustomizeCommand());
            BackCommand = new Command(() => ExecuteBackCommand());
        }
        #endregion

        #region Properties
        public ObservableCollection<Coffee> Coffees
        {
            get { return _coffee; }
            set
            {
                _coffee = value;
                OnPropertyChanged();
            }
        }

        public Cart Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public Command LoadItemsCommand { get; set; }
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Coffees.Clear();
                var coffees = await CoffeeStore.GetItemsAsync(true);
                foreach (var coffee in coffees)
                {
                    Coffees.Add(coffee);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command<Coffee> AddtoCartCommand { get; set; }
        async Task ExecuteAddToCartCommand(Coffee coffee)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await CoffeeStore.AddItemToCartAsync(coffee);
                Cart.Coffee.Add(coffee);
                MessagingCenter.Send(coffee, Constants.MessagingCenterMessages.Added);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command BackCommand { get; set; }
        void ExecuteBackCommand()
        {
            MessagingCenter.Send("back", Constants.MessagingCenterMessages.TopMenuSwiped);            
        }

        public Command CustomizeCommand { get; set; }
        void ExecuteCustomizeCommand()
        {
            MessagingCenter.Send("customize", Constants.MessagingCenterMessages.ButtomMenuSwiped);
        }
        #endregion
    }
}
