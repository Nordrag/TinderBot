using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TinderBotGUI.Core;

namespace TinderBotGUI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {       
        public RelayCommand TinderViewCommand { get; set; }
        public RelayCommand BumbleViewCommand { get; set; }
        public RelayCommand BadooViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand AboutViewCommand { get; set; }

        public TinderViewModel Tinder { get; set; }
        public BumbleViewModel Bumble { get; set; }
        public BadooViewModel Badoo { get; set; }
        public SettingsViewModel Settings { get; set; }
        public AboutViewModel About { get; set; }

        private object currentView;

        public object CurrentView 
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnpropertyChanged();
            }
        }
      
        public void DestroyDriver()
        {
            Tinder.DestroyDriver();   
            Bumble.DestroyDriver();
            Badoo.DesroyDriver();
        }

        public MainViewModel()
        {
            Settings = new SettingsViewModel();
            Tinder = new TinderViewModel();
            Bumble = new BumbleViewModel();
            Badoo = new BadooViewModel();
            About = new AboutViewModel(Settings.SavedData.LikesSentOnTinder, Settings.SavedData.LikesSentOnBadoo, Settings.SavedData.MatchesOnTinder, Settings.SavedData.MatchesOnBadoo);
            currentView = Tinder;

            TinderViewCommand = new RelayCommand(e => { CurrentView = Tinder;}, f => true);
            BumbleViewCommand = new RelayCommand(e => { CurrentView = Bumble;}, f => true);
            BadooViewCommand = new RelayCommand(e => { CurrentView = Badoo;}, f => true);
            SettingsViewCommand = new RelayCommand(e => { CurrentView = Settings; }, f => true);
            AboutViewCommand = new RelayCommand(e => { CurrentView = About; }, f => true);     
                       
        }
     
    }
}
