using Snake.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Windows.LAN
{
    public partial class Form2 : Form
    {
        Snake.Net.LAN.Client client;
        Snake.Net.PlayerController player;

        int scale = 20;

        Brush myBrush = Brushes.Blue;
        Brush otherBrush = Brushes.Red;
        Brush tailBrush = Brushes.Gray;
        Brush foodBrush = Brushes.Green;

        public Form2(string host, int serverPort, int dataPort, string name)
        {
            InitializeComponent();

            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            if (screenHeight < 956)
            {
                this.Height = 656;
                this.Width = 822;
                scale = 10;
            }
            else if (screenHeight < 1256)
            {
                this.Height = 956;
                this.Width = 1222;
                scale = 15;
            }

            this.KeyPress += Form2_KeyPress;
            this.FormClosing += Form2_FormClosing;
            this.KeyDown += Form2_KeyDown;
            this.KeyUp += Form2_KeyUp;

            client = new(host, serverPort, dataPort);
            client.Next += Draw;
            player = new(client, name);
        }

        private void Form2_FormClosing(object? sender, FormClosingEventArgs e)
        {
            player.Exit();
        }

        private void Form2_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (player == null) return;
            switch (e.KeyChar)
            {
                case 'W':
                case 'w':
                    player.Turn(Game.Direction.Up);
                    break;
                case 'S':
                case 's':
                    player.Turn(Game.Direction.Down);
                    break;
                case 'A':
                case 'a':
                    player.Turn(Game.Direction.Left);
                    break;
                case 'D':
                case 'd':
                    player.Turn(Game.Direction.Right);
                    break;
                default:
                    break;
            }
        }

        private void Form2_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Shift)
                player.Speed(1);
        }

        private void Form2_KeyUp(object? sender, KeyEventArgs e)
        {
            if (!e.Shift)
                player.Speed(0);
        }

        private void Draw(GameInformationPlus? game)
        {
            if (game == null) return;
            var tails = game.Tails();
            var heads = game.Heads();
            var food = game.Food();
            var lengthes = ((IGameInformation)game).LengthList();
            var names = game.Names();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            foreach (var food1 in food) g.FillRectangle(foodBrush, food1.x * scale, food1.y * scale, scale, scale);
            foreach (var tail in tails) g.FillRectangle(tailBrush, tail.x * scale, tail.y * scale, scale, scale);
            foreach (var headKvp in heads) 
            {
                if (player != null && headKvp.Key == player.id) g.FillRectangle(myBrush, headKvp.Value.x * scale, headKvp.Value.y * scale, scale, scale);
                else g.FillRectangle(otherBrush, headKvp.Value.x * scale, headKvp.Value.y * scale, scale, scale);
            }
            float y = 10;
            float n = 1;
            foreach (var (id, length) in lengthes)
            {
                g.DrawString($"{n} {names[id]} {length}", new Font("微软雅黑", 14),player != null && id == player.id ? myBrush : otherBrush, 10, y);
                y += 30;
                n++;
            }
        }
    }
}
