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
            public int score, total;
            public readonly TreeNode[] children;
            public TreeNode(int size)
            {
                score = total = 0;
                children = new TreeNode[size];
            }
            public double GetScore(int parentTotal)
            {
                return score / (double)total + Math.Sqrt(2 * Math.Log(parentTotal) / total);
            }
            public int Playout(UltimateTicTacToe board)
            {
                Players player = board.PlayerMove;
                while (!board.IsFinished)
                {
                    PlayerMove move = board.GetAllMoves().RandomElement();
                    board.MakeMove(board.PlayerMove, move.x, move.y);
                }
                int result = board.Winner == Players.Draw ? 0 : board.Winner == player ? -1 : 1;
                score += result;
                total++;
                return result;
            }
            public int Selection(UltimateTicTacToe board)
            {
                if (board.IsFinished)
                    return score;
                // If has null leaf then Expansion
                //children.Find
                PlayerMove[] moves = board.GetAllMoves();
                int[] nullNodes = children.Select((value, index) => value == null ? index : -1)
                                          .Where(index => index != -1).ToArray();
                //List<int> nullNodes = new List<int>();
                int result;
                if (nullNodes.Length > 0)
                {
                    int index = nullNodes.RandomElement();
                    board.MakeMove(board.PlayerMove, moves[index].x, moves[index].y);
                    children[index] = new TreeNode(board.GetAllMoves().Length);
                    result = -children[index].Playout(board);
                }
                else
                {
                    // Else Selection
                    double bestScore = double.NegativeInfinity;
                    int bestMove = -1;
                    for (int i = 0; i < children.Length; i++)
                    //foreach (TreeNode child in children)
                    {
                        double score = children[i].GetScore(total);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = i;
                        }
                    }
                    board.MakeMove(board.PlayerMove, moves[bestMove].x, moves[bestMove].y);
                    result = -children[bestMove].Selection(board);
                }
                score += result;
                total++;
                return result;
            }
        }
        protected static readonly Random random = new Random();
        protected readonly int maxAttempts;
        public MonteCarloStrategy(int maxAttempts = 100)
        {
            this.maxAttempts = maxAttempts;
        }
        TreeNode root = null;
        int[] moves = new int[0];
        protected int MonteCarlo(UltimateTicTacToe board, int attempts)
        {
            if (root != null) // TODO: Fix reuse
            {
                bool found = false;
                for (int i = 0; i < moves.Length; i++)
                    if (moves[i] == board.LastMove)
                    {
                        root = root.children[i];
                        found = true;
                        break;
                    }
                if (!found)
                    root = null;
            }
            moves = board.GetAllMovesId();
            if (root == null)
                root = new TreeNode(moves.Length);

            for (int i = 0; i < attempts; i++)
                root.Selection((UltimateTicTacToe)board.Clone());
            int bestMove = -1;
            double bestScore = double.NegativeInfinity;
            for (int i = 0; i < moves.Length; i++)
            {
                double score = root.children[i]?.GetScore(root.total) ?? double.NaN;
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
            //Console.WriteLine("MonteCarloDebug: score = {0}/{1}, at [{2}, {3}]", -root.score, root.total, moves[bestMove].x, moves[bestMove].y);
            root = root.children[bestMove];
            int move = moves[bestMove];
            board.MakeMove(move);
            moves = board.GetAllMovesId();
            return move;
        }
        public void MakeMove(BoardProxy board)
        {

            int move = MonteCarlo(board.GetBoardCopy(), maxAttempts);
            board.MakeMove(move);
        }
    }
}
