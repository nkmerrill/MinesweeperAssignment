using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Primary game engine object. Handles game specific behaviors.
    /// </summary>
    public class Game
    {

        /// <summary>
        /// Dalegate for the endgame event handler. 
        /// </summary>
        /// <param name="e">Passes whether the game was won or lost and how long 
        /// the game lasted.</param>
        public delegate void EndGameHandler(object sender, EndGameArgs e);

        /// <summary>
        /// Custom event for the end of the game.
        /// </summary>
        public event EndGameHandler EndGame;

        /// <summary>
        /// The game implementation of the board. Handles game logic relating to the
        /// game's cells.
        /// </summary>
        public Board board;

        /// <summary>
        /// Game clock that tracks the time, in seconds, the game is being played.
        /// </summary>
        public Clock clock;


        int bombCount;
        bool firstTurnTaken = false;
        public bool gameIsOver { get; private set; } = false;
        Random rand = new Random();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Game()
        {
            board = new Board();
            clock = new Clock();
            bombCount = 10;
            EndGame += (sender, e) => { clock.StopClock(); 
                gameIsOver = true; 
                Stats.SaveStats(e.isWin, e.elapsedTime); };

        }

        /// <summary>
        /// The game is assumed to start from a button click.
        /// A flag is used to track the first turn. If it is the first turn, then
        /// start the game clock and populate the bomb positions.
        /// </summary>
        /// <param name="x">x coordinate of the cell clicked.</param>
        /// <param name="y">y coordinate of the cell clicked.</param>
        private void StartGame(int x, int y)
        {

            firstTurnTaken = true;
            clock.Start();
            PopulateBombs(x, y);

        }


        /// <summary>
        /// Bombs are placed only in cells that do not already have a bomb. Also avoid the
        /// selected square. Then, iterate through adjacent cells and increment the cell
        /// state so the cell indicates the number of bombs adjacent to it.
        /// </summary>
        /// <param name="x">x coordinate of the cell clicked.</param>
        /// <param name="y">y coordinate of the cell clicked.</param>
        private void PopulateBombs(int x, int y)
        {
            int col, row;
            for (int i = 0; i < bombCount; i++)
            {
                col = rand.Next(0, 9);
                row = rand.Next(0, 9);

                if ((board.GetCell(row, col) != CellState.Bomb) && ((row != x || col != y)))
                {
                    //Modify an offset between -1 and 1 of row and col.
                    for (int j = -1; j < 2; j++)
                        for (int n = -1; n < 2; n++)
                        {
                            //Ensure we are not out of bounds
                            if (row + n < 0 || row + n >= board.GetSize() ||
                                col + j < 0 || col + j >= board.GetSize())
                                continue;

                            //Set center square to a bomb.
                            if (n == 0 && j == 0)
                                board.SetCell(row + n, col + j, CellState.Bomb);

                            //If the square is a bomb leave it alone, others increment the adjacent bomb count.
                            else if (board.GetCell(row + n, col + j) == CellState.Blank)
                                board.SetCell(row + n, col + j, CellState.One);

                            else
                                board.SetCell(row + n, col + j,
                                    board.GetCell(row + n, col + j) != CellState.Bomb ? 
                                    board.GetCell(row + n, col + j) + 1 :
                                    CellState.Bomb);
                        }
                }
                else
                    i--; //Try again

            }
        }

        /// <summary>
        /// Triggered when a cell is clicked. If this is the first turn, call the
        /// start game method to populate the cell states. Then, calls a recursive function
        /// to reveal any blank spaces around the current one. Finally, verifies end game
        /// conditions by checking for a bomb click and a win state.
        /// </summary>
        /// <param name="x">x coordinate of the cell clicked.</param>
        /// <param name="y">y coordinate of the cell clicked.</param>
        public void ClickSpace(int x, int y)
        {
            if (!firstTurnTaken)
                StartGame(x,y);
            RevealAdjacent(x,y);

            if (board.GetCell(x, y) == CellState.Bomb)
                BombClick();
            CheckWin(x, y);
        }


        /// <summary>
        /// Recursive function to reveal blank cells. Checks the 8 cells around the given
        /// cell, and calls the function again for that cell if it is blank.
        /// Only runs on cells that are not revealed to prevent an infinite loop.
        /// </summary>
        /// <param name="x">x coordinate of the cell clicked.</param>
        /// <param name="y">y coordinate of the cell clicked.</param>
        private void RevealAdjacent(int x, int y)
        {
            if (!board.IsCellRevealed(x, y))
            {
                board.SetCellRevealed(x, y, true);
                if (board.GetCell(x, y) == CellState.Blank)
                {
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            if ((x + i < 0) || 
                                (x + i >= board.GetSize()) || 
                                (y + j < 0) || 
                                (y + j >= board.GetSize()))
                                continue;
                            if (board.GetCell(x + i, y + j) == CellState.Blank 
                                && ((i != 0) || (j != 0)))
                                RevealAdjacent(x + i, y + j);
                            else
                                board.SetCellRevealed(x+i, y+j, true);
                        }
                }
            }
        }


        /// <summary>
        /// Uses a LINQ query to count the number of unrevealed tiles.
        /// If the count is less than or equal to the number of bombs,
        /// then the player wins.
        /// </summary>
        /// <param name="x">x coordinate of the cell clicked.</param>
        /// <param name="y">y coordinate of the cell clicked.</param>
        public void CheckWin(int x, int y)
        {
            var query = from BoardCell c in board.GetAllCells()
                        where c.isRevealed == false
                        select c;

            if (query.Count() <= bombCount)
                GameOver(new EndGameArgs { isWin = true, elapsedTime = clock.GetClock() });

        }

        /// <summary>
        /// Triggered when a bomb is click. Calls the game over method, that triggers the
        /// end game event.
        /// </summary>
        public void BombClick()
        {
            GameOver(new EndGameArgs { isWin = false, elapsedTime = clock.GetClock() });
        }

        /// <summary>
        /// Calls the end game event.
        /// </summary>
        /// <param name="e">Indicates whether the game was won or lost, 
        /// and how long the game lasted.</param>
        private void GameOver(EndGameArgs e)
        {
            EndGame?.Invoke(this, e);
        }
    }


    /// <summary>
    /// Arguments for end game event. 
    /// Includes:
    /// isWin, which indicates if the player won or lost
    /// elapsedTime, indicating how long the game was played in seconds.
    /// </summary>
    public class EndGameArgs : EventArgs
    {
        public bool isWin;
        public int elapsedTime;
    }
}
