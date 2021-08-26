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
        public int id;
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
    
    public class UltimateTicTacToe : ICloneable
    {
        public const int LocalBoardSize = 3;
        public const int BoardSize = LocalBoardCount * LocalBoardSize;
        public const int LocalBoardCount = LocalBoardSize;

        private Players[] board = new Players[BoardSize * BoardSize];
        private Players[] winners = new Players[LocalBoardSize * LocalBoardSize];
        public Players PlayerMove { get; protected set; } = Players.First;
        public Players Winner { get; set; }
        public bool IsFinished => Winner != Players.None;
        public Players this[int x, int y]
        {
            get
            {
                return board[ConvertPlayerMoveToId(x, y)];
            }
            set // Only outer usage
            {
                board[ConvertPlayerMoveToId(x, y)] = value;
                Winner = Players.None;
                Winner = GetResult(winners);
            }
        }

        private static readonly int[] lineScore = new int[64];
        static UltimateTicTacToe()
        {
            int[] scorePerCell = new int[] { 0, 1, 10, 100 };
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        int cur = (i << 4) + (j << 2) + k;
                        lineScore[cur] = scorePerCell[i + j + k];
                        lineScore[cur << 1] = -scorePerCell[i + j + k];
                    }
        }
        private const int lineFirst  = 0b010101;
        private const int lineSecond = 0b101010;
        protected static Players GetResult(Players[] board)
        {
            int line;
            // Vertical
            for (int i = 0; i < 3; i++)
            {
                line = 0;
                for (int j = i; j < 9; j += 3)
                    line = (line << 2) + (int)board[j];
                if (line == lineFirst)
                    return Players.First;
                if (line == lineSecond)
                    return Players.Second;
            }
            // Horizontal
            for (int i = 0; i < 9; i += 3)
            {
                line = 0;
                for (int j = 0; j < 3; j++)
                    line = (line << 2) + (int)board[i + j];
                if (line == lineFirst)
                    return Players.First;
                if (line == lineSecond)
                    return Players.Second;
            }
            // Main Diagonal
            line = 0;
            for (int i = 0; i < 9; i += 4)
                line = (line << 2) + (int)board[i];
            if (line == lineFirst)
                return Players.First;
            if (line == lineSecond)
                return Players.Second;
            // Side Diagonal
            line = 0;
            for (int i = 2; i < 8; i += 2)
                line = (line << 2) + (int)board[i];
            if (line == lineFirst)
                return Players.First;
            if (line == lineSecond)
                return Players.Second;
            // Draw Check
            for (int i = 0; i < 9; i++)
                if (board[i] == Players.None)
                    return Players.None;
            return Players.Draw;
        }


        public Players GetOwner(int x, int y) => winners[x * 3 + y];
        public Players[] GetRawBoard(int id)
        {
            return board.SubArray(id * 9, 9);
        }
        public Players[] GetRawBoard(int x, int y)
        {
            return GetRawBoard(ConvertPlayerMoveToId(x, y) / 9);
        }
        public Players[] GetRawWinners() // TODO: Property?, Copy?
        {
            return winners;
        }
        // bidirectional convert
        private static void CoordConvert(int a, int b, out int c, out int d)
        {
            c = (a / 3) * 3 + b / 3;
            d = (a % 3) * 3 + b % 3;
        }
        public static PlayerMove ConvertIdToPlayerMove(int id)
        {
            CoordConvert(id / 9, id % 9, out int x, out int y);
            return new PlayerMove(x, y);
        }
        public static int ConvertPlayerMoveToId(int x, int y)
        {
            CoordConvert(x, y, out int a, out int b);
            return a * 9 + b;
        }
        public static int ConvertPlayerMoveToId(PlayerMove move)
        {
            return ConvertPlayerMoveToId(move.x, move.y);
        }
        public UltimateTicTacToe()
        {
            //for (int i = 0; i < LocalBoardCount; i++)
            //    for (int j = 0; j < LocalBoardCount; j++)
            //        boards[i, j] = new TicTacToe();
        }
        public ActiveBoard ActiveBoard { get; private set; } = new ActiveBoard { all = true };
        public bool MakeMove(int id)
        {
            if (Winner != Players.None)
                return false;
            // Move in available board
            int bid = id / 9;
            if (!ActiveBoard.all && bid != ActiveBoard.id)
                return false;
            // Is possible move to cell
            if (winners[bid] != Players.None || board[id] != Players.None)
                return false;
            board[id] = PlayerMove;
            winners[bid] = GetResult(board.SubArray(bid * 9, 9));
            history.Push(new HistoryMove
            {
                id = id,
                all = ActiveBoard.all
            });
            int sid = id % 9;
            ActiveBoard = new ActiveBoard
            {
                id = sid,
                all = winners[sid] != Players.None // TODO: Rule Set
            };
            LastMove = id;
            PlayerMove = PlayerMove.GetOpponent();
            Winner = GetResult(winners);
            return true;
        }
        public bool MakeMove(Players player, int id)
        {
            if (player != PlayerMove)
                return false;
            return MakeMove(id);
        }
        public bool MakeMove(Players player, int x, int y)
        {
            return MakeMove(player, ConvertPlayerMoveToId(x, y));
        }
        public bool MakeMove(int x, int y)
        {
            return MakeMove(ConvertPlayerMoveToId(x, y));
        }
        private struct HistoryMove
        {
            public int id;
            public bool all;
        }
        private Stack<HistoryMove> history = new Stack<HistoryMove>();
        public void Undo()
        {
            HistoryMove move = history.Pop();
            board[move.id] = Players.None;
            winners[move.id / 9] = Players.None;
            Winner = Players.None;
            if (history.Count > 0)
                LastMove = history.Peek().id;
            PlayerMove = PlayerMove.GetOpponent();
            ActiveBoard = new ActiveBoard
            {
                id = move.id / 9,
                all = move.all
            };
        }
        public PlayerMove[] GetAllMoves() // TODO: Yield?
        {
            List<int> result = new List<int>();
            if (!IsFinished)
                if (ActiveBoard.all)
                {
                    for (int i = 0; i < 9; i++)
                        if (winners[i] == Players.None)
                            for (int j = 0; j < 9; j++)
                                if (board[i * 9 + j] == Players.None)
                                    result.Add(i * 9 + j);
                }
                else
                    for (int i = 0; i < 9; i++)
                        if (board[ActiveBoard.id * 9 + i] == Players.None)
                            result.Add(ActiveBoard.id * 9 + i);
            return result.ConvertAll(id => ConvertIdToPlayerMove(id)).ToArray();
            //return GetAllMovesId().ToList().ConvertAll(id => ConvertIdToPlayerMove(id)).ToArray();
        }
        public int[] GetAllMovesId() // TODO: Yield?
        {
            List<int> result = new List<int>();
            if (!IsFinished)
                if (ActiveBoard.all)
                {
                    for (int i = 0; i < 9; i++)
                        if (winners[i] == Players.None)
                            for (int j = 0; j < 9; j++)
                                if (board[i * 9 + j] == Players.None)
                                    result.Add(i * 9 + j);
                }
                else
                    for (int i = 0; i < 9; i++)
                        if (board[ActiveBoard.id * 9 + i] == Players.None)
                            result.Add(ActiveBoard.id * 9 + i);
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

        public object Clone()
        {
            UltimateTicTacToe result = (UltimateTicTacToe)MemberwiseClone();
            result.history = new Stack<HistoryMove>(history); // TODO: Clone without history for speed
            result.board = (Players[])board.Clone();
            result.winners = (Players[])winners.Clone();
            //result.boards = new TicTacToe[LocalBoardCount, LocalBoardCount];
            //for (int i = 0; i < LocalBoardCount; i++)
            //    for (int j = 0; j < LocalBoardCount; j++)
            //        result.boards[i, j] = (TicTacToe)boards[i, j].Clone();
            return result;
        }
        public int LastMove { get; private set; } = -1;
        public PlayerMove LastAction
        {
            get
            {
                CoordConvert(LastMove / 9, LastMove % 9, out int x, out int y);
                return new PlayerMove(x, y);
            }
        }
    }

    public class BoardProxy // TODO: move in personal file
    {
        public const int LocalBoardSize = 3;
        public const int BoardSize = LocalBoardCount * LocalBoardSize; // TODO: Remove duplicate
        public const int LocalBoardCount = LocalBoardSize;

        readonly UltimateTicTacToe board;
        public PlayerMove[] GetAllMoves() => board.GetAllMoves();
        //public Players GetOwner(int x, int y) => board.GetOwner(x, y);
        public bool MakeMove(int id) => board.MakeMove(Player, id); // TODO: throw error if try enemy move
        public bool MakeMove(int x, int y) => board.MakeMove(Player, x, y); // TODO: throw error if try enemy move
        //public StrategyAction LastAction => board.LastAction;
        //public ActiveBoard ActiveBoard => board.ActiveBoard;
        //public bool Undo();
        //public bool OfferDraw();
        //public bool Surrender();
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
