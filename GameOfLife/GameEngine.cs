using System;

namespace GameOfLife
{
    class GameEngine
    {
        private Random random = new Random();
        public uint currentGeneration { get; private set; }
        private bool[,] field;
        private readonly int rows, cols;

        public GameEngine(int _rows, int _cols, int density)
        {
            rows = _rows;
            cols = _cols;
            field = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }

        public void next_Generation()
        {
            var copyfield = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = count_Neighbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                    {
                        copyfield[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        copyfield[x, y] = false;
                    }
                    else
                    {
                        copyfield[x, y] = field[x, y];
                    }
                }
            }

            field = copyfield;
            currentGeneration++;
        }

        public bool[,] getCurrent_Generation()
        {
            var result = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }

            return result;
        }

        private int count_Neighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y;

                    var hasLife = field[col, row];

                    if (hasLife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool validate_Cell_Position(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void update_Cell(int x, int y, bool state)
        {
            if(validate_Cell_Position(x, y))
            {
                field[x, y] = state;
            }
        }

        public void add_Cell(int x, int y)
        {
            update_Cell(x, y, state: true);
        }

        public void remove_Cell(int x, int y)
        {
            update_Cell(x, y, state: false);
        }
    }
}
