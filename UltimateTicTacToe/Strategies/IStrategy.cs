using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    public interface IStrategy
    {
        void MakeMove(BoardProxy board);
    }
}
