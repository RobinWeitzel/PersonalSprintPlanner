using System;
using System.Linq;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PersonalSprintPlanner.Views
{
    public sealed partial class BoardsPage : Page
    {
        public BoardsViewModel ViewModel { get; } = new BoardsViewModel();

        public BoardsPage()
        {
            InitializeComponent();

            Color.ItemsSource = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();

            Loaded += BoardsPage_Loaded;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Selected = null;
        }

        private async void BoardsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        private void Boards_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.Selected = e.ClickedItem as Board;

            Details.IsPaneOpen = true;
        }

        private void CloseDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Details.IsPaneOpen = false;
            ViewModel.Selected = null;
        }

        private async void DeleteBoard_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            bool success = await ViewModel.DeleteSelected();
            if(success)
            {
                Details.IsPaneOpen = false;
            } else
            {
                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = "Could not delete board",
                    Content = "Board is still in use in some tasks",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await noWifiDialog.ShowAsync();
            }
        }

        private void NewBoard_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddBoard();
        }
    }
}
