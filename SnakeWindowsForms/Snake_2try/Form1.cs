using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake_2try
{
    public partial class Snake_Game : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Snake_Game()
        {
            InitializeComponent();

            // Set settings to default
            new Settings();

            // Set game speed and start timer
            GameTimer.Interval = 1000 / Settings.Speed;
            GameTimer.Tick += UpdateScreen;
            GameTimer.Start();

            // Start New Game
            StartGame();
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            // Check for Game Over
            if (Settings.GameOver)
            {
                // Ckeck if Entr=er is pressed
                if (Inputs.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Inputs.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Inputs.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Inputs.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Inputs.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }
            pbCanvas.Invalidate();
        }

        private void StartGame()
        {
            LableGameOver.Visible = false;

            // Set settings to default
            new Settings();

            Snake.Clear();

            // Create new player object
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            LabelScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        // Place a food in a random position
        private void GenerateFood()
        {
            int MaxXPosition = pbCanvas.Size.Width / Settings.Width;
            int MaxYPosition = pbCanvas.Size.Height / Settings.Heigth;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, MaxXPosition);
            food.Y = random.Next(0, MaxYPosition);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                // Set colour of Snake
                Brush SnakeColour;

                // Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {

                    if (i == 0)
                        SnakeColour = Brushes.Black;    // Draw head
                    else
                        SnakeColour = Brushes.Green;   //Rest of body

                    // Draw snake
                    canvas.FillEllipse(SnakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                        Snake[i].Y * Settings.Heigth,
                                        Settings.Width,
                                        Settings.Heigth));

                    // Draw Food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width, food.Y * Settings.Heigth,
                        Settings.Width, Settings.Heigth));
                }
            }
            else
            {
                string GameOver = "Game over \nYour final score is " + Settings.Score + "\nPress Enter to try again";
                LableGameOver.Text = GameOver;
                LableGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                // Move head
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    // Detectcollision with hame world
                    int MaxXPosition = pbCanvas.Size.Width / Settings.Width;
                    int MaxYPosition = pbCanvas.Size.Height / Settings.Heigth;

                    if (Snake[i].X < 0 ||
                        Snake[i].Y < 0 ||
                        Snake[i].X >= MaxXPosition ||
                        Snake[i].Y >= MaxYPosition)
                    {
                        Die();
                    }

                    //Detect coillision withbody
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].X == Snake[j].X
                            && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    // Detect colilision with food place
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    // Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            // Update score
            Settings.Score += Settings.Points;
            LabelScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Inputs.ChangState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Inputs.ChangState(e.KeyCode, false);
        }
    }
}
