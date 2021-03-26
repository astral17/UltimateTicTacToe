using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    class MiniMaxStrategy : IStrategy
    {
        private struct StrategyMove
        {
            public int x, y, score;
            public StrategyMove(int score)
            {
                this.score = score;
                x = y = 0;
            }
        }

        readonly int maxDepth;
        BoardProxy board;
        public MiniMaxStrategy(int maxDepth = 5)
        {
            this.maxDepth = maxDepth;
        }
        public void Init(BoardProxy board)
        {
            this.board = board;
        }
        private int GetScore(Board board) // TODO: Extract heuristics
        {
            if ((int)board.Winner == (int)board.PlayerMove)
                return 100;
            if ((int)board.Winner == 3 - (int)board.PlayerMove)
                return -100;
            int score = 0;
            for (int i = 0; i < Board.LocalBoardSize; i++)
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    score += (int)board.GetOwner(i, j) == (int)board.PlayerMove ? 1 : (int)board.GetOwner(i, j) == 3 - (int)board.PlayerMove ? -1 : 0;
            return score;
        }
        private StrategyMove MiniMax(Board board, int depth)
        {
            if (board.IsFinished || depth == 0)
                return new StrategyMove(GetScore(board));
            PlayerMove[] moves = board.GetAllMoves();
            StrategyMove bestMove = new StrategyMove(int.MaxValue);
            foreach (PlayerMove move in moves)
            {
                Board curBoard = (Board)board.Clone(); // TODO: Undo moves
                curBoard.MakeMove(move.x, move.y);
                StrategyMove curMove = MiniMax(curBoard, depth - 1);
                curMove.x = move.x;
                curMove.y = move.y;
                if (curMove.score < bestMove.score)
                    bestMove = curMove;
            }
            return bestMove;
        }
        public void MakeTurn()
        {
            StrategyMove move = MiniMax(board.GetBoardCopy(), maxDepth);
            board.MakeMove(move.x, move.y);
        }
    }
}
