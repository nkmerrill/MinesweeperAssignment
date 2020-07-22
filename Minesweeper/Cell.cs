using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace Minesweeper
{

    public delegate void CellClick(object sender, EventArgs e);

    /// <summary>
    /// Customer user control to represent a cell in the minesweeper game. Consists of a button, and panel, and a label.
    /// </summary>
    public partial class Cell : UserControl
    {
        Button button = new Button();
        Panel panel = new Panel();
        Label label = new Label();
        Game gameEngine;
        int x, y;


        /// <summary>
        /// Constructor should at least take the game object, coordinates, and a delegate . This is required for eventhandling.
        /// </summary>
        public Cell(Game game, int newX, int newY, CellClick cellClick, CellState newCellState = CellState.Blank, int size = 25)
        {
            gameEngine = game;
            x = newX;
            y = newY;
            InitializeComponent();
            InitializeClickDelegates(cellClick);
            Initialize(newCellState, size);
        }

        /// <summary>
        /// Set elements to default values.
        /// </summary>
        public void Initialize(CellState newCellState, int size)
        {

            ChangeSize(size);

            this.Controls.Add(button);
            this.Controls.Add(label);
            this.Controls.Add(panel);

            SetCellState(newCellState);
        }

        public void ChangeSize(int size)
        {
            this.Width = size;
            this.Height = size;

            button.Width = size;
            button.Height = size;

            panel.Width = size;
            panel.Height = size;

            label.Width = size;
            label.Height = size;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Font = new Font(label.Font.FontFamily, 8.6f * this.Width / 40);
        }

        /// <summary>
        /// Changes the state of the cell to match the current status of the given space on the board.
        /// </summary>
        /// <param name="newStatus">The new status to represent.</param>
        public void SetCellState(CellState newStatus)
        {
            switch (newStatus)
            {
                case CellState.Hidden:
                    button.Visible = true;
                    label.Visible = false;
                    panel.Visible = false;
                    break;

                case CellState.Blank:
                    button.Visible = false;
                    label.Visible = false;
                    panel.Visible = true;
                    break;

                case CellState.One:
                    button.Visible = false;

                    label.Text = "1";
                    label.ForeColor = Color.Blue;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Two:
                    button.Visible = false;

                    label.Text = "2";
                    label.ForeColor = Color.DarkCyan;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Three:
                    button.Visible = false;

                    label.Text = "3";
                    label.ForeColor = Color.DarkTurquoise;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Four:
                    button.Visible = false;

                    label.Text = "4";
                    label.ForeColor = Color.DarkOliveGreen;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Five:
                    button.Visible = false;

                    label.Text = "5";
                    label.ForeColor = Color.SandyBrown;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Six:
                    button.Visible = false;

                    label.Text = "6";
                    label.ForeColor = Color.Orange;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Seven:
                    button.Visible = false;

                    label.Text = "7";
                    label.ForeColor = Color.DarkOrange;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Eight:
                    button.Visible = false;

                    label.Text = "8";
                    label.ForeColor = Color.Red;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                case CellState.Bomb:
                    button.Visible = false;

                    label.Text = "!B!";
                    label.ForeColor = Color.DarkRed;
                    label.Visible = true;

                    panel.BackColor = Color.White;
                    panel.Visible = true;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();

            }

        }
        



        /// <summary>
        /// Register delegates for button click event handler.
        /// </summary>
        /// <param name="clickDelegate">Delegate of a standard event method.</param>
        public void InitializeClickDelegates(CellClick cellClick)
        {
            button.Click += new EventHandler(OnClick);
            button.Click += new EventHandler(cellClick);
        }

        private void OnClick(object sender, EventArgs e)
        {
            gameEngine.ClickSpace(x,y);
            SetCellState(gameEngine.board.IsCellRevealed(x, y) ? gameEngine.board.GetCell(x, y) : CellState.Hidden);
        }
    }
}
