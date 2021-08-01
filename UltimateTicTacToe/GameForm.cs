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
            //gameManager.StartGame(new Strategies.AlphaBetaStrategy(8), new Strategies.AlphaBetaStrategy(4));
            //gameManager.StartGame(new Strategies.AlphaBetaStrategy(3), new Strategies.MonteCarloStrategy(10000));
            //gameManager.StartGame(new Strategies.AlphaBetaStrategy(8), human);
            BoardClick += human.SetMove;
            //gameManager.StartGame(human, new Strategies.AlphaBetaStrategy(2));
            gameManager.StartGame(human, human);
            //gameManager.StartGame(new Strategies.MonteCarloStrategy(1000), human);
            gameManager.MoveDone += () => Invoke(new Action(Refresh));
        }

        private void GameCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine();
            //Console.WriteLine(gameManager.Board.PlayerMove);
            //gameManager.Board.DebugPrint();
            //gameManager.StartGame(null, null);
            //Board board = new Board();
            //Console.WriteLine(board.MakeMove(Players.First, 0 , 0));
            //Console.WriteLine(board.MakeMove(Players.Second, 1 , 0));
            //board.DebugPrint();
            //human.MakeMove(e.X / CellSize, e.Y / CellSize);
            int x = e.X / CellSize, y = e.Y / CellSize;
            if (0 <= x && x < UltimateTicTacToe.BoardSize && 0 <= y && y < UltimateTicTacToe.BoardSize)
                BoardClick?.Invoke(x, y);
        }
        private event Action<int, int> BoardClick;

        private readonly string[] sign = new string[3] { " ", "X", "O" };
        private readonly Font signFont = new Font("Arial", 20);
        private const int CellSize = 30;
        private readonly Pen thinLine = Pens.Black;
        private readonly Pen fatLine = new Pen(Brushes.Black, 3);
        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (gameManager.Board.IsFinished)
            {
                e.Graphics.DrawString(gameManager.Board.Winner.ToString(), DefaultFont, Brushes.Black, UltimateTicTacToe.BoardSize * CellSize + 10, 10);
            }
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    e.Graphics.DrawString(sign[(int)gameManager.Board[i, j]], signFont, i == gameManager.Board.LastAction.x && j == gameManager.Board.LastAction.y ? Brushes.Red : Brushes.Black, i * CellSize, j * CellSize);
            for (int i = 0; i <= UltimateTicTacToe.BoardSize; i++)
            {
                e.Graphics.DrawLine(i % 3 == 0 ? fatLine : thinLine, 0, i * CellSize, UltimateTicTacToe.BoardSize * CellSize, i * CellSize);
                e.Graphics.DrawLine(i % 3 == 0 ? fatLine : thinLine, i * CellSize, 0, i * CellSize, UltimateTicTacToe.BoardSize * CellSize);
            }
        }
    }
}
