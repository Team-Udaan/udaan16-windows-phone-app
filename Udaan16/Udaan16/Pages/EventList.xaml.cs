using Udaan16.Common;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Udaan16.Pages
{
    public sealed partial class EventList : Page
    {
        private NavigationHelper navigationHelper;
        private List<Event> Items;
        public EventList()
        {
            this.InitializeComponent();
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Event eve = e.ClickedItem as Event;
            if (eve.name != "Coming Soon")
                Frame.Navigate(typeof(EventDetails), eve);
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            Department d = e.Parameter as Department;
            if (d != null)
            {
                TitleOfPage.Text = d.Title;
                if (d.Title == "Nights")
                    Items = new List<Event>() { new Event("Coming Soon") };
                else
                    Items = d.Events;
                listView.ItemsSource = Items;
                listView.DataContext = this;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
