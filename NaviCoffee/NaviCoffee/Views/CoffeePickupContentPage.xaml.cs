using System;
using System.Collections.Generic;
using System.Linq;
using NaviCoffee.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaviCoffee.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoffeePickupContentPage : ContentPage
    {
        public CoffeeCustomizationViewModel _viewModel;

        public CoffeePickupContentPage(CoffeeCustomizationViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;

            // TODO: Remember that currently you can only make one coffee per order at the machine.
            //       Add ability to process multiple coffees per order.
            BarcodeImageView.BarcodeValue = _viewModel.Order.Coffees.First().Barcode; 
        }

        public string Barcode { get; set; }

        async void StartOver_Clicked(System.Object sender, System.EventArgs e)
        {
            _viewModel.Reset();

            await Navigation.PopToRootAsync();            
        }
    }
}
