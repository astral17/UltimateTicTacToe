using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    public class GameManager
    {
        public UltimateTicTacToe Board { get; private set; } = null;
        
        public Task StartGame(IStrategy firstPlayer, IStrategy secondPlayer/*, CancellationToken token*/)
        {
            Board = new UltimateTicTacToe();
            return Task.Run(() =>
            {
                IStrategy currentStrategy = firstPlayer, otherStrategy = secondPlayer;
                BoardProxy currentProxy = new BoardProxy(Board, Players.First), otherProxy = new BoardProxy(Board, Players.Second);
                while (!Board.IsFinished)
                {
                    Players lastMove = Board.PlayerMove;
                    currentStrategy.MakeMove(currentProxy);
                    if (lastMove == Board.PlayerMove)
                        throw new Exception("Strategy make incorrect move");
                    Utils.Swap(ref currentStrategy, ref otherStrategy);
                    Utils.Swap(ref currentProxy, ref otherProxy);
                    MoveDone?.Invoke();
                }
                Finished?.Invoke();
            });
        }
        public event Action MoveDone;
        public event Action Finished;
    }
}
