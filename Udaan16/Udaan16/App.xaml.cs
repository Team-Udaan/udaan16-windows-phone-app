﻿using Udaan16.Common;
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
            Uri dataUri = new Uri("ms-appx:///DataModel/data.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject Data = JsonObject.Parse(jsonText);
            JsonArray Categories = Data["departments"].GetArray();
            foreach (JsonValue val in Categories)
            {
                JsonObject obj = val.GetObject();
                Department d = new Department(obj["name"].GetString(), obj["alias"].GetString());
                Depts[d.Alias] = d;
                foreach (JsonValue item in obj["events"].GetArray())
                {
                    JsonObject eventobj = item.GetObject();
                    Event e = new Event();
                    e.name = eventobj["name"].GetString();
                    e.Fee = eventobj["fees"].GetString();
                    e.Description = eventobj["description"].GetString();
                    e.Description += "\r\n Round 1 : \r\n" + eventobj["round1Description"].GetString();
                    e.Description += "\r\n Round 2 : \r\n" + eventobj["round2Description"].GetString();
                    if (eventobj["round3Description"].GetString() != "")
                        e.Description += "\r\n Round 3 : \r\n" + eventobj["round3Description"].GetString();
                    e.NoOfParticipants = eventobj["participants"].GetString();
                    e.Managers = new List<Manager>();
                    foreach (JsonValue manager in eventobj["managers"].GetArray())
                    {
                        JsonObject mgr = manager.GetObject();
                        Manager m = new Manager();
                        m.name = mgr["name"].GetString();
                        m.Contact = mgr["mobile"].GetString();
                        m.Email = mgr["email"].GetString();
                        e.Managers.Add(m);
                    }
                    d.Events.Add(e);
                }
            }
            foreach (JsonValue item in Data["categories"].GetArray())
            {
                JsonObject o = item.GetObject();
                if (o["name"].GetString() != "Departments")
                {
                    Department dep = new Department(o["name"].GetString(), o["alias"].GetString());
                    Depts[dep.Title] = dep;
                    foreach (JsonValue v in Data[o["alias"].GetString()].GetArray())
                    {
                        JsonObject jobj = v.GetObject();
                        Event e = new Event();
                        e.name = jobj["name"].GetString();
                        e.Description = jobj["description"].GetString();
                        try
                        {
                            e.Description += "\r\n Round 1 : \r\n" + jobj["round1Description"].GetString();
                            e.Description += "\r\n Round 2 : \r\n" + jobj["round2Description"].GetString();
                            if (jobj["round3Description"].GetString() != "")
                                e.Description += "\r\n Round 3 : \r\n" + jobj["round3Description"].GetString();
                            e.NoOfParticipants = jobj["participants"].GetString();
                            e.Fee = jobj["fees"].GetString();
                        }
                        catch (Exception)
                        {
                            e.NoOfParticipants = e.Fee = "N/A";
                        }
                        e.Managers = new List<Manager>();
                        foreach (JsonValue manager in jobj["managers"].GetArray())
                        {
                            JsonObject mgr = manager.GetObject();
                            Manager m = new Manager();
                            m.name = mgr["name"].GetString();
                            m.Contact = mgr["mobile"].GetString();
                            m.Email = mgr["email"].GetString();
                            e.Managers.Add(m);
                        }
                        dep.Events.Add(e);
                    }
                }
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
