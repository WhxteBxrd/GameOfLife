using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Runtime.InteropServices;

namespace GameOfLifeSfml
{
    class Program
    {
        const int SW_HIDE = 0;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static GameEngine gameEngine;
        static RenderWindow window;
        static byte resolution = 3;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            uint windowWidth = 935;
            uint windowHight = 584;

            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 99;

            window = new RenderWindow(new VideoMode(windowWidth, windowHight), "GameOfLife", Styles.Default, settings);
            window.SetActive();
            window.Closed += new EventHandler(window_Closed);

            gameEngine = new GameEngine
                (
                    _rows: (int)windowHight / resolution,
                    _cols: (int)windowWidth / resolution,
                    density: 2
                );

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);

                window.MouseButtonPressed += window_MouseButtonPressed;

                var field = gameEngine.getCurrent_Generation();

                for (int x = 0; x < field.GetLength(0); x++)
                {
                    for (int y = 0; y < field.GetLength(1); y++)
                    {
                        if (field[x, y])
                        {
                            RectangleShape rectangle = new RectangleShape(new Vector2f(resolution-1, resolution-1));
                            rectangle.FillColor = new Color(200, 200, 200);
                            rectangle.Position = (Vector2f)new Vector2i(x * resolution, y * resolution);

                            rectangle.Draw(window, RenderStates.Default);
                        }
                    }
                }

                window.Display();
                window.SetTitle($"GameOfLife    Generation:{gameEngine.currentGeneration}");
                gameEngine.next_Generation();
            }
        }

        private static void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {

            if (e.Button == Mouse.Button.Left)
            {
                var x = e.X / resolution;
                var y = e.Y / resolution;

                gameEngine.add_Cell(x, y);
            }

            if(e.Button == Mouse.Button.Right)
            {
                var x = e.X / resolution;
                var y = e.Y / resolution;

                gameEngine.remove_Cell(x, y);
            }
        }

        private static void window_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}
