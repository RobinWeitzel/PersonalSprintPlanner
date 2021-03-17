using System;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;
using PersonalSprintPlanner.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Selected = null;
        }

        private async void NotesPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        private void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.Selected = e.ClickedItem as Note;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateNote();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteNote();
        }
    }
}
