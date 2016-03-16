using Udaan16.Common;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;

namespace Udaan16.Pages
{
    public sealed partial class EventDetails : Page
    {
        private NavigationHelper navigationHelper;
        private List<Event> _event;
       
        public EventDetails()
        {
            this.InitializeComponent();
            _event = new List<Event>();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            
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
            _event.Add(e.Parameter as Event);
            if(_event != null)
            {
                title.Text = _event.First().name;
                listView.ItemsSource = _event;
                listView.DataContext = this;
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void mgrs_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            string name;
            string Contact;
            StackPanel s;
            try
            {
                s = (e.OriginalSource as TextBlock).Parent as StackPanel;
                name = (s.Children[0] as TextBlock).Text;
                Contact = (s.Children[1] as TextBlock).Text;
            }
            catch(Exception)
            {
                Manager m = (sender as ListBox).SelectedItem as Manager;
                name = m.name;
                Contact = m.Contact;
            }
           
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(Contact, name);
        }
    }
}
