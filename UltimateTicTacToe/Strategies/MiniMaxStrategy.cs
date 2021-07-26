using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class MiniMaxStrategy : IStrategy // Only for tests? AlphaBeta is MiniMax, but faster
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
        protected BoardProxy board;
        public MiniMaxStrategy(int maxDepth = 5)
        {
            this.maxDepth = maxDepth;
        }
        public void Init(BoardProxy board)
        {
            this.board = board;
        }
        protected int GetScore(Board board) // TODO: Extract heuristics
        {
            if (board.Winner == board.PlayerMove)
                return 100;
            if (board.Winner == board.PlayerMove.GetOpponent())
                return -100;
            int score = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    score += board.GetOwner(i, j) == board.PlayerMove ? 1 : board.GetOwner(i, j) == board.PlayerMove.GetOpponent() ? -1 : 0;
            return score;
        }
        private StrategyMove MiniMax(Board board, int depth) // NegaMax
        {
            if (depth == 0 || board.IsFinished)
                return new StrategyMove(GetScore(board));
            PlayerMove[] moves = board.GetAllMoves();
            StrategyMove bestMove = new StrategyMove(-int.MaxValue), curMove;
            foreach (PlayerMove move in moves)
            {
                Board curBoard = (Board)board.Clone(); // TODO: Undo moves
                curBoard.MakeMove(move.x, move.y); // TODO: Check is moved
                curMove.x = move.x;
                curMove.y = move.y;
                curMove.score = -MiniMax(curBoard, depth - 1).score;
                if (bestMove.score < curMove.score)
                    bestMove = curMove;
            }
            return bestMove;
        }
        public virtual void MakeTurn()
        {
            StrategyMove move = MiniMax(board.GetBoardCopy(), maxDepth);
            board.MakeMove(move.x, move.y);
        }
    }
}
