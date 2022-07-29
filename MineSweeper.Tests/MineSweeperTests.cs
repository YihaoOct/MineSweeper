using Xunit;

namespace MineSweeper.Tests
{

    public class MineSweeperTests
    {
        MineSweeper minesweeper;

        public MineSweeperTests()
        {
            minesweeper = new MineSweeper();
        }

        [Fact]
        public void CalculatesHintsCorrectly()
        {
            // Arrange
            var input = new char[4,4] 
            {
                { '.', '*', '.', '.' }, 
                { '.', '.', '.', '.' }, 
                { '.', '*', '.', '.' }, 
                { '.', '.', '.', '.' }
            };
            var expected = new int?[4,4]
            {
                { 1, null, 1, 0 },
                { 2, 2, 2, 0 },
                { 1, null, 1, 0 },
                { 1, 1, 1, 0 }
            };

            // Act
            var result = minesweeper.CalculateHints(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MineCellShouldShowANullHint()
        {
            var input = new char[3,3] 
            {
                { '.', '.', '.' },
                { '*', '.', '.' },
                { '.', '.', '.' }
            };

            var result = minesweeper.CalculateHints(input);

            Assert.Equal(null, result[1,0]);
        }

        [Fact]
        public void CellWithNoAdjacentMineShowsAZeroHint()
        {
            var input = new char[3,3] 
            {
                { '.', '.', '.' },
                { '*', '.', '.' },
                { '.', '.', '.' }
            };

            var result = minesweeper.CalculateHints(input);

            Assert.Equal(0, result[0,2]);
            Assert.Equal(0, result[1,2]);
            Assert.Equal(0, result[2,2]);
        }

        [Fact]
        public void CellWithOneAdjacentMineShowsAOneHint()
        {
            var input = new char[3,3] 
            {
                { '.', '.', '.' },
                { '*', '.', '.' },
                { '.', '.', '.' }
            };

            var result = minesweeper.CalculateHints(input);

            Assert.Equal(1, result[0,0]);
            Assert.Equal(1, result[0,1]);
            Assert.Equal(1, result[1,1]);
            Assert.Equal(1, result[2,1]);
            Assert.Equal(1, result[2,0]);
        }

        [Fact]
        public void CellWithMultipleAdjacentMineShowsCorrectHint()
        {
            var input = new char[3,3] 
            {
                { '.', '.', '*' },
                { '*', '.', '.' },
                { '*', '.', '.' }
            };

            var result = minesweeper.CalculateHints(input);

            Assert.Equal(2, result[0,1]);
            Assert.Equal(3, result[1,1]);
            Assert.Equal(2, result[2,1]);
        }

        // The following test will not pass, since C# does not support tail recursion
        // https://github.com/dotnet/csharplang/discussions/2544

        // [Fact]
        // public void NoStackOverflow()
        // {
        //     var input = new char[10_000, 10_000];
        //     for (int r = 0; r < 10_000; r++)
        //     {
        //         for (int c = 0; c < 10_000; c++)
        //         {
        //             input[r, c] = '.';
        //         }
        //     }

        //     input[100, 100] = '*';
        //     input[100, 101] = '*';
        //     input[200, 200] = '*';

        //     var result = minesweeper.CalculateHints(input);


        // }
    }
}