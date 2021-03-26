using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    public enum Players
    {
        None = 0,
        First = 1,
        Second = 2,
    }
    public enum CellOwners // TODO: Mb Merge with Players
    {
        None = 0,
        First = 1,
        Second = 2,
    }
    public enum GameResults // TODO: WinLine
    {
        None = 0,
        FirstWin = 1,
        SecondWin = 2,
        Draw = 3,
    }
    public struct ActiveBoard
    {
        public int x, y;
        public bool all;
    }
    public struct PlayerMove : IComparable<PlayerMove>
    {
        public int x, y;
        public PlayerMove(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public PlayerMove Add(int dx, int dy)
        {
            return new PlayerMove(x + dx, y + dy);
        }

        public int CompareTo(PlayerMove other)
        {
            if (x == other.x)
                return y.CompareTo(other.y);
            return x.CompareTo(other.x);
        }
    }
    public abstract class Board : ICloneable
    {
        public const int LocalBoardSize = 3;
        public GameResults Winner { get; set; }
        public abstract CellOwners this[int x, int y] { get; protected set; }
        public abstract CellOwners GetOwner(int x, int y);
        public abstract bool MakeMove(Players player, int x, int y);
        public virtual bool MakeMove(int x, int y) => MakeMove(PlayerMove, x, y);
        public abstract PlayerMove[] GetAllMoves();
        protected GameResults GetResult() // TODO: Rename
        {
            if (Winner != GameResults.None)
                return Winner;
            CellOwners result;
            // Vertical
            for (int i = 0; i < LocalBoardSize; i++)
            {
                result = GetOwner(i, 0);
                for (int j = 1; j < LocalBoardSize; j++)
                    if (GetOwner(i, j) != result)
                        result = CellOwners.None;
                if (result != CellOwners.None)
                    return (GameResults)(int)result;
            }
            // Horizontal
            for (int j = 0; j < LocalBoardSize; j++)
            {
                result = GetOwner(0, j);
                for (int i = 1; i < LocalBoardSize; i++)
                    if (GetOwner(i, j) != result)
                        result = CellOwners.None;
                if (result != CellOwners.None)
                    return (GameResults)(int)result;
            }
            // Diagonal
            result = GetOwner(0, 0);
            for (int i = 1; i < LocalBoardSize; i++)
                if (GetOwner(i, i) != result)
                    result = CellOwners.None;
            if (result != CellOwners.None)
                return (GameResults)(int)result;
            result = GetOwner(LocalBoardSize - 1, 0);
            for (int i = 1; i < LocalBoardSize; i++)
                if (GetOwner(LocalBoardSize - 1 - i, i) != result)
                    result = CellOwners.None;
            if (result != CellOwners.None)
                return (GameResults)(int)result;
            // Draw Check
            for (int j = 0; j < LocalBoardSize; j++)
                for (int i = 0; i < LocalBoardSize; i++)
                    if (GetOwner(i, j) == CellOwners.None)
                        return GameResults.None;
            return GameResults.Draw;
        }

        public abstract object Clone();// => MemberwiseClone();

        public Players PlayerMove { get; protected set; } = Players.First;
        public bool IsFinished => Winner != GameResults.None;
    }
    public class TicTacToe : Board
    {
        private CellOwners[,] board = new CellOwners[LocalBoardSize, LocalBoardSize];
        public override CellOwners this[int x, int y]
        { 
            get => board[x, y];
            protected set => board[x, y] = value;
        }
        public override CellOwners GetOwner(int x, int y) => board[x, y];
        public override PlayerMove[] GetAllMoves()
        {
            List<PlayerMove> result = new List<PlayerMove>();
            if (!IsFinished)
                for (int i = 0; i< LocalBoardSize; i++)
                    for (int j = 0; j< LocalBoardSize; j++)
                        if (this[i, j] == CellOwners.None)
                            result.Add(new PlayerMove(i, j));
            return result.ToArray();
        }
        public override bool MakeMove(Players player, int x, int y)
        {
            if (Winner != GameResults.None || board[x, y] != CellOwners.None)
                return false;
            board[x, y] = (CellOwners)(int)player;
            Winner = GetResult();
            return true;
        }

        public override object Clone()
        {
            TicTacToe result = (TicTacToe)MemberwiseClone();
            result.board = (CellOwners[,])board.Clone();
            return result;
        }
    }
    public class UltimateTicTacToe : Board
    {
        public const int BoardSize = LocalBoardCount * LocalBoardSize;
        public const int LocalBoardCount = LocalBoardSize;

        private TicTacToe[,] boards = new TicTacToe[LocalBoardCount, LocalBoardCount];
        public override CellOwners this[int x, int y]
        {
            get => boards[x / LocalBoardSize, y / LocalBoardSize][x % LocalBoardSize, y % LocalBoardSize];
            protected set => throw new NotImplementedException();
        }
        public override CellOwners GetOwner(int x, int y) => (CellOwners)((int)boards[x, y].Winner % 3); // TODO: Winner == Draw
        public UltimateTicTacToe()
        {
            for (int i = 0; i < LocalBoardCount; i++)
                for (int j = 0; j < LocalBoardCount; j++)
                    boards[i, j] = new TicTacToe();
        }
        public ActiveBoard ActiveBoard { get; private set; } = new ActiveBoard { all = true };
        public override bool MakeMove(Players player, int x, int y)
        {
            if (Winner != GameResults.None)
                return false;
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
            PlayerMove = (Players)(3 - (int)PlayerMove);
            Winner = GetResult();
            return true;
        }

        public override PlayerMove[] GetAllMoves() // TODO: Yield?
        {
            List<PlayerMove> result = new List<PlayerMove>();
            if (!IsFinished)
                if (ActiveBoard.all)
                    for (int i = 0; i < LocalBoardCount; i++)
                        for (int j = 0; j < LocalBoardCount; j++)
                            result.AddRange(Array.ConvertAll(boards[i, j].GetAllMoves(), p => p.Add(i * LocalBoardSize, j * LocalBoardSize)));
                else
                    result.AddRange(Array.ConvertAll(boards[ActiveBoard.x, ActiveBoard.y].GetAllMoves(), p => p.Add(ActiveBoard.x * LocalBoardSize, ActiveBoard.y * LocalBoardSize)));
            return result.ToArray();
        }
        [Obsolete("Debug Only")]
        public void DebugPrint()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                    Console.Write($"{(int)this[i, j]:2}");
                Console.WriteLine();
            }
        }

        public override object Clone()
        {
            UltimateTicTacToe result = (UltimateTicTacToe)MemberwiseClone();
            result.boards = new TicTacToe[LocalBoardCount, LocalBoardCount];
            for (int i = 0; i < LocalBoardCount; i++)
                for (int j = 0; j < LocalBoardCount; j++)
                    result.boards[i, j] = (TicTacToe)boards[i, j].Clone();
            return result;
        }

        //public bool Undo()
        //{
        //    throw new NotImplementedException();
        //}
        public StrategyAction LastAction { get; private set; } = new StrategyAction(Actions.None);
    }

    class BoardProxy : Board
    {
        //public const int LocalBoardSize = 3;
        public const int BoardSize = LocalBoardCount * LocalBoardSize; // TODO: Remove duplicate
        public const int LocalBoardCount = LocalBoardSize;

        readonly UltimateTicTacToe board;
        public override PlayerMove[] GetAllMoves() => board.GetAllMoves();
        public override CellOwners GetOwner(int x, int y) => board.GetOwner(x, y);
        public override bool MakeMove(Players player, int x, int y) => board.MakeMove(Player, x, y); // TODO: throw error if try enemy move
        public StrategyAction LastAction => board.LastAction;
        public ActiveBoard ActiveBoard => board.ActiveBoard;
        public Players Player { get; }
        public BoardProxy(UltimateTicTacToe board, Players player)
        {
            this.board = board;
            Player = player;
        }
        public override CellOwners this[int x, int y] 
        { 
            get => board[x, y];
            protected set => throw new NotImplementedException();
        }
        public Board GetBoardCopy()
        {
            return (Board)board.Clone();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
