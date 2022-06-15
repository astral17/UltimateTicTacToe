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
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                Started?.Invoke(Board);
                while (!Board.IsFinished)
                {
                    Players lastMove = Board.PlayerMove;
                    sw.Restart();
                    currentStrategy.MakeMove(currentProxy);
                    Console.WriteLine("Elapsed: {0}", sw.ElapsedMilliseconds);
                    if (lastMove == Board.PlayerMove)
                        throw new Exception("Strategy make incorrect move");
                    Utils.Swap(ref currentStrategy, ref otherStrategy);
                    Utils.Swap(ref currentProxy, ref otherProxy);
                    MoveDone?.Invoke(Board, "Elapsed: " + sw.ElapsedMilliseconds);
                }
                Finished?.Invoke(Board);
            });
        }
        public delegate void BoardEvent(UltimateTicTacToe board, string info = null);
        public event BoardEvent Started;
        public event BoardEvent MoveDone;
        public event BoardEvent Finished;
    }
}
