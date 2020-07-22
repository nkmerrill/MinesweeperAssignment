using System.Collections.Generic;

namespace Minesweeper
{
    /// <summary>
    /// Structure that pairs a boardState and a isRevealed flag.
    /// </summary>
    public struct BoardCell
    {
        public CellState cellState;
        public bool isRevealed;
    }

    /// <summary>
    /// Class to represent the game implementation of the board. the board will hold the real values of the given cells, 
    /// even if not revealed.
    /// </summary>
    public class Board
    {
        private BoardCell[,] cells;
        private readonly int size;


        /// <summary>
        /// Default constructor with size of 10.
        /// </summary>
        public Board()
        {
            size = 10;
            ResetBoard();
        }

        /// <summary>
        /// Constructor that allows a different size than 10, a feature to implement later.
        /// </summary>
        /// <param name="newSize">the size to initiate the board with.</param>
        public Board(int newSize)
        {
            size = newSize;
            ResetBoard();
        }

        /// <summary>
        /// Accessor function for board size.
        /// </summary>
        /// <returns>Size of the board</returns>
        public int GetSize()
        {
            return size;
        }

        /// <summary>
        /// Resets the board to default values. Sets all cells to blank and not revealed.
        /// </summary>
        public void ResetBoard()
        {
            cells = new BoardCell[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    cells[i,j] = new BoardCell { cellState = CellState.Blank, isRevealed = false };

        }

        /// <summary>
        /// Returns whether the indicated cell should be revealed to the player or not.
        /// </summary>
        /// <param name="x">x coordinate of cell</param>
        /// <param name="y">y coordinate of cell</param>
        /// <returns>A boolean to represent whether the cell should be revealed to the player</returns>
        public bool IsCellRevealed(int x, int y)
        {
            return cells[x, y].isRevealed;
        }

        /// <summary>
        /// Sets the revealed property of the given cell.
        /// </summary>
        /// <param name="x">x coordinate of the cell</param>
        /// <param name="y">y coordinate of the cell</param>
        /// <param name="newStatus">A boolean representating whether the cell should be revealed to the player or not.</param>
        public void SetCellRevealed(int x, int y, bool newStatus)
        {
            cells[x, y].isRevealed = newStatus;
        }

        /// <summary>
        /// Change the CellState of the cell at (x,y).
        /// </summary>
        /// <param name="x">x coordinate of the cell</param>
        /// <param name="y">y coordinate of the cell</param>
        /// <param name="newState">The new state for the cell</param>
        public void SetCell(int x, int y, CellState newState)
        {
            cells[x, y].cellState = newState;
        }

        /// <summary>
        /// Get the current state of the given cell.
        /// </summary>
        /// <param name="x">x coordinate of the cell</param>
        /// <param name="y">y coordinate of the cell</param>
        /// <returns>The cell state of the given cell</returns>
        public CellState GetCell(int x, int y)
        {
            return cells[x, y].cellState;
        }

        /// <summary>
        /// Accessor function for the BoardCell array.
        /// </summary>
        /// <returns>Array of BoardCell</returns>
        public List<BoardCell> GetAllCells()
        {
            List<BoardCell> output = new List<BoardCell>();
            foreach (BoardCell c in cells)
                output.Add(c);
            return output;
        }
    }
}
