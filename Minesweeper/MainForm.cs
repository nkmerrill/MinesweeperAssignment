using System;
using System.Windows.Forms;
using System.Timers;


namespace Minesweeper
{
    /// <summary>
    /// The mainform for the application.
    /// </summary>
    public partial class MainForm : Form
    {
        GameGrid gameGrid;
        Game gameEngine;
        int windowSize;
        int menuStripSize;
        int statusStripSize;

        /// <summary>
        /// Default constructor. Sets default size 
        /// and captures menu strip and status strip height to adjust 
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
        /// Initialize game. Creates new game object and game grid.
        /// Removes the old game grid if it exists. Finally, loads the player stats.
        /// </summary>
        public void Initialize()
        {

            gameEngine = new Game();
            gameEngine.clock.OnTick(OnTimerTick);
            lblTimer.Text = gameEngine.clock.GetClock().ToString();
            gameEngine.EndGame += (sender, e) => { EndGameMessage(e); };
            if (gameGrid != null)
                this.Controls.Remove(gameGrid);
            gameGrid = new GameGrid(gameEngine.board.GetSize(), windowSize, RedrawBoard, gameEngine);
            this.Controls.Add(gameGrid);
            RedrawBoard(gameEngine.board.GetSize());

            Stats.LoadStats();
        }


        /// <summary>
        /// Has the grid object redraw all of its cells, 
        /// then repositions the grid in the window.
        /// </summary>
        /// <param name="size">The height and
        /// width of the board. The board is always a square.</param>
        public void RedrawBoard(int size)
        {
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
                    RedrawBoard(gameEngine.board.GetSize());
                }
                else
                {
                    windowSize = 400;
                    this.Width = windowSize;
                    this.Height = windowSize + menuStripSize + statusStripSize;
                    RedrawBoard(gameEngine.board.GetSize());
                }

            }
           if (this.Height != windowSize + menuStripSize + statusStripSize)
            {
                if (this.Height >= 400 + menuStripSize + statusStripSize)
                {
                    windowSize = this.Height - menuStripSize - statusStripSize;
                    this.Width = windowSize;
                    RedrawBoard(gameEngine.board.GetSize());
                }
                else
                {
                    windowSize = 400;
                    this.Width = windowSize;
                    this.Height = windowSize + menuStripSize + statusStripSize;
                    RedrawBoard(gameEngine.board.GetSize());
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

        }


        /// <summary>
        /// Displays a messagebox showing the player's wins, losses, winrate, and
        /// average time played.
        /// </summary>
        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Wins:{Stats.Wins}, Losses:{Stats.Losses}, " +
                $"Winrate:{(Stats.Losses!=0?(float)Stats.Wins / Stats.Losses:1)}" +
                $"\nAverage Game Time:{Stats.AvgTime} seconds.", "Lifetime Stats");

        }


        /// <summary>
        /// Quit game. If game is running, then ask for confirmation.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameEngine.gameIsOver ||
                MessageBox.Show("Are you sure you want to " +
                    "quit the game?",
                    "Confirm Quit",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                Close();
        }


        /// <summary>
        /// Reset option click event. Calls game initialization method. 
        /// Checks if game is currently going. If so, then asks for confirmation.
        /// </summary>
        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameEngine.gameIsOver ||
                MessageBox.Show("Are you sure you want to " +
                    "end the current game and start over?", 
                    "Confirm Restart", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                Initialize();
        }

        /// <summary>
        /// Prints game instructions.
        /// </summary>
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

        /// <summary>
        /// Prints program information dialog.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Designed by:\tNicholos Merrill\n"+
                             "For:\tCS 3020 Summer 2020 Semester, UCCS\n\n"+
                             "Finished:\tJuly 22, 2020";
            MessageBox.Show(message, "About this Application");
        }


        /// <summary>
        /// Event for a timer tick. Updates the status label to reflect any changes to
        /// the clock.
        /// </summary>
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            lblTimer.Text = gameEngine.clock.GetClock().ToString();
        }

        /// <summary>
        /// Message displayed when the game is over.
        /// </summary>
        /// <param name="e">EndGameArgs saying whether the game was won or lost
        /// and how long the game lasted.</param>
        private void EndGameMessage(EndGameArgs e)
        {
            string message = $"You {(e.isWin ? "won" : "lost")} after {e.elapsedTime} second(s)!\n"+
                "\t\tPlay again?";
            if(MessageBox.Show(message,"Game Over!", MessageBoxButtons.YesNo)==DialogResult.Yes)
                Initialize();
        }
    }
}
