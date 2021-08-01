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
        protected virtual int GetBoardScore(Board board, Players player)
        {
            int score = 0;
            int count;
            for (int i = 0; i < Board.LocalBoardSize; i++)
            {
                count = 0;
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    if (board.GetOwner(i, j) != Players.None)
                        count += board.GetOwner(i, j) == player ? 1 : -1;
                if (Math.Abs(count) == 2)
                    score += Math.Sign(count);
            }
            // Horizontal
            for (int j = 0; j < Board.LocalBoardSize; j++)
            {
                count = 0;
                for (int i = 0; i < Board.LocalBoardSize; i++)
                    if (board.GetOwner(i, j) != Players.None)
                        count += board.GetOwner(i, j) == player ? 1 : -1;
                if (Math.Abs(count) == 2)
                    score += Math.Sign(count);
            }
            // Main Diagonal
            count = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                if (board.GetOwner(i, i) != Players.None)
                    count += board.GetOwner(i, i) == player ? 1 : -1;
            if (Math.Abs(count) == 2)
                score += Math.Sign(count);
            // Side Diagonal
            count = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                if (board.GetOwner(Board.LocalBoardSize - 1 - i, i) != Players.None)
                    count += board.GetOwner(Board.LocalBoardSize - 1 - i, i) == player ? 1 : -1;
            if (Math.Abs(count) == 2)
                score += Math.Sign(count);
            return score;
        }
        protected virtual int GetScore(UltimateTicTacToe board) // TODO: Extract heuristics
        {
            if (board.Winner == board.PlayerMove)
                return 1000;
            if (board.Winner == board.PlayerMove.GetOpponent())
                return -1000;
            Players player = board.PlayerMove;
            int score = GetBoardScore(board, player) * 10;
            for (int x = 0; x < UltimateTicTacToe.LocalBoardCount; x++)
                for (int y = 0; y < UltimateTicTacToe.LocalBoardCount; y++)
                {
                    TicTacToe smallBoard = board.GetBoard(x, y);
                    if (smallBoard.Winner == Players.None)
                        score += GetBoardScore(smallBoard, player);
                    else if (smallBoard.Winner != Players.Draw)
                        score += smallBoard.Winner == board.PlayerMove ? 10 : -10;
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
