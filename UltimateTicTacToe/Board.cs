using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    enum Players
    {
        None = 0,
        First = 1,
        Second = 2,
    }
    enum CellTypes // TODO: Mb Merge with Players
    {
        Empty = 0,
        First = 1,
        Second = 2,
    }
    enum GameResults // TODO: WinLine
    {
        None = 0,
        FirstWin = 1,
        SecondWin = 2,
        Draw = 3,
    }
    struct ActiveBoard
    {
        public int x, y;
        public bool all;
    }
    //interface ICell
    //{
    //    Players Owner { get; set; }
    //}
    //struct Cell : ICell
    //{
    //    public Players Owner { get; set; }
    //}
    abstract class AbstractBoard // TODO: Rename
    {
        public const int LocalBoardSize = 3;
        public GameResults Winner { get; set; }
        public abstract CellTypes this[int x, int y] { get; set; }
        protected GameResults GetResult() // TODO: Rename
        {
            if (Winner != GameResults.None)
                return Winner;
            CellTypes result;
            // Vertical
            for (int i = 0; i < LocalBoardSize; i++)
            {
                result = this[i, 0];
                for (int j = 1; j < LocalBoardSize; j++)
                    if (this[i, j] != result)
                        result = CellTypes.Empty;
                if (result != CellTypes.Empty)
                    return (GameResults)(int)result;
            }
            // Horizontal
            for (int j = 0; j < LocalBoardSize; j++)
            {
                result = this[0, j];
                for (int i = 1; i < LocalBoardSize; i++)
                    if (this[i, j] != result)
                        result = CellTypes.Empty;
                if (result != CellTypes.Empty)
                    return (GameResults)(int)result;
            }
            // Diagonal
            result = this[0, 0];
            for (int i = 1; i < LocalBoardSize; i++)
                if (this[i, i] != result)
                    result = CellTypes.Empty;
            if (result != CellTypes.Empty)
                return (GameResults)(int)result;
            result = this[LocalBoardSize - 1, 0];
            for (int i = 1; i < LocalBoardSize; i++)
                if (this[LocalBoardSize - 1 - i, i] != result)
                    result = CellTypes.Empty;
            if (result != CellTypes.Empty)
                return (GameResults)(int)result;
            // Draw Check
            for (int j = 0; j < LocalBoardSize; j++)
                for (int i = 0; i < LocalBoardSize; i++)
                    if (this[i, j] == CellTypes.Empty)
                        return GameResults.None;
            return GameResults.Draw;
        }
    }
    class MiniBoard : AbstractBoard // TODO: Rename
    {
        private readonly CellTypes[,] board = new CellTypes[LocalBoardSize, LocalBoardSize];
        public override CellTypes this[int x, int y] { get => board[x, y]; set => board[x, y] = value; }
        public bool MakeMove(Players player, int x, int y)
        {
            if (Winner != GameResults.None || board[x, y] != CellTypes.Empty)
                return false;
            board[x, y] = (CellTypes)(int)player;
            Winner = GetResult();
            return true;
        }

    }
    class Board : AbstractBoard
    {
        public const int BoardSize = LocalBoardCount * LocalBoardSize;
        public const int LocalBoardCount = LocalBoardSize;

        public readonly MiniBoard[,] boards = new MiniBoard[LocalBoardSize, LocalBoardSize];
        public override CellTypes this[int x, int y]
        {
            get => (CellTypes)(int)boards[x, y].Winner;
            set => throw new NotImplementedException();
        }
        public Board()
        {
            for (int i = 0; i < LocalBoardSize; i++)
                for (int j = 0; j < LocalBoardSize; j++)
                    boards[i, j] = new MiniBoard();
        }
        public ActiveBoard ActiveBoard { get; private set; } = new ActiveBoard { all = true };
        public bool MakeMove(Players player, int x, int y)
        {
            if (player != PlayerMove)
                return false;
            // Move in right board
            if (!ActiveBoard.all && (x / LocalBoardSize != ActiveBoard.x || y / LocalBoardSize != ActiveBoard.y))
                return false;
            // Is possible move to cell
            if (!boards[x / LocalBoardSize, y / LocalBoardSize].MakeMove(player, x % LocalBoardSize, y % LocalBoardSize))
                return false;

            ActiveBoard = new ActiveBoard
            {
                x = x % LocalBoardSize,
                y = y % LocalBoardSize,
                all = boards[x % LocalBoardSize, y % LocalBoardSize].Winner != GameResults.None // TODO: Rule Set
            };
            LastAction = new StrategyAction(x, y);
            //{
            //    action = Actions.Move,
            //    x = x,
            //    y = y,
            //};
            PlayerMove = (Players)(3 - (int)PlayerMove);
            return true;
        }
        [Obsolete("Debug Only")]
        public void DebugPrint()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                    Console.Write("{0} ", (int)boards[i / LocalBoardSize, j / LocalBoardSize][i % LocalBoardSize, j % LocalBoardSize]);
                Console.WriteLine();
            }
        }
        //public bool Undo()
        //{
        //    throw new NotImplementedException();
        //}
        public StrategyAction LastAction { get; private set; } = new StrategyAction(Actions.None);
        public Players PlayerMove { get; private set; } = Players.First;
        public bool IsFinished { get; private set; } = false;
    }
}
