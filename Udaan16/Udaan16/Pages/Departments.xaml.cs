using Udaan16.Common;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Udaan16.Pages
{
    public sealed partial class Deaprtments : Page
    {
        private NavigationHelper navigationHelper;
        private List<Department> Items;
        
        public Deaprtments()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            Items = new List<Department>();
            Items.Add(new Department("Tech", "tech"));
            foreach (Department d in (Application.Current as App).Depts.Values.ToList<Department>())
            {
                Items.Add(new Department(d.Title, d.Alias));
            }
            listView.ItemsSource = Items;
            listView.DataContext = this;
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

        #endregion

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Department d = e.ClickedItem as Department;
            if (d.Title == "Tech")
                Frame.Navigate(typeof(DList));
            else
                Frame.Navigate(typeof(EventList), (Application.Current as App).Depts[d.Title]);
        }

        //private void pinAppBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    Frame.Navigate(typeof(About));
        //}
    }
}
