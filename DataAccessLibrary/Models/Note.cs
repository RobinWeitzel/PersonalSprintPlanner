using DataAccessLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class Note : Observable2
    {
        public long ID { get; set; }
        public string _title;
        public string Title {
            get { return _title; }
            set
            {
                Set(ref _title, value);
            }
        }

        public string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                Set(ref _content, value);
            }
        }
        public DateTimeOffset CreationDate { get; set; }
    }
}
