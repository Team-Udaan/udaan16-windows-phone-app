using Udaan16.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Udaan16.Pages;
using Windows.Storage;
using Windows.Data.Json;

namespace Udaan16
{
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        public Dictionary<string,Department> Depts { get; set; }

        public App()
        {
            this.InitializeComponent();
            LoadData();
            this.Suspending += this.OnSuspending;
        }

        private async void LoadData()
        {
            Depts = new Dictionary<string, Department>();
            Uri dataUri = new Uri("ms-appx:///DataModel/Data.json");
            Uri deptUri = new Uri("ms-appx:///DataModel/Departments.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            StorageFile depts = await StorageFile.GetFileFromApplicationUriAsync(deptUri);
            string depttext = await FileIO.ReadTextAsync(depts);
            JsonObject Events = JsonObject.Parse(jsonText);
            JsonObject Dept = JsonObject.Parse(depttext);
            JsonArray EventsArray = Events["TechEvents"].GetArray();
            JsonArray DeptsArray = Dept["Departments"].GetArray();
            foreach (JsonValue item in DeptsArray)
            {
                JsonObject dataObject = item.GetObject();
                Department d = new Department(dataObject["name"].GetString(), dataObject["alias"].GetString());
                Depts[d.Alias] = d;
            }
            foreach (JsonValue item in EventsArray)
            {
                JsonObject obj = item.GetObject();
                Event e = new Event();
                e.Dept = obj["department"].GetString();
                e.name = obj["name"].GetString();
                e.Fee = obj["fees"].GetString();
                e.Description = obj["description"].GetString();
                e.prize = obj["prize"].GetString();
                e.NoOfParticipants = obj["numberOfParticipants"].GetString();
                e.Managers = new List<Manager>();
                foreach (JsonValue val in obj["manager"].GetArray())
                {
                    JsonObject mgr = val.GetObject();
                    Manager m = new Manager();
                    m.name = mgr["name"].GetString();
                    m.Contact = mgr["number"].GetString();
                    m.Email = mgr["email"].GetString();
                    e.Managers.Add(m);
                }
                Depts[e.Dept].Events.Add(e);
            }
            
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

             if (rootFrame == null)
            {
                rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                rootFrame.CacheSize = 1;
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                if (!rootFrame.Navigate(typeof(Deaprtments), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

       private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
