using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    class RandomStrategy : IStrategy
    {
        static readonly Random random = new Random();
        BoardProxy board;
        public void Init(BoardProxy board)
        {
            this.board = board;
        }

        public void MakeTurn()
        {
            PlayerMove[] moves = board.GetAllMoves();
            PlayerMove move = moves[random.Next(moves.Length)];
            board.MakeMove(move.x, move.y);
        }
    }
}
