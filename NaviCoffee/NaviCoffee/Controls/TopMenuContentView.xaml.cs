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
    public partial class TopMenuContentView : ContentView
    {
        public static readonly BindableProperty MenuTitleProperty = BindableProperty.Create(nameof(MenuTitle), typeof(string), typeof(TopMenuContentView), Constants.Menu.DefaultTopMenuText);
        public static readonly BindableProperty MenuImageProperty = BindableProperty.Create(nameof(MenuImage), typeof(string), typeof(TopMenuContentView), Constants.Images.DownArrow);

        public TopMenuContentView()
        {
            InitializeComponent();
        }

        #region Properties
        public string MenuTitle
        {
            get => (string)GetValue(MenuTitleProperty);
            set => SetValue(MenuTitleProperty, value);
        }

        public string MenuImage
        {
            get => (string)GetValue(MenuImageProperty);
            set => SetValue(MenuImageProperty, value);
        }
        #endregion

        #region Private Methods
        private void SwipeItemView_Invoked(object sender, EventArgs e)
        {
            MessagingCenter.Send("topMenuInvoked", Constants.MessagingCenterMessages.TopMenuSwiped);
        }
        #endregion
    }
}