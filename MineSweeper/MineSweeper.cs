using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            // This method converts the 2D array to a nested ReadOnlyCollection
            // for the functional part to use, and then converts the 
            // result back to 2D array so that we don't change the
            // public interface.

            var (rows, cols) = (board.GetLength(0), board.GetLength(1));

            var mines = Enumerable.Range(0, rows)
                .Select(r => Enumerable.Range(0, cols).Select(c => board[r, c]).ToList().AsReadOnly())
                .ToList().AsReadOnly();

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

        private ReadOnlyCollection<ReadOnlyCollection<int?>> CalculateHintsFunctional(ReadOnlyCollection<ReadOnlyCollection<char>> mines) =>
            CalculateHintsFunctional(new List<ReadOnlyCollection<int?>>().AsReadOnly(), mines, 0);

        private ReadOnlyCollection<ReadOnlyCollection<int?>> CalculateHintsFunctional(ReadOnlyCollection<ReadOnlyCollection<int?>> hints, ReadOnlyCollection<ReadOnlyCollection<char>> mines, int position) => 
            position == mines.Count
                ? hints
                : CalculateHintsFunctional(CalculateNextRow(hints, mines), mines, position + 1);

        private ReadOnlyCollection<ReadOnlyCollection<int?>> CalculateNextRow(ReadOnlyCollection<ReadOnlyCollection<int?>> hints, ReadOnlyCollection<ReadOnlyCollection<char>> mines) =>
            hints.Count == 0
                ? hints.Append(CalculateRow(GetInitialHints(mines.First()), mines.First())).ToList().AsReadOnly()
                : hints.Take(hints.Count - 1)
                    .Append(CalculateRow(hints.Last(), mines[hints.Count]))
                    .Append(CalculateRow(
                        CalculateRow(GetInitialHints(mines[hints.Count]), mines[hints.Count]), 
                        mines[hints.Count - 1]))
                    .ToList().AsReadOnly();

        private ReadOnlyCollection<int?> CalculateRow(ReadOnlyCollection<int?> hintRow, ReadOnlyCollection<char> mineRow) => 
            CalculateRow(hintRow, mineRow, 0);
        
        private ReadOnlyCollection<int?> CalculateRow(ReadOnlyCollection<int?> hintRow, ReadOnlyCollection<char> mineRow, int position) =>
            position == hintRow.Count
                ? hintRow 
                : mineRow[position] == mineCharacter
                    ? CalculateRow(CalculateRowForMineAt(hintRow, position), mineRow, position + 1)
                    : CalculateRow(hintRow, mineRow, position + 1);

        private ReadOnlyCollection<int?> CalculateRowForMineAt(ReadOnlyCollection<int?> row, int position) =>
            position == row.Count
                ? row 
                : row.Select((count, index) => 
                    count == null || index < position - 1 || index > position + 1
                        ? count 
                        : count + 1).ToList().AsReadOnly();

        private ReadOnlyCollection<int?> GetInitialHints(ReadOnlyCollection<char> row) => 
            row.Select<char, int?>(c => c == mineCharacter ? null : 0)
            .ToList().AsReadOnly();
    }
}
