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
        public Strategies.HumanStrategy human = new Strategies.HumanStrategy();
        public GameForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            gameManager.StartGame(new Strategies.AlphaBetaStrategy(3), new Strategies.AlphaBetaStrategy(3));
            //gameManager.StartGame(new Strategies.AlphaBetaStrategy(3), new Strategies.MonteCarloStrategy(10000));
            //gameManager.StartGame(new Strategies.AlphaBetaStrategy(8), human);
            BoardClick += human.SetMove;
            //gameManager.StartGame(human, new Strategies.AlphaBetaStrategy(2));
            //gameManager.StartGame(human, human);
            //gameManager.StartGame(new Strategies.MonteCarloStrategy(1000), human);
            gameManager.MoveDone += () => Invoke(new Action(Refresh));
        }

        private void GameCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / CellSize, y = e.Y / CellSize;
            if (0 <= x && x < UltimateTicTacToe.BoardSize && 0 <= y && y < UltimateTicTacToe.BoardSize)
                BoardClick?.Invoke(x, y);
        }
        private event Action<int, int> BoardClick;

        private const int CellSize = 35;
        private const int CellBorder = 5;

        private readonly Font winnerFont = new Font("Arial", 30);

        private readonly Brush brushX = Brushes.Cyan;
        private readonly Pen lineX = new Pen(Brushes.Blue, 2);
        private readonly Pen lastLineX = new Pen(Brushes.Blue, 4);
        private readonly Pen winLineX = new Pen(Brushes.Blue, 10);

        private readonly Brush brushO = Brushes.Pink;
        private readonly Pen lineO = new Pen(Brushes.Red, 2);
        private readonly Pen lastLineO = new Pen(Brushes.Red, 4);
        private readonly Pen winLineO = new Pen(Brushes.Red, 10);

        private readonly Pen thinLine = Pens.Black;
        private readonly Pen fatLine = new Pen(Brushes.Black, 3);
        private void DrawX(Graphics g, int x, int y, Pen pen, int cellSize = CellSize, int cellBorder = CellBorder)
        {
            g.DrawLine(pen, x * cellSize + cellBorder, y * cellSize + cellBorder, x * cellSize + cellSize - cellBorder, y * cellSize + cellSize - cellBorder);
            g.DrawLine(pen, x * cellSize + cellSize - cellBorder, y * cellSize + cellBorder, x * cellSize + cellBorder, y * cellSize + cellSize - cellBorder);
        }
        private void DrawO(Graphics g, int x, int y, Pen pen, int cellSize = CellSize, int cellBorder = CellBorder)
        {
            g.DrawEllipse(pen, x * cellSize + cellBorder, y * cellSize + cellBorder, cellSize - 2 * cellBorder, cellSize - 2 * cellBorder);
        }
        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            // TODO: Read board from local copy
            Brush brush = gameManager.Board.PlayerMove == Players.First ? brushX : brushO;
            foreach (PlayerMove move in gameManager.Board.GetAllMoves())
            {
                e.Graphics.FillRectangle(brush, move.x * CellSize, move.y * CellSize, CellSize, CellSize);
            }
            for (int i = 0; i <= UltimateTicTacToe.BoardSize; i++)
            {
                e.Graphics.DrawLine(i % Board.LocalBoardSize == 0 ? fatLine : thinLine, 0, i * CellSize, UltimateTicTacToe.BoardSize * CellSize, i * CellSize);
                e.Graphics.DrawLine(i % Board.LocalBoardSize == 0 ? fatLine : thinLine, i * CellSize, 0, i * CellSize, UltimateTicTacToe.BoardSize * CellSize);
            }
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                {
                    if (gameManager.Board[i, j] == Players.First)
                        DrawX(e.Graphics, i, j, i == gameManager.Board.LastAction.x && j == gameManager.Board.LastAction.y ? lastLineX : lineX);
                    else if (gameManager.Board[i, j] == Players.Second)
                        DrawO(e.Graphics, i, j, i == gameManager.Board.LastAction.x && j == gameManager.Board.LastAction.y ? lastLineO : lineO);
                }
            for (int i = 0; i < Board.LocalBoardSize; i++)
                for (int j = 0; j < Board.LocalBoardSize; j++)
                {
                    if (gameManager.Board.GetOwner(i, j) == Players.First)
                        DrawX(e.Graphics, i, j, winLineX, CellSize * Board.LocalBoardSize, CellBorder * Board.LocalBoardSize);
                    else if (gameManager.Board.GetOwner(i, j) == Players.Second)
                        DrawO(e.Graphics, i, j, winLineO, CellSize * Board.LocalBoardSize, CellBorder * Board.LocalBoardSize);
                }
            if (gameManager.Board.IsFinished)
            {
                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                e.Graphics.DrawString(gameManager.Board.Winner.ToString(), winnerFont, Brushes.Black, GameCanvas.ClientRectangle, sf); // UltimateTicTacToe.BoardSize * CellSize / 2, UltimateTicTacToe.BoardSize * CellSize / 2 - 10
            }
        }
    }
}
