using System;

using PersonalSprintPlanner.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PersonalSprintPlanner.Views
{
    public sealed partial class NotesPage : Page
    {
        public NotesViewModel ViewModel { get; } = new NotesViewModel();

        public NotesPage()
        {
            InitializeComponent();
            Loaded += NotesPage_Loaded;
        }

        private async void NotesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
