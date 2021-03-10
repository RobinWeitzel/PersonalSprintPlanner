using System;
using System.Linq;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PersonalSprintPlanner.Views
{
    public sealed partial class SprintPage : Page
    {
        public SprintViewModel ViewModel { get; } = new SprintViewModel();

        public SprintPage()
        {
            InitializeComponent();
            Priority.ItemsSource = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();
            Loaded += SprintPage_Loaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Selected = null;
        }


        private async void SprintPage_Loaded(object sender, RoutedEventArgs e)
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

        private void ToDoList_ItemDrop(object sender, Helpers.TaskCustomDropEventArgs e)
        {
            Status from = e.Task.Status;
            Status to = Status.ToDo;

            if(from != to)
            {
                ViewModel.MoveTask(from, to, e.Task, e.Index);
            }
        }

        private void InProgressList_ItemDrop(object sender, Helpers.TaskCustomDropEventArgs e)
        {
            Status from = e.Task.Status;
            Status to = Status.InProgress;

            if (from != to)
            {
                ViewModel.MoveTask(from, to, e.Task, e.Index);
            }
        }

        private void InReviewList_ItemDrop(object sender, Helpers.TaskCustomDropEventArgs e)
        {
            Status from = e.Task.Status;
            Status to = Status.Review;

            if (from != to)
            {
                ViewModel.MoveTask(from, to, e.Task, e.Index);
            }
        }

        private void CompletedList_ItemDrop(object sender, Helpers.TaskCustomDropEventArgs e)
        {
            Status from = e.Task.Status;
            Status to = Status.Completed;

            if (from != to)
            {
                ViewModel.MoveTask(from, to, e.Task, e.Index);
            }
        }
    }
}
