﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    class GameManager
    {
        public UltimateTicTacToe Board { get; private set; } = null; // TODO: Thread Safe Access
        
        public Task StartGame(IStrategy firstPlayer, IStrategy secondPlayer/*, CancellationToken token*/)
        {
            Board = new UltimateTicTacToe();
            return Task.Run(() =>
            {
                firstPlayer.Init(new BoardProxy(Board, Players.First));
                secondPlayer.Init(new BoardProxy(Board, Players.Second));
                IStrategy currentStrategy = firstPlayer, otherStrategy = secondPlayer, tmp;
                while (!Board.IsFinished)
                {
                    currentStrategy.MakeTurn(); // TODO: Check is moved
                    tmp = currentStrategy;
                    currentStrategy = otherStrategy;
                    otherStrategy = tmp;
                }
            });
        }
    }
}
