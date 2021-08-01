using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    class RandomStrategy : IStrategy
    {
        public void MakeMove(BoardProxy board)
        {
            PlayerMove move = board.GetAllMoves().RandomElement();
            board.MakeMove(move.x, move.y);
        }
    }
}
