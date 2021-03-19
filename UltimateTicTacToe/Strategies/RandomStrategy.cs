using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    class RandomStrategy : IStrategy
    {
        static Random random = new Random();
        PlayerBoard board;
        public void Init(PlayerBoard board)
        {
            this.board = board;
        }

        public void MakeTurn()
        {
            PlayerMove[] moves = board.GetAllMoves();
            PlayerMove move = moves[random.Next(moves.Length)];
            //PlayerMove move = moves[0];

            board.MakeMove(move.x, move.y);
            //return new StrategyAction(move.X, move.Y);
        }
    }
}
