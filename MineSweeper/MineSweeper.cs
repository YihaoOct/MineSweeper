using System.Collections.Generic;
using System.Linq;

namespace MineSweeper
{
    public class MineSweeper
    {
        private const char mineCharacter = '*';

        public int?[,] CalculateHints(char[,] board)
        {
            // Since functional programming is about immutable states,
            // arrays are not a good data structure to use with FP.
            // This method converts the 2D array to a nested List
            // for the functional part to use, and then converts the 
            // result back to 2D array so that we don't change the
            // public interface.

            var (rows, cols) = (board.GetLength(0), board.GetLength(1));

            var mines = Enumerable.Range(0, rows)
                .Select(r => Enumerable.Range(0, cols).Select(c => board[r, c]).ToList())
                .ToList();

            var hints = CalculateHintsFunctional(mines);

            var result = new int?[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++) 
                {
                    result[r, c] = hints[r][c];
                }
            }

            return result;
        }

        private List<List<int?>> CalculateHintsFunctional(List<List<char>> mines) =>
            CalculateHintsFunctional(new List<List<int?>>(), mines, 0);

        private List<List<int?>> CalculateHintsFunctional(List<List<int?>> hints, List<List<char>> mines, int position) => 
            position == mines.Count
                ? hints
                : CalculateHintsFunctional(CalculateNextRow(hints, mines), mines, position + 1);

        private List<List<int?>> CalculateNextRow(List<List<int?>> hints, List<List<char>> mines) =>
            hints.Count == 0
                ? hints.Append(CalculateRow(GetInitialHints(mines.First()), mines.First())).ToList()
                : hints.Take(hints.Count - 1)
                    .Append(CalculateRow(hints.Last(), mines[hints.Count]))
                    .Append(CalculateRow(
                        CalculateRow(GetInitialHints(mines[hints.Count]), mines[hints.Count]), 
                        mines[hints.Count - 1]))
                    .ToList();

        private List<int?> CalculateRow(List<int?> hintRow, List<char> mineRow) => 
            CalculateRow(hintRow, mineRow, 0);
        
        private List<int?> CalculateRow(List<int?> hintRow, List<char> mineRow, int position) =>
            position == hintRow.Count
                ? hintRow 
                : mineRow[position] == mineCharacter
                    ? CalculateRow(CalculateRowForMineAt(hintRow, position), mineRow, position + 1)
                    : CalculateRow(hintRow, mineRow, position + 1);

        private List<int?> CalculateRowForMineAt(List<int?> row, int position) =>
            position == row.Count
                ? row 
                : row.Select((count, index) => 
                    count == null || index < position - 1 || index > position + 1
                        ? count 
                        : count + 1).ToList();

        private List<int?> GetInitialHints(List<char> row) => 
            row.Select<char, int?>(c => c == mineCharacter ? null : 0)
            .ToList();
    }
}
