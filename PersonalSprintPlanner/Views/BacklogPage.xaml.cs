using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;
using PersonalSprintPlanner.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PersonalSprintPlanner.Views
{
    public sealed partial class BacklogPage : Page
    {
        public BacklogViewModel ViewModel { get; } = new BacklogViewModel();

        public BacklogPage()
        {
            InitializeComponent();

            Priority.ItemsSource = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();
            List<string> priorities = new List<string>() { " " };
            priorities.AddRange(Enum.GetValues(typeof(Priority)).Cast<Priority>().Select(p => Enum.GetName(p.GetType(), p)));
            PriorityFilter.ItemsSource = priorities;

            Loaded += BacklogPage_Loaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Selected = null;
        }

        private async void BacklogPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        private void TaskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.Selected = e.ClickedItem as Task;

            Details.IsPaneOpen = true;
        }

        private void CloseDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Details.IsPaneOpen = false;
            ViewModel.Selected = null;
        }

        private async void DeleteTask_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete task permanently?",
                Content = "If you delete this task, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            // Delete the file if the user clicked the primary button.
            /// Otherwise, do nothing.
            if (result == ContentDialogResult.Primary)
            {
                Details.IsPaneOpen = false;
                ViewModel.DeleteSelected();
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
        }

        private void Backlog_ContextItemClick(object sender, TaskContextMenuClickedEventArgs e)
        {
            ViewModel.AddToSprint(e.Task);
        }

        private void Sprint_ContextItemClick(object sender, TaskContextMenuClickedEventArgs e)
        {
            ViewModel.RemoveFromSprint(e.Task);
        }

        private async void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.CreateTask();
            Details.IsPaneOpen = true;
            Title.Focus(FocusState.Programmatic);
        }

        private void CreateSprint_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateSprint();
        }
    }
}
