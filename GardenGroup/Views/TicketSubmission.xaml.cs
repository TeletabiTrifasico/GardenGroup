using System.Windows;
using GardenGroup.ViewModels;
using Service;

namespace GardenGroup.Views
{
    /// <summary>
    /// Interaction logic for TicketSubmission.xaml
    /// </summary>
    public partial class TicketSubmission : Window
    {
        public TicketSubmission(IServiceManager serviceManager, MainViewModel mainViewModel)
        {
            InitializeComponent();
        
            var viewModel = new TicketSubmissionViewModel(serviceManager, mainViewModel);
            viewModel.SubmitCompleted += OnSubmitCompleted;
        
            DataContext = viewModel;
        }

        private void OnSubmitCompleted()
        {
            // Close the window when the submission is complete
            this.Close();
        }
    }

}