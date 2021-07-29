using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    public enum Players // TODO: Rename
    {
        None = 0,
        First = 1,
        Second = 2,
        Draw = 3,
    }
    public static class PlayersExtensions
    {
        public static Players GetOpponent(this Players player) // TODO: None and Draw is undefined behaviour
        {
            return (Players)(3 - (int)player);
        }
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
        public Players Winner { get; set; }
        public abstract Players this[int x, int y] { get; set; }
        public abstract Players GetOwner(int x, int y); // TODO: Rename
        //public abstract bool IsAvailable(Players player, int x, int y);
        public abstract bool MakeMove(Players player, int x, int y);
        public abstract PlayerMove[] GetAllMoves();
        protected Players GetResult() // TODO: Rename
        {
            if (Winner != Players.None) // TODO: Remake cache
                return Winner;
            Players result;
            // Vertical
            for (int i = 0; i < LocalBoardSize; i++)
            {
                result = GetOwner(i, 0);
                for (int j = 1; j < LocalBoardSize; j++)
                    if (GetOwner(i, j) != result)
                        result = Players.None;
                if (result != Players.None)
                    return result;
            }
            // Horizontal
            for (int j = 0; j < LocalBoardSize; j++)
            {
                result = GetOwner(0, j);
                for (int i = 1; i < LocalBoardSize; i++)
                    if (GetOwner(i, j) != result)
                        result = Players.None;
                if (result != Players.None)
                    return result;
            }
            // Main Diagonal
            result = GetOwner(0, 0);
            for (int i = 1; i < LocalBoardSize; i++)
                if (GetOwner(i, i) != result)
                    result = Players.None;
            if (result != Players.None)
                return result;
            // Side Diagonal
            result = GetOwner(LocalBoardSize - 1, 0);
            for (int i = 1; i < LocalBoardSize; i++)
                if (GetOwner(LocalBoardSize - 1 - i, i) != result)
                    result = Players.None;
            if (result != Players.None)
                return result;
            // Draw Check
            for (int j = 0; j < LocalBoardSize; j++)
                for (int i = 0; i < LocalBoardSize; i++)
                    if (GetOwner(i, j) == Players.None)
                        return Players.None;
            return Players.Draw;
        }
        public abstract object Clone();
        public bool IsFinished => Winner != Players.None;
    }
    public class TicTacToe : Board
    {
        private Players[,] board = new Players[LocalBoardSize, LocalBoardSize];
        public override Players this[int x, int y]
        { 
            get => board[x, y];
            set // Only outer usage
            {
                board[x, y] = value;
                Winner = Players.None;
                Winner = GetResult();
            }
        }
        public override Players GetOwner(int x, int y) => board[x, y];
        public override PlayerMove[] GetAllMoves()
        {
            List<PlayerMove> result = new List<PlayerMove>();
            if (!IsFinished)
                for (int i = 0; i< LocalBoardSize; i++)
                    for (int j = 0; j< LocalBoardSize; j++)
                        if (this[i, j] == Players.None)
                            result.Add(new PlayerMove(i, j));
            return result.ToArray();
        }
        public override bool MakeMove(Players player, int x, int y)
        {
            if (Winner != Players.None || board[x, y] != Players.None)
                return false;
            board[x, y] = player;
            Winner = GetResult();
            return true;
        }

        public override object Clone()
        {
            TicTacToe result = (TicTacToe)MemberwiseClone();
            result.board = (Players[,])board.Clone();
            return result;
        }
    }
    public class UltimateTicTacToe : Board
    {
        public const int BoardSize = LocalBoardCount * LocalBoardSize;
        public const int LocalBoardCount = LocalBoardSize;

        private TicTacToe[,] boards = new TicTacToe[LocalBoardCount, LocalBoardCount];
        public Players PlayerMove { get; protected set; } = Players.First; // Move to UltimateTicTacToe
        public override Players this[int x, int y]
        {
            get => boards[x / LocalBoardSize, y / LocalBoardSize][x % LocalBoardSize, y % LocalBoardSize];
            set // Only outer usage
            {
                boards[x / LocalBoardSize, y / LocalBoardSize][x % LocalBoardSize, y % LocalBoardSize] = value;
                Winner = Players.None;
                Winner = GetResult();
            }
        }
        public override Players GetOwner(int x, int y) => boards[x, y].Winner;
        public TicTacToe GetBoard(int x, int y) => boards[x, y];
        public UltimateTicTacToe()
        {
            for (int i = 0; i < LocalBoardCount; i++)
                for (int j = 0; j < LocalBoardCount; j++)
                    boards[i, j] = new TicTacToe();
        }
        public ActiveBoard ActiveBoard { get; private set; } = new ActiveBoard { all = true };
        public override bool MakeMove(Players player, int x, int y)
        {
            if (Winner != Players.None)
                return false;
            if (player != PlayerMove)
                return false;
            // Move in available board
            if (!ActiveBoard.all && (x / LocalBoardSize != ActiveBoard.x || y / LocalBoardSize != ActiveBoard.y))
                return false;
            // Is possible move to cell
            if (!boards[x / LocalBoardSize, y / LocalBoardSize].MakeMove(player, x % LocalBoardSize, y % LocalBoardSize))
                return false;
            history.Push(new HistoryMove
            {
                x = x,
                y = y,
                all = ActiveBoard.all
            });

            ActiveBoard = new ActiveBoard
            {
                x = x % LocalBoardSize,
                y = y % LocalBoardSize,
                all = boards[x % LocalBoardSize, y % LocalBoardSize].Winner != Players.None // TODO: Rule Set
            };
            LastAction = new StrategyAction(x, y);
            PlayerMove = PlayerMove.GetOpponent();
            Winner = GetResult();
            return true;
        }
        public virtual bool MakeMove(int x, int y) => MakeMove(PlayerMove, x, y);
        private struct HistoryMove
        {
            public int x, y;
            public bool all;
        }
        private Stack<HistoryMove> history = new Stack<HistoryMove>();
        public void Undo()
        {
            HistoryMove move = history.Pop();
            boards[move.x / LocalBoardSize, move.y / LocalBoardSize][move.x % LocalBoardSize, move.y % LocalBoardSize] = Players.None;
            Winner = Players.None;
            if (history.Count > 0)
                LastAction = new StrategyAction(history.Peek().x, history.Peek().y);
            PlayerMove = PlayerMove.GetOpponent();
            ActiveBoard = new ActiveBoard
            {
                x = move.x / LocalBoardSize,
                y = move.y / LocalBoardSize,
                all = move.all
            };
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
                    Console.Write($"{(int)this[i, j], 2}");
                Console.WriteLine();
            }
        }

        [Obsolete("Debug Only")]
        public string DebugToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                    builder.AppendFormat("{0, 2}", (int)this[i, j]);
                builder.Append('\n');
            }
            return builder.ToString();
        }

        public override object Clone()
        {
            UltimateTicTacToe result = (UltimateTicTacToe)MemberwiseClone();
            result.history = new Stack<HistoryMove>(history); // TODO: Clone without history for speed
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
        public StrategyAction LastAction { get; private set; } = new StrategyAction(Actions.None); // TODO: Action history
    }

    public class BoardProxy
    {
        public const int LocalBoardSize = 3;
        public const int BoardSize = LocalBoardCount * LocalBoardSize; // TODO: Remove duplicate
        public const int LocalBoardCount = LocalBoardSize;

        readonly UltimateTicTacToe board;
        public PlayerMove[] GetAllMoves() => board.GetAllMoves();
        //public Players GetOwner(int x, int y) => board.GetOwner(x, y);
        public bool MakeMove(int x, int y) => board.MakeMove(Player, x, y); // TODO: throw error if try enemy move
        //public StrategyAction LastAction => board.LastAction;
        //public ActiveBoard ActiveBoard => board.ActiveBoard;
        public Players Player { get; }
        public BoardProxy(UltimateTicTacToe board, Players player)
        {
            this.board = board;
            Player = player;
        }
        public Players this[int x, int y] 
        { 
            get => board[x, y];
            //protected set => throw new NotImplementedException();
        }
        public UltimateTicTacToe GetBoardCopy()
        {
            return (UltimateTicTacToe)board.Clone();
        }
    }
}
