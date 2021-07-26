using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    class HumanStrategy : IStrategy
    {
        public BoardProxy board;
        private readonly object locker = new object();
        private PlayerMove move = new PlayerMove();
        private bool isMoved = false;
        public void Init(BoardProxy board)
        {
            this.board = board;
        }
        public void MakeMove(int x, int y)
        {
            lock (locker)
            {
                isMoved = true;
                move.x = x;
                move.y = y;
                Monitor.Pulse(locker);
            }
        }
        public void MakeTurn()
        {
            PlayerMove move;
            lock (locker)
            {
                while (!isMoved)
                    Monitor.Wait(locker);
                isMoved = false;
                move = this.move;
            }
            board.MakeMove(move.x, move.y);
        }
    }
}
