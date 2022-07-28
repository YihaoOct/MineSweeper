namespace MineSweeper
{
    public class MineSweeper
    {
        private const char mineCharacter = '*';

        public int?[,] CalculateHints(char[,] board)
        {
            var (rows, cols) = (board.GetLength(0), board.GetLength(1));
            var result = new int?[rows, cols];
            
            for (int r = 0; r < rows; r++) 
            {
                for (int c = 0; c < cols; c++) 
                {
                    result[r,c] = GetNumberOfNeighbours(board, r, c);
                }
            }

            return result;
        }

        private int? GetNumberOfNeighbours(char[,] board, int row, int col)
        {
            if (board[row, col] == mineCharacter)
            {
                return null;
            }

            var neighbours = 0;
            for (int rowOffset = -1; rowOffset <= 1; rowOffset ++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset ++)
                {
                    if (rowOffset == 0 && colOffset == 0)
                    {
                        continue;
                    }

                    if (IsMine(board, row + rowOffset, col + colOffset))
                    {
                        neighbours++;
                    }
                }
            }

            return neighbours;
        }

        private bool IsMine(char[,] board, int row, int col)
        {
            if (row < 0 || row >= board.GetLength(0) || col < 0 || col >= board.GetLength(1))
            {
                return false;
            }
            
            return board[row,col] == mineCharacter;
        }
    }
}
