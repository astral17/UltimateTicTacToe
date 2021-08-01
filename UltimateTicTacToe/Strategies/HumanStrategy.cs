using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class HumanStrategy : IStrategy
    {
        private readonly object locker = new object();
        private PlayerMove move = new PlayerMove();
        private bool isMoved = false;
        public void SetMove(int x, int y)
        {
            lock (locker)
            {
                isMoved = true;
                move.x = x;
                move.y = y;
                Monitor.Pulse(locker);
            }
        }
        public void MakeMove(BoardProxy board)
        {
            PlayerMove move;
            do
            {
                lock (locker)
                {
                    while (!isMoved)
                        Monitor.Wait(locker);
                    isMoved = false;
                    move = this.move;
                }
            } while (!board.MakeMove(move.x, move.y));
        }
    }
}
