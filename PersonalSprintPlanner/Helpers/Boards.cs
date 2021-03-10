using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PersonalSprintPlanner.Helpers
{
    public static class Boards
    {
        private static List<Board> _boards { get; set; } = new List<Board>();

        public static void SetBoards(IEnumerable<Board> boards)
        {
            _boards.Clear();

            foreach (var board in boards)
            {
                _boards.Add(board);
            }
        }

        public static Board GetBoard(long id)
        {
            return _boards.Find(b => b.ID == id);
        }

        public static string GetBoardName(long id)
        {
            return Boards.GetBoard(id)?.Name;
        }

        public static SolidColorBrush GetBoardColor(long id)
        {
            Board board = Boards.GetBoard(id);
            CustomColorConverter conv = new CustomColorConverter();

            return (SolidColorBrush) conv.Convert(board.Color, null, null, null);
        }

        public static List<Board> GetBoards()
        {
            return _boards;
        }

        public static List<Board> GetBoardsAndEmpty()
        {
            List<Board> boards = new List<Board>();
            boards.Add(new Board() {
                Name = "",
                ID = -1
            });
            boards.AddRange(_boards);
            return boards;
        }
    }
}
