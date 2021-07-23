using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    public enum Actions
    {
        None,
        Move,
        Undo,
        OfferDraw,
        Surrender,
    }
    public struct StrategyAction
    {
        public Actions action;
        //public Players player;
        public int x, y;
        public StrategyAction(Actions action)
        {
            this.action = action;
            x = y = 0;
        }
        public StrategyAction(int x, int y)
        {
            this.action = Actions.Move;
            this.x = x;
            this.y = y;
        }
    }
    public interface IStrategy
    {
        void Init(BoardProxy board);
        void MakeTurn();
    }
}
