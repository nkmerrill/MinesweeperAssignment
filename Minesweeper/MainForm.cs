using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace Minesweeper
{
    public partial class MainForm : Form
    {
        GameGrid gameGrid;
        Game gameEngine;
        int windowSize;
        int menuStripSize;
        int statusStripSize;

        /// <summary>
        /// Default constructor. Sets default size and captures menu strip and status strip height to adjust 
        /// window height. Then initializes game.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            windowSize = 400;
            menuStripSize = this.Controls.Find("menuStrip", false)[0].Height;
            statusStripSize = this.Controls.Find("statusStrip", false)[0].Height;
            Initialize();
        }



        /// <summary>
        /// Initialize game. Creates new game object and resets board.
        /// </summary>
        public void Initialize()
        {

            gameEngine = new Game();
            gameEngine.clock.OnTick(OnTimerTick);
            lblTimer.Text = gameEngine.clock.GetClock().ToString();
            gameEngine.EndGame += (sender, e) => { EndGameMessage(e); };
            if (gameGrid != null)
                this.Controls.Remove(gameGrid);
            gameGrid = new GameGrid(gameEngine.board.GetSize(), windowSize, ResetBoard, gameEngine);
            this.Controls.Add(gameGrid);
            ResetBoard(gameEngine.board.GetSize());
        }


        /// <summary>
        /// Create grid of cells.
        /// </summary>
        /// <param name="size">The dimensions of the board. The board is always a square.</param>
        public void ResetBoard(int size)
        {
            //if (gameGrid != null)
            //    this.Controls.Remove(gameGrid);

            gameGrid.DrawBoard(size, windowSize);
            gameGrid.Anchor = AnchorStyles.None;
            gameGrid.Left = (this.ClientSize.Width-gameGrid.Width)/2;
            gameGrid.Top = (this.ClientSize.Height-gameGrid.Height)/2;
        }



        /// <summary>
        /// Form resize event. Ensures that the window maintains a square, 
        /// and redraws the board if needed to match the new window size.
        /// </summary>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.Width != windowSize)
            {
                if (this.Width >= 400)
                {
                    windowSize = this.Width;
                    this.Height = windowSize + menuStripSize + statusStripSize;
                    ResetBoard(gameEngine.board.GetSize());
                }
                else
                {
                    windowSize = 400;
                    this.Width = windowSize;
                    this.Height = windowSize + menuStripSize + statusStripSize;
                    ResetBoard(gameEngine.board.GetSize());
                }

            }
           if (this.Height != windowSize + menuStripSize + statusStripSize)
            {
                if (this.Height >= 400 + menuStripSize + statusStripSize)
                {
                    windowSize = this.Height - menuStripSize - statusStripSize;
                    this.Width = windowSize;
                    ResetBoard(gameEngine.board.GetSize());
                }
                else
                {
                    windowSize = 400;
                    this.Width = windowSize;
                    this.Height = windowSize + menuStripSize + statusStripSize;
                    ResetBoard(gameEngine.board.GetSize());
                }
            }

        }

        /// <summary>
        /// Load form event. Initializes the size of the window. Window should be a square.
        /// Note: Height is adjusted to make room for the menu strip and status strip.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Width = windowSize;
            this.Height = windowSize + menuStripSize + statusStripSize;
            Stats.LoadStats();

        }


        /// <summary>
        /// Displays a messagebox showing the player's wins, losses, winrate, and
        /// average time played.
        /// </summary>
        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Wins:{Stats.Wins}, Losses:{Stats.Losses}, " +
                $"Winrate:{(float)Stats.Wins / Stats.Losses}" +
                $"\nAverage Game Time:{Stats.AvgTime} seconds.", "Lifetime Stats");

        }


        /// <summary>
        /// Quit game.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Reset option click event. Calls game initialization method.
        /// </summary>
        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize();
        }


        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "=============MINESWEEPER=============\n" +
                             "\t\t     How to play             \n" +
                             "___________________________________________________\n" +
                             "The goal of minesweeper is to reveal every square except\n" +
                             "the squares with a bomb. The location of bombs can be predicted\n" +
                             "using adjacent box numbers. Click a box to reveal it.";
            MessageBox.Show(message, "How to Play");
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Designed by:\tNicholos Merrill\n"+
                             "For:\tCS 3020 Summer 2020 Semester, UCCS\n\n"+
                             "Finished:\tJuly 22, 2020";
            MessageBox.Show(message, "About this Application");
        }


        /// <summary>
        /// Event for a timer tick.
        /// </summary>
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            lblTimer.Text = gameEngine.clock.GetClock().ToString();
        }

        private void EndGameMessage(EndGameArgs e)
        {
            string message = $"You {(e.isWin ? "won" : "lost")} after {e.elapsedTime} second(s)!\n"+
                "\t\tPlay again?";
            if(MessageBox.Show(message,"Game Over!", MessageBoxButtons.YesNo)==DialogResult.Yes)
                Initialize();
        }
    }
}
