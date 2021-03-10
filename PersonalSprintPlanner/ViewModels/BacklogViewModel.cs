using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PersonalSprintPlanner.Helpers;


using Microsoft.Toolkit.Uwp.UI.Controls;
using DataAccessLibrary.Models;
using DataAccessLibrary;
using System.Linq;
using DataAccessLibrary.Helpers;
using System.Collections.Specialized;

namespace PersonalSprintPlanner.ViewModels
{
    public class BacklogViewModel : Observable
    {
        private Task _selected;
        private Board _boardFilter;
        private string _priorityFilter;
        private string _dueDateFilter;
        private string _searchTerm;
        private Task m_ReorderItem;
        private int m_ReorderIndexFrom;

        public List<string> DueDates = new List<string>()
        {
            " ",
            "Overdue",
            "Today",
            "3 Days"
        };

        public Board BoardFilter
        {
            get { return _boardFilter; }
            set
            {
                Set(ref _boardFilter, value);
                ApplyFilters();
            }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                Set(ref _searchTerm, value);
                ApplyFilters();
            }
        }

        public string PriorityFilter
        {
            get { return _priorityFilter; }
            set
            {
                Set(ref _priorityFilter, value);
                ApplyFilters();
            }
        }

        public string DueDateFilter
        {
            get { return _dueDateFilter; }
            set
            {
                Set(ref _dueDateFilter, value);
                ApplyFilters();
            }
        }

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

        public ObservableCollection<Task> SprintTasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> BacklogTasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> BacklogTasksFiltered { get; set; } = new ObservableCollection<Task>();

        public BacklogViewModel()
        {
        }

        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            BacklogTasks.Clear();
            SprintTasks.Clear();

            var tasks = await DataAccess.GetTasks(true);

            foreach (var task in tasks)
            {
                if (task.SprintRelevant)
                {
                    SprintTasks.Add(task);
                }
                else
                {
                    BacklogTasks.Add(task);
                }
            }

            ApplyFilters();

            BacklogTasksFiltered.CollectionChanged += CollectionChanged;
        }

        private void SaveSelected()
        {
            DataAccess.UpdateTask(Selected);
            if (Selected.SprintRelevant)
            {
                int index = SprintTasks.IndexOf(Selected);
                if (index >= 0)
                {
                    SprintTasks[index] = Selected;
                }
            }
            else
            {
                ApplyFilters();
            }
        }

        public void DeleteSelected()
        {
            DataAccess.DeleteTask(Selected);
            if (Selected.SprintRelevant)
            {
                SprintTasks.Remove(Selected);
            }
            else
            {
                BacklogTasks.Remove(Selected);
                ApplyFilters();
            }
            Selected = null;
        }

        private void ApplyFilters()
        {
            BacklogTasksFiltered.Clear();

            Priority? priority;

            if(PriorityFilter == null || PriorityFilter.Equals(" "))
            {
                priority = null;
            } else
            {
                priority = (Priority)System.Enum.Parse(typeof(Priority), PriorityFilter);
            }

            var temp = BacklogTasks
                .Where(
                t => (t?.Title ?? "").Contains(SearchTerm ?? "") &&
                (BoardFilter == null || BoardFilter.ID == -1 || t.BoardID == BoardFilter.ID) &&
                (priority == null || t.Priority == priority) &&
                (DueDateFilter == null || DueDateFilter.Equals(" ") ||
                    (DueDateFilter.Equals("Overdue") && t.DueDate != null && t.DueDate < DateTime.Now) ||
                    (DueDateFilter.Equals("Todays") && t.DueDate != null && t.DueDate == DateTime.Today) ||
                    (DueDateFilter.Equals("3 Days") && t.DueDate != null && DateTime.Now.AddDays(3) - t.DueDate <= TimeSpan.FromDays(3))
                )).OrderBy(t => t.Position);

            foreach(Task task in temp)
            {
                BacklogTasksFiltered.Add(task);
            }
        }

        public void AddToSprint(Task task)
        {
            BacklogTasks.Remove(task);
            task.SprintRelevant = true;
            if (task.SprintPosition == null)
            {
                // Add it at the bottom of the ToDo tasks
                task.SprintPosition = Positioning.Calculate(SprintTasks.Where(t => t.Status == Status.ToDo).LastOrDefault()?.Position, null);
                DataAccess.UpdateTask(task);
            }
            else
            {
                DataAccess.UpdateTaskSprintRelevance(task);
            }

            for (int i = 0; i < SprintTasks.Count(); i++)
            {
                if (String.Compare(SprintTasks[i].SprintPosition, task.SprintPosition) >= 0)
                {
                    SprintTasks.Insert(i, task);
                    ApplyFilters();
                    return;
                }
            }

            SprintTasks.Add(task);
            ApplyFilters();
            Selected = task;
        }

        public void RemoveFromSprint(Task task)
        {
            SprintTasks.Remove(task);
            task.SprintRelevant = false;
            DataAccess.UpdateTaskSprintRelevance(task);

            for (int i = 0; i < BacklogTasks.Count(); i++)
            {
                if (String.Compare(BacklogTasks[i].Position, task.Position) >= 0)
                {
                    BacklogTasks.Insert(i, task);
                    return;
                }
            }

            BacklogTasks.Add(task);
            ApplyFilters();
            Selected = task;
        }

        public async System.Threading.Tasks.Task CreateTask()
        {
            Task task = new Task()
            {
                Status = Status.ToDo,
                SprintRelevant = false,
                BoardID = 1,
                Priority = Priority.Low,
                Position = Positioning.Calculate(Positioning.Max(BacklogTasks.LastOrDefault()?.Position, SprintTasks.LastOrDefault()?.Position), null),
                CreationDate = DateTime.Now
            };

            long id = await DataAccess.AddTask(task);

            task.ID = id;
            BacklogTasks.Add(task);
            ApplyFilters();
            Selected = task;
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
                    HandleReorder(m_ReorderItem, m_ReorderIndexFrom, _ReorderIndexTo);
                    m_ReorderItem = null;
                    break;
            }
        }

        private void HandleReorder(Task task, int indexFrom, int indexTo)
        {
            string nextPos = BacklogTasksFiltered.ElementAtOrDefault(indexTo + 1)?.Position ?? Positioning.EndStop;
            string prevPos = BacklogTasks.Where(t => String.Compare(nextPos, t.Position) >= 1).OrderBy(t => t.Position).LastOrDefault()?.Position ?? "a";

            // if there are sprint tasks that would be below the previous (= above) element make sure that the new element is placed below those as well so that they end in a similar position once they are added to the backlog again
            prevPos = SprintTasks.Where(t => String.Compare(t.Position, prevPos) >= 0 && String.Compare(t.Position, nextPos) < 0).OrderBy(t => t.Position).LastOrDefault()?.Position ?? prevPos;

            task.Position = Positioning.Calculate(prevPos, nextPos);
            DataAccess.UpdateTaskPosition(task);
        }

        public async void CreateSprint()
        {
            await DataAccess.ClearSprint();

            foreach(Task task in SprintTasks)
            {
                task.SprintRelevant = false;
                BacklogTasks.Add(task);
            }

            SprintTasks.Clear();
            ApplyFilters();
        }
    }
}
