using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    class GameManager
    {
        //public IStrategy FirstStrategy { get; private set; } = null;
        //public IStrategy SecondStrategy { get; private set; } = null;
        public Board Board { get; private set; } = null; // TODO: Thread Safe Access
        
        public Task StartGame(IStrategy firstPlayer, IStrategy secondPlayer/*, CancellationToken token*/)
        {
            //FirstStrategy = firstPlayer;
            //SecondStrategy = secondPlayer;
            Board = new Board();
            return Task.Run(() =>
            {
                firstPlayer.Init();
                secondPlayer.Init();
                IStrategy currentStrategy = firstPlayer, otherStrategy = secondPlayer, tmp;
                while (!Board.IsFinished)
                {
                    currentStrategy.MakeTurn();
                    tmp = currentStrategy;
                    currentStrategy = otherStrategy;
                    otherStrategy = tmp;
                }
            });
        }
    }
}
