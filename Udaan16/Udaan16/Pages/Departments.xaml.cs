using Udaan16.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

namespace Udaan16.Pages
{
    public sealed partial class Deaprtments : Page
    {
        private NavigationHelper navigationHelper;
        private List<Department> Items;
        
        public Deaprtments()
        {
            this.InitializeComponent();
            Items = (Application.Current as App).Depts.Values.ToList<Department>();
            listView.ItemsSource = Items;
            listView.DataContext = this;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.Content.GetType().Equals(typeof(Deaprtments)))
            {
                Application.Current.Exit();
            }
            else
            {
                if (navigationHelper.GoBackCommand.CanExecute(null))
                {
                    e.Handled = true;
                    navigationHelper.GoBackCommand.Execute(null);
                }
            }
        }

        #endregion

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Department d = e.ClickedItem as Department;
            Frame.Navigate(typeof(EventList),d);
        }
    }
}
