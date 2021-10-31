using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private GameEngine gameEngine;
        private Graphics graphics;
        private int resolution;

        public Form1()
        {
            InitializeComponent();
        }

        private void start_Game()
        {
            if (timer1.Enabled)
            {
                return;
            }

            nudDensity.Enabled = false;
            nudResolution.Enabled = false;
            resolution = (int)nudResolution.Value;

            gameEngine = new GameEngine
            (
                _rows: pictureBox1.Height / resolution,
                _cols: pictureBox1.Width / resolution,
                density: (int)(nudDensity.Minimum) + (int)nudDensity.Maximum - (int)nudDensity.Value
            );

            label3.Text = $"Generation:{gameEngine.currentGeneration}";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void draw_Next_Generation()
        {
            graphics.Clear(Color.Black);

            var field = gameEngine.getCurrent_Generation();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if(field[x, y])
                    {
                        graphics.FillRectangle(Brushes.Gray, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                }
            }

            pictureBox1.Refresh();
            label3.Text = $"Generation:{gameEngine.currentGeneration}";
            gameEngine.next_Generation();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            draw_Next_Generation();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!timer1.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.add_Cell(x, y);
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.remove_Cell(x, y);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            start_Game();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if(!timer1.Enabled)
            {
                return;
            }

            timer1.Stop();
            nudDensity.Enabled = true;
            nudResolution.Enabled = true;
        }
    }
}
