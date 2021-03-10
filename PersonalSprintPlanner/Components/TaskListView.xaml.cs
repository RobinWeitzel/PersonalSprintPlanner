using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace PersonalSprintPlanner.Components
{
    public sealed partial class TaskListView : UserControl
    {
        public ObservableCollection<Task> Tasks
        {
            get { return (ObservableCollection<Task>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register("Tasks", typeof(ObservableCollection<Task>), typeof(ListView), new PropertyMetadata(new ObservableCollection<Task>()));

        public ObservableCollection<Board> Boards
        {
            get { return (ObservableCollection<Board>)GetValue(BoardsProperty); }
            set { SetValue(BoardsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoardsProperty =
            DependencyProperty.Register("Boards", typeof(ObservableCollection<Board>), typeof(ListView), new PropertyMetadata(new ObservableCollection<Task>()));

        public event ItemClickEventHandler ItemClick
        {
            add { List.ItemClick += value; }
            remove { List.ItemClick -= value; }
        }

        public static readonly DependencyProperty ItemClickProperty =
            DependencyProperty.Register("ItemClick", typeof(ItemClickEventHandler), typeof(ListView), null);

        public event EventHandler<TaskContextMenuClickedEventArgs> ContextItemClick;
        public static readonly DependencyProperty ContextItemClickProperty =
            DependencyProperty.Register("ContextItemClick", typeof(EventHandler<TaskContextMenuClickedEventArgs>), typeof(MenuFlyoutItem), null);

        public bool CanReorderItems
        {
            get { return (bool)GetValue(CanReorderItemsProperty); }
            set { SetValue(CanReorderItemsProperty, value); }
        }

        public static readonly DependencyProperty CanReorderItemsProperty =
            DependencyProperty.Register("CanReorderItems", typeof(bool), typeof(ListView), null);

        public string ContextItemText
        {
            get { return (string)GetValue(ContextItemTextProperty);  }
            set { SetValue(ContextItemTextProperty, value);  }
        }

        public static readonly DependencyProperty ContextItemTextProperty =
            DependencyProperty.Register("ContextItemText", typeof(string), typeof(MenuFlyoutItem), null);

        public string ContextItemSymbol
        {
            get { return (string)GetValue(ContextItemSymbolProperty); }
            set { SetValue(ContextItemSymbolProperty, value); }
        }

        public static readonly DependencyProperty ContextItemSymbolProperty =
            DependencyProperty.Register("ContextItemSymbol", typeof(string), typeof(SymbolIcon), null);

        private Task ContextTask;

        public TaskListView()
        {
            this.InitializeComponent();
        }

        private void List_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ContextTask = ((FrameworkElement)e.OriginalSource).DataContext as Task;
            ListView listView = (ListView)sender;
            ListContextMenu.ShowAt(listView, e.GetPosition(listView));
        }

        private void ContextItem_Click(object sender, RoutedEventArgs e)
        {
            var handler = ContextItemClick;

            if (handler == null)
                return;

            handler(this, new TaskContextMenuClickedEventArgs(ContextTask));
        }
    }
}
