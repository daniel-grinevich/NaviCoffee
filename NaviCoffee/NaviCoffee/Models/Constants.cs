using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NaviCoffee.Models
{
    public static class Constants
    {
        public static class MessagingCenterMessages
        {
            public static string Added = "Added";
            public static string TopMenuSwiped = "TopMenuSwiped";
            public static string ButtomMenuSwiped = "Customize";
            public static string Checkout = "Checkout";
            public static string CheckoutFailed = "Checkout Failed";
            public static string Ingredient = "Ingredient";
        }

        public static class Images
        {
            public static string UpArrow = "uparrow.png";
            public static string DownArrow = "downarrow.png";
            public static string IngredientName = "INGREDIENT NAME";
        }

        public static class Menu
        {
            public static string DefaultTopMenuText = "BACK";
            public static string DefaultBottomMenuText = "NEXT";
        }
    }
}
