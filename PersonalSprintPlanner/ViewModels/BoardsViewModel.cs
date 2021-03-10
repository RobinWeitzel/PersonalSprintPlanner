using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using PersonalSprintPlanner.Helpers;

namespace PersonalSprintPlanner.ViewModels
{
    public class BoardsViewModel : Observable
    {
        public ObservableCollection<Board> Boards = new ObservableCollection<Board>();

        private Board _selected;
        public Board Selected
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

        public BoardsViewModel()
        {
        }

        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            Boards.Clear();

            var boards = Helpers.Boards.GetBoards();

            foreach (Board board in boards)
            {
                Boards.Add(board);
            }
        }

        private void SaveSelected()
        {
            DataAccess.UpdateBoard(Selected);

            int index = Boards.IndexOf(Selected);
            if (index >= 0)
            {
                Boards[index] = Selected;
            }
        }

        public async Task<bool> DeleteSelected()
        {
            bool success = await DataAccess.DeleteBoard(Selected);
            if(success)
            {
                Boards.Remove(Selected);
                Helpers.Boards.SetBoards(Boards);

                Selected = null;
            }
            
            return success;
        }

        public async void AddBoard()
        {
            Board board = new Board
            {
                Name = "",
                Color = Color.Transparent
            };

            board.ID = await DataAccess.AddBoard(board);

            Boards.Add(board);
            Helpers.Boards.SetBoards(Boards);

        }
    }
}
