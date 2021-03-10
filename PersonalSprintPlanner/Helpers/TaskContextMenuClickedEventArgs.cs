using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSprintPlanner.Helpers
{
    public class TaskContextMenuClickedEventArgs : EventArgs
    {
        private readonly Task _task;

        public TaskContextMenuClickedEventArgs(Task task)
        {
            _task = task;
        }

        public Task Task
        {
            get { return _task; }
        }
    }
}
