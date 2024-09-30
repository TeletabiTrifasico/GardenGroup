using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace GardenGroup.ViewModels
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        protected MongoCRUD _crud;
        private object _currentView;

        public MainViewModel(MongoCRUD crud)
        {
            _crud = crud;
            // Initialize with the Login view
            CurrentView = new LoginViewModel(this);
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public void SwitchToDashboard()
        {
            // Switch to the Dashboard view
            CurrentView = new DashboardViewModel(_crud);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
