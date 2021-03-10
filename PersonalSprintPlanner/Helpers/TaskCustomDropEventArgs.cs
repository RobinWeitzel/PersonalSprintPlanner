using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSprintPlanner.Helpers
{
    public class TaskCustomDropEventArgs : EventArgs
    {
        private readonly Task _task;
        private readonly int _index;

        public TaskCustomDropEventArgs(Task task, int index)
        {
            _task = task;
            _index = index;
        }

        public Task Task
        {
            get { return _task; }
        }

        public int Index
        {
            get { return _index; }
        }
    }
}
