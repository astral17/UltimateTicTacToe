using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateTicTacToe
{
    public partial class GameForm : Form
    {
        private readonly GameManager gameManager = new GameManager();
        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void GameCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            //gameManager.StartGame(null, null);
            //Board board = new Board();
            //Console.WriteLine(board.MakeMove(Players.First, 0 , 0));
            //Console.WriteLine(board.MakeMove(Players.Second, 1 , 0));
            //board.DebugPrint();
        }

        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, 5, 5, 20, 20);
            //gameManager.Board[0, 0]
            //gameManager.Board.LocalBoardResult(0, 0)
        }
    }
}
