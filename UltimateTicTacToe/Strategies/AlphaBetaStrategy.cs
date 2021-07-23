using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class AlphaBetaStrategy : MiniMaxStrategy
    {
        public AlphaBetaStrategy(int depth = 5) : base(depth) {}
        // alpha shouldn't be int.MinValue, because int.MinValue == -int.MinValue
        private StrategyMove AlphaBeta(Board board, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue)
        {
            if (depth == 0 || board.IsFinished)
                return new StrategyMove(GetScore(board));
            PlayerMove[] moves = board.GetAllMoves(); // TODO: Sort by score
            StrategyMove bestMove = new StrategyMove(-int.MaxValue), curMove;
            foreach (PlayerMove move in moves)
            {
                Board curBoard = (Board)board.Clone(); // TODO: Undo moves
                curBoard.MakeMove(move.x, move.y); // TODO: Check is moved
                curMove.x = move.x;
                curMove.y = move.y;
                curMove.score = -AlphaBeta(curBoard, depth - 1, -beta, -alpha).score;
                if (bestMove.score < curMove.score)
                    bestMove = curMove;
                if (beta <= curMove.score)
                    break;
                alpha = Math.Max(alpha, curMove.score);
            }
            return bestMove;
        }
        public override void MakeTurn()
        {
            StrategyMove move = AlphaBeta(board.GetBoardCopy(), maxDepth);
            board.MakeMove(move.x, move.y);
        }
    }
}
