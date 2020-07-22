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
    public delegate void BoardReset(int size);

    public partial class GameGrid : UserControl
    { 
        public Cell[,] GameBoard;
        Game gameEngine;

        public GameGrid(int size, int windowSize, BoardReset boardReset, Game inEngine)
        {
            InitializeComponent();
            gameEngine = inEngine;
            RegisterEvents();
            InitializeBoard(size, windowSize, boardReset);
        }


        private void RegisterEvents()
        {
            gameEngine.EndGame += (sender, e) => { this.Enabled = false; };
        }

        private void InitializeBoard(int size, int windowSize, BoardReset boardReset)
        {
            GameBoard = new Cell[size, size];
            this.Height = windowSize - (windowSize / 10);
            this.Width = this.Height;
            int cellSize = this.Height / (size);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    GameBoard[x, y] = new Cell(gameEngine, x, y, (sender, e) => { boardReset(size); },
                        gameEngine.board.IsCellRevealed(x, y) ? gameEngine.board.GetCell(x, y) : CellState.Hidden,
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
