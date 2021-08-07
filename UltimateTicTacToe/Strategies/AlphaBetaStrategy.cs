using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class AlphaBetaStrategy : IStrategy
    {
        protected struct StrategyMove
        {
            public int x, y, score;
            public StrategyMove(int score)
            {
                this.score = score;
                x = y = -1;
            }
        }

        protected readonly int maxDepth;
        public AlphaBetaStrategy(int maxDepth = 5)
        {
            this.maxDepth = maxDepth;
        }

        private static readonly int[] lineScore = new int[64];
        static AlphaBetaStrategy()
        {
            int[] scorePerCell = new int[] { 0, 1, 10, 100 };
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        int cur = (i << 4) + (j << 2) + k;
                        lineScore[cur] = scorePerCell[i + j + k];
                        lineScore[cur << 1] = -scorePerCell[i + j + k];
                    }
        }
        protected virtual int GetBoardScore(Board board, Players player)
        {
            int score = 0;
            int line;
            // Vertical
            for (int i = 0; i < Board.LocalBoardSize; i++)
            {
                line = 0;
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    line = (line << 2) + (int)board.GetOwner(i, j);
                score += lineScore[line];
            }
            // Horizontal
            for (int j = 0; j < Board.LocalBoardSize; j++)
            {
                line = 0;
                for (int i = 0; i < Board.LocalBoardSize; i++)
                    line = (line << 2) + (int)board.GetOwner(i, j);
                score += lineScore[line];
            }
            // Main Diagonal
            line = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                line = (line << 2) + (int)board.GetOwner(i, i);
            score += lineScore[line];
            // Side Diagonal
            line = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                line = (line << 2) + (int)board.GetOwner(Board.LocalBoardSize - 1 - i, i);
            score += lineScore[line];
            if (player == Players.Second)
                score *= -1;
            return score;
        }
        protected virtual int GetScore(UltimateTicTacToe board) // TODO: Extract heuristics
        {
            if (board.Winner == board.PlayerMove)
                return 100000;
            if (board.Winner == board.PlayerMove.GetOpponent())
                return -100000;
            Players player = board.PlayerMove;
            int score = GetBoardScore(board, player) * 100;
            for (int x = 0; x < UltimateTicTacToe.LocalBoardCount; x++)
                for (int y = 0; y < UltimateTicTacToe.LocalBoardCount; y++)
                {
                    TicTacToe smallBoard = board.GetBoard(x, y);
                    if (smallBoard.Winner == Players.None)
                        score += GetBoardScore(smallBoard, player);
                }
            return score;
        }
        // alpha shouldn't be int.MinValue, because int.MinValue == -int.MinValue
        private StrategyMove AlphaBeta(UltimateTicTacToe board, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue)
        {
            if (depth == 0 || board.IsFinished)
                return new StrategyMove(GetScore(board));
            PlayerMove[] moves = board.GetAllMoves(); // TODO: Sort by score
            moves.Shuffle();
            StrategyMove bestMove = new StrategyMove(-int.MaxValue), curMove;
            foreach (PlayerMove move in moves)
            {
                board.MakeMove(move.x, move.y); // TODO: Check is moved
                curMove.x = move.x;
                curMove.y = move.y;
                curMove.score = -AlphaBeta(board, depth - 1, -beta, -alpha).score;
                board.Undo();
                if (bestMove.score < curMove.score)
                    bestMove = curMove;
                if (beta <= curMove.score)
                    break;
                alpha = Math.Max(alpha, curMove.score);
            }
            return bestMove;
        }
        public void MakeMove(BoardProxy board)
        {
            StrategyMove move = AlphaBeta(board.GetBoardCopy(), maxDepth);
            Console.WriteLine("AlphaBetaDebug: score = {0}, at [{1}, {2}]", move.score, move.x, move.y);
            board.MakeMove(move.x, move.y);
        }
    }
}
