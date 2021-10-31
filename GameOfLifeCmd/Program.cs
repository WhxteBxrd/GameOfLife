using System;
using System.Runtime.InteropServices;

namespace GameOfLifeCmd
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int MAXIMIZE = 3;

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(GetConsoleWindow(), MAXIMIZE);
            Console.WriteLine("Press Enter to Start...");
            Console.ReadLine();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Clear();

            var gameEngine = new GameEngine
                (
                    _rows: 149,
                    _cols: 675,
                    density: 2
                );

            while (true)
            {
                Console.Title = gameEngine.currentGeneration.ToString();

                var field = gameEngine.getCurrent_Generation();

                for (int y = 0; y < field.GetLength(1); y++)
                {
                    var line = new char[field.GetLength(0)];

                    for (int x = 0; x < field.GetLength(0); x++)
                    {
                        if (field[x, y])
                        {
                            line[x] = '#';
                        }
                        else
                        { 
                            line[x] = ' ';
                        }
                    }

                    Console.WriteLine(line);
                }

                Console.SetCursorPosition(0, 0);
                gameEngine.next_Generation();
            }
        }
    }
}
