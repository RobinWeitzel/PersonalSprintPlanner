using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class SprintTaskListView : UserControl
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

        public event EventHandler<TaskCustomDropEventArgs> ItemDrop;
        public static readonly DependencyProperty ItemDropProperty =
            DependencyProperty.Register("ItemDrop", typeof(EventHandler<TaskCustomDropEventArgs>), typeof(ListView), null);

        public SprintTaskListView()
        {
            this.InitializeComponent();
        }

        private void List_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private int GetItemIndex(double positionY, ListView targetListView)
        {
            var index = 0;
            double height = 0;

            foreach (var item in targetListView.Items)
            {
                height += GetRowHeight(item, targetListView);
                if (height > positionY) return index;
                index++;
            }

            return index;
        }

        private double GetRowHeight(object listItem, ListView targetListView)
        {
            var listItemContainer = targetListView.ContainerFromItem(listItem) as ListViewItem;
            var height = listItemContainer.ActualHeight;
            var marginTop = listItemContainer.Margin.Top;
            return marginTop + height;
        }

        private void List_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView != null &&
            e.DataView.Properties != null &&
            e.DataView.Properties.Any(x => x.Key == "item" && x.Value.GetType() == typeof(Task)))
            {
                var handler = ItemDrop;

                if (handler == null)
                    return;

                var scrollViewer = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild((ListView)sender, 0), 0) as ScrollViewer;
                var position = e.GetPosition((ListView)sender);
                var positionY = scrollViewer.VerticalOffset + position.Y;
                var index = GetItemIndex(positionY, (ListView)sender);

                var item = e.Data.Properties.FirstOrDefault(x => x.Key == "item");
                Task task = item.Value as Task;

                
                handler(this, new TaskCustomDropEventArgs(task, index));
            }
        }

        private void List_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.RequestedOperation = DataPackageOperation.Move;

            if (e.Items != null && e.Items.Any())
            {
                var items = e.Items.Cast<Task>();
                e.Data.Properties.Add("item", items.First());
            }
        }
    }
}
