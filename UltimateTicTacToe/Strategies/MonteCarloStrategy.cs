using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class MonteCarloStrategy : IStrategy
    {
        protected class TreeNode
        {
            public int win, total;
            public readonly TreeNode[] children;
            public TreeNode(int size)
            {
                win = total = 0;
                children = new TreeNode[size];
            }
            public double GetScore()
            {
                return win / (double)total;
            }
            private const int MaxPlayoutScore = 2;
            public int Playout(UltimateTicTacToe board)
            {
                Players player = board.PlayerMove;
                while (!board.IsFinished)
                {
                    // Mb use RandomStrategy?
                    PlayerMove[] moves = board.GetAllMoves();
                    PlayerMove move = moves[random.Next(moves.Length)];
                    board.MakeMove(board.PlayerMove, move.x, move.y);
                }
                int result = board.Winner == Players.Draw ? 1 : board.Winner == player ? 2 : 0;
                win += result;
                total += MaxPlayoutScore;
                return result;
            }
            public int Selection(UltimateTicTacToe board)
            {
                if (board.IsFinished)
                    return win;
                // If has null leaf then Expansion
                //children.Find
                PlayerMove[] moves = board.GetAllMoves();
                int[] nullNodes = children.Select((value, index) => value == null ? index : -1)
                                          .Where(index => index != -1).ToArray();
                //List<int> nullNodes = new List<int>();
                int result;
                if (nullNodes.Length > 0)
                {
                    int index = nullNodes[random.Next(nullNodes.Length)];
                    board.MakeMove(board.PlayerMove, moves[index].x, moves[index].y);
                    children[index] = new TreeNode(board.GetAllMoves().Length);
                    result = MaxPlayoutScore - children[index].Playout(board);
                }
                else
                {
                    // Else Selection
                    double bestScore = double.PositiveInfinity;
                    int bestMove = -1;
                    for (int i = 0; i < children.Length; i++)
                    //foreach (TreeNode child in children)
                    {
                        double score = children[i].GetScore();
                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestMove = i;
                        }
                    }
                    board.MakeMove(board.PlayerMove, moves[bestMove].x, moves[bestMove].y);
                    result = MaxPlayoutScore - children[bestMove].Selection(board);
                }
                win += result;
                total += MaxPlayoutScore;
                return result;
            }
        }
        protected static readonly Random random = new Random();
        protected readonly int maxAttempts;
        public MonteCarloStrategy(int maxAttempts = 100)
        {
            this.maxAttempts = maxAttempts;
        }
        protected PlayerMove MonteCarlo(UltimateTicTacToe board, int attempts)
        {
            PlayerMove[] moves = board.GetAllMoves();
            TreeNode root = new TreeNode(moves.Length);
            for (int i = 0; i < attempts; i++)
                root.Selection((UltimateTicTacToe)board.Clone());
            int bestMove = -1;
            double bestScore = double.PositiveInfinity;
            for (int i = 0; i < moves.Length; i++)
            {
                double score = root.children[i]?.GetScore() ?? double.NaN;
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
            return moves[bestMove];
        }
        public void MakeMove(BoardProxy board)
        {
            PlayerMove move = MonteCarlo(board.GetBoardCopy(), maxAttempts);
            //Console.WriteLine("MonteCarloDebug: score = {0}, at [{1}, {2}]", move.score, move.x, move.y);
            board.MakeMove(move.x, move.y);
        }
    }
}
