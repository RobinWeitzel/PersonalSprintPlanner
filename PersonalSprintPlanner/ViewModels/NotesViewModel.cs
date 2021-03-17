using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;

using PersonalSprintPlanner.Core.Models;
using PersonalSprintPlanner.Core.Services;
using PersonalSprintPlanner.Helpers;

namespace PersonalSprintPlanner.ViewModels
{
    public class NotesViewModel : Observable
    {
        private Note _selected;

        public Note Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != null)
                {
                    SaveSelected();
                }

                if(_selected != value)
                {
                    Set(ref _selected, value);
                }
            }
        }

        public FullyObservableCollection<Note> Notes { get; set; } = new FullyObservableCollection<Note>();

        public NotesViewModel()
        {
        }

        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            Notes.Clear();

            var data = await DataAccess.GetNotes();

            foreach (var item in data)
            {
                Notes.Add(item);
            }

            Notes.ItemPropertyChanged += ItemPropertyChanged;
        }

        public void ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs args)
        {
            Notes[args.CollectionIndex] = Notes[args.CollectionIndex];
        }

        public async void CreateNote()
        {
            Note note = new Note();
            note.Title = "New note";
            note.CreationDate = DateTime.Now;
            note.ID = await DataAccess.AddNote(note);

            Notes.Add(note);
            Selected = note;
        }

        public async void DeleteNote()
        {
            await DataAccess.DeleteNote(Selected);

            Notes.Remove(Selected);
            Selected = null;
        }

        private void SaveSelected()
        {
            if (Selected != null)
            {
                DataAccess.UpdateNote(Selected);
            }
        }
    }
}
