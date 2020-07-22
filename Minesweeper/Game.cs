using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{

    public class Game
    {
        public delegate void EndGameHandler(object sender, EndGameArgs e);
        public event EndGameHandler EndGame;

        public Board board;
        public Clock clock;

        int bombCount;
        bool firstTurnTaken = false;
        Random rand = new Random();

        public Game()
        {
            board = new Board();
            clock = new Clock();
            bombCount = 10;
            EndGame += (sender, e) => { clock.StopClock(); };
            EndGame += (sender, e) => { Stats.SaveStats(e.isWin,e.elapsedTime); };
        }

        public void StartGame(int x, int y)
        {

            firstTurnTaken = true;
            clock.Start();
            PopulateBombs(x, y);

        }



        public void PopulateBombs(int x, int y)
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
                            if (row + n < 0 || row + n >= board.GetSize() || col + j < 0 || col + j >= board.GetSize())
                                continue;

                            //Set center square to a bomb.
                            if (n == 0 && j == 0)
                                board.SetCell(row + n, col + j, CellState.Bomb);

                            //If the square is a bomb leave it alone, others increment the adjacent bomb count.
                            else if (board.GetCell(row + n, col + j) == CellState.Blank)
                                board.SetCell(row + n, col + j, CellState.One);

                            else
                                board.SetCell(row + n, col + j,
                                    board.GetCell(row + n, col + j) != CellState.Bomb ? board.GetCell(row + n, col + j) + 1 :
                                    CellState.Bomb);
                        }
                }
                else
                    i--; //Try again

            }
        }


        public void ClickSpace(int x, int y)
        {
            if (!firstTurnTaken)
                StartGame(x,y);
            RevealAdjacent(x,y);

            if (board.GetCell(x, y) == CellState.Bomb)
                BombClick();
            CheckWin(x, y);
        }

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
                            if ((x + i < 0) || (x + i >= board.GetSize()) || (y + j < 0) || (y + j >= board.GetSize()))
                                continue;
                            if ((board.GetCell(x, y) == board.GetCell(x + i, y + j)) && ((i != 0) || (j != 0)))
                                RevealAdjacent(x + i, y + j);
                            else
                                board.SetCellRevealed(x+i, y+j, true);
                        }
                }
            }
        }

        public void CheckWin(int x, int y)
        {
            var query = from BoardCell c in board.GetAllCells()
                        where c.isRevealed == false
                        select c;

            if (query.Count() <= bombCount)
                GameOver(new EndGameArgs { isWin = true, elapsedTime = clock.GetClock() });

        }

        public void BombClick()
        {
            GameOver(new EndGameArgs { isWin = false, elapsedTime = clock.GetClock() });
        }

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
