using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DataAccessLibrary;
using DataAccessLibrary.Helpers;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;

namespace PersonalSprintPlanner.ViewModels
{
    public class SprintViewModel : Observable
    {
        private Task _selected;
        private Task m_ReorderItem;
        private int m_ReorderIndexFrom;

        public Task Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != null)
                {
                    SaveSelected();
                }

                Set(ref _selected, value);
            }
        }

        private List<Task> Tasks = new List<Task>();
        public ObservableCollection<Task> ToDoTasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> InProgressTasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> InReviewTasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> CompletedTasks { get; set; } = new ObservableCollection<Task>();
        public SprintViewModel()
        {
        }

        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            ToDoTasks.Clear();
            InProgressTasks.Clear();
            InReviewTasks.Clear();
            CompletedTasks.Clear();

            var tasks = await DataAccess.GetTasks(true);

            foreach (var task in tasks)
            {
                Tasks.Add(task);
                if(!task.SprintRelevant)
                {
                    continue;
                }

                switch(task.Status)
                {
                    case Status.ToDo:
                        ToDoTasks.Add(task);
                        break;
                    case Status.InProgress:
                        InProgressTasks.Add(task);
                        break;
                    case Status.Review:
                        InReviewTasks.Add(task);
                        break;
                    case Status.Completed:
                        CompletedTasks.Add(task);
                        break;
                }
            }

            ToDoTasks.CollectionChanged += CollectionChanged;
            InProgressTasks.CollectionChanged += CollectionChanged;
            InReviewTasks.CollectionChanged += CollectionChanged;
            CompletedTasks.CollectionChanged += CollectionChanged;
        }

        private void SaveSelected()
        {
            DataAccess.UpdateTask(Selected);
            int index;
            switch (Selected.Status)
            {
                case Status.ToDo:
                    index = ToDoTasks.IndexOf(Selected);
                    if (index >= 0)
                    {
                        ToDoTasks[index] = Selected;
                    }
                    break;
                case Status.InProgress:
                    index = InProgressTasks.IndexOf(Selected);
                    if (index >= 0)
                    {
                        InProgressTasks[index] = Selected;
                    }
                    break;
                case Status.Review:
                    index = InReviewTasks.IndexOf(Selected);
                    if (index >= 0)
                    {
                        InReviewTasks[index] = Selected;
                    }
                    break;
                case Status.Completed:
                    index = CompletedTasks.IndexOf(Selected);
                    if (index >= 0)
                    {
                        CompletedTasks[index] = Selected;
                    }
                    break;
            }  
        }

        public void DeleteSelected()
        {
            DataAccess.DeleteTask(Selected);
            switch (Selected.Status)
            {
                case Status.ToDo:
                    ToDoTasks.Remove(Selected);
                    break;
                case Status.InProgress:
                    InProgressTasks.Remove(Selected);
                    break;
                case Status.Review:
                    InReviewTasks.Remove(Selected);
                    break;
                case Status.Completed:
                    CompletedTasks.Remove(Selected);
                    break;
            }
            Selected = null;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    m_ReorderItem = (Task)e.OldItems[0];
                    m_ReorderIndexFrom = e.OldStartingIndex;
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (m_ReorderItem == null)
                        return;
                    var _ReorderIndexTo = e.NewStartingIndex;
                    HandleReorder((ObservableCollection<Task>) sender, m_ReorderItem, _ReorderIndexTo);
                    m_ReorderItem = null;
                    break;
            }
        }

        private void HandleReorder(ObservableCollection<Task> sender, Task task, int index, bool save = true)
        {
            List<Task> collection = Tasks.Where(t => t.Status == task.Status).ToList();
            string nextPos = sender.ElementAtOrDefault(index + 1)?.Position ?? Positioning.EndStop;
            string prevPos = collection.Where(t => String.Compare(nextPos, t.Position) >= 0).OrderBy(t => t.Position).LastOrDefault()?.Position ?? "a";
            task.Position = Positioning.Calculate(prevPos, nextPos);
            
            if(save)
            {
                DataAccess.UpdateTaskPosition(task);
            }
        }

        public void MoveTask(Status from, Status to, Task task, int index)
        {
            task.Status = to;

            switch (from)
            {
                case Status.ToDo:
                    ToDoTasks.Remove(task);
                    break;
                case Status.InProgress:
                    InProgressTasks.Remove(task);
                    break;
                case Status.Review:
                    InReviewTasks.Remove(task);
                    break;
                case Status.Completed:
                    CompletedTasks.Remove(task);
                    break;
            }

            switch (to)
            {
                case Status.ToDo:
                    ToDoTasks.Insert(index, task);
                    HandleReorder(ToDoTasks, task, index, false);
                    break;
                case Status.InProgress:
                    InProgressTasks.Insert(index, task);
                    HandleReorder(InProgressTasks, task, index, false);
                    break;
                case Status.Review:
                    InReviewTasks.Insert(index, task);
                    HandleReorder(InReviewTasks, task, index, false);
                    break;
                case Status.Completed:
                    CompletedTasks.Insert(index, task);
                    HandleReorder(CompletedTasks, task, index, false);
                    break;
            }

            DataAccess.UpdateTask(task);
        }
    }
}
