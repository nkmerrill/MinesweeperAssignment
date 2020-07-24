using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// Delegate of a board reset event.
    /// </summary>
    /// <param name="size">Number of cells in a given column or row.</param>
    public delegate void BoardReset(int size);

    /// <summary>
    /// Gamegrid to contain the cells of the gameboard.
    /// </summary>
    public partial class GameGrid : UserControl
    { 
        public Cell[,] GameBoard;
        Game gameEngine;

        /// <summary>
        /// Contructor. Requires a size, windowsize, a boardreset delegate, 
        /// and the Game object.
        /// </summary>
        /// <param name="size">Number of cells in a given column or row.</param>
        /// <param name="windowSize">Size of the length or width of the board.</param>
        /// <param name="boardReset">Delegate for the board reset event.</param>
        /// <param name="inEngine"></param>
        public GameGrid(int size, int windowSize, BoardReset boardReset, Game inEngine)
        {
            InitializeComponent();
            gameEngine = inEngine;
            RegisterEvents();
            InitializeBoard(size, windowSize, boardReset);
        }

        /// <summary>
        /// Creates a anonymous method to pass to the EndGame event. 
        /// This disables the board when the game is over.
        /// </summary>
        private void RegisterEvents()
        {
            gameEngine.EndGame += (sender, e) => { this.Enabled = false; };
        }


        /// <summary>
        /// Creates the cells within the board.
        /// </summary>
        /// <param name="size">Number of cells in a given column or row.</param>
        /// <param name="windowSize">The size of window's height or width.</param>
        /// <param name="boardReset">A passed delegate to allow button click events to redraw board.</param>
        private void InitializeBoard(int size, int windowSize, BoardReset boardReset)
        {
            GameBoard = new Cell[size, size];
            this.Height = windowSize - (windowSize / 10);
            this.Width = this.Height;
            int cellSize = this.Height / (size);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    GameBoard[x, y] = new Cell(gameEngine, x, y, 
                        (sender, e) => { boardReset(size); },
                        gameEngine.board.IsCellRevealed(x, y) ?
                        gameEngine.board.GetCell(x, y) : CellState.Hidden,
                        cellSize)
                    {
                        Name = $"Cell{x}{y}",
                        Margin = new Padding(0),
                        Padding = new Padding(0),
                    };
                    this.Controls.Add(GameBoard[x, y]);
                    GameBoard[x, y].Location = new Point((x * cellSize), (y * cellSize));
                }
        }

        /// <summary>
        /// Repositions buttons and modifies the state of the buttons to match the
        /// current game-relevant status.
        /// </summary>
        /// <param name="size">Number of cells in a given column or row.</param>
        /// <param name="windowSize">The size of window's height or width.</param>
        public void DrawBoard(int size, int windowSize)
        {
            this.Height = windowSize - (windowSize / 10);
            this.Width = this.Height;
            int cellSize = this.Height / (size);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    GameBoard[x, y].ChangeSize(cellSize);
                    GameBoard[x, y].Location = new Point((x * cellSize), (y * cellSize));
                    GameBoard[x, y].SetCellState(gameEngine.board.IsCellRevealed(x,y)?
                        gameEngine.board.GetCell(x, y):CellState.Hidden);
                }
        }



    }
}
