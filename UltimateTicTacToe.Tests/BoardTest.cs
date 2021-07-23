using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateTicTacToe.Tests
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void BoardInit()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Players[,] cells = new Players[UltimateTicTacToe.BoardSize, UltimateTicTacToe.BoardSize];
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Empty default values");
            GameResults[,] winners = new GameResults[UltimateTicTacToe.LocalBoardCount, UltimateTicTacToe.LocalBoardCount];
            Assert.IsTrue(Utils.CompareWinnersWithArray(board, winners), "None local winners");

            Assert.AreEqual(board.ActiveBoard, new ActiveBoard { all = true, x = 0, y = 0 });
            Assert.AreEqual(board.IsFinished, false, "Game couldn't start finished");
            Assert.AreEqual(board.Winner, GameResults.None, "Game couldn't start with winner");
            Assert.AreEqual(board.PlayerMove, Players.First, "Game must start with first player");
        }
        [TestMethod]
        public void BoardPlayerMoveCheck()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Players[,] cells = new Players[UltimateTicTacToe.BoardSize, UltimateTicTacToe.BoardSize];

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "Move should be valid");
            cells[0, 0] = Players.First;
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsFalse(board.MakeMove(Players.First, 2, 2), "Move shouldn't be valid");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell shouldn't be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1), "Move should be valid");
            cells[1, 1] = Players.Second;
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");
        }
        [TestMethod]
        public void BoardAllMoves1()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            
            PlayerMove[] moves = board.GetAllMoves();
            Array.Sort(moves);
            List<PlayerMove> expected = new List<PlayerMove>();
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    expected.Add(new PlayerMove(i, j));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All Cells on start");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0));
            moves = board.GetAllMoves();
            Array.Sort(moves);
            expected.Clear();
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    if (i != 0 || j != 0)
                        expected.Add(new PlayerMove(i, j));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All free cells on board 0, 0");

            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1));
            moves = board.GetAllMoves();
            Array.Sort(moves);
            expected.Clear();
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    expected.Add(new PlayerMove(i + UltimateTicTacToe.LocalBoardSize, j + UltimateTicTacToe.LocalBoardSize));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All cells on board 1, 1");
        }
        [TestMethod]
        public void BoardAllMoves2()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Assert.AreEqual(Players.None, board.GetOwner(0, 0), "None winners at board 0, 0");
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0),
                new PlayerMove(1, 1),
                new PlayerMove(4, 4),
                new PlayerMove(3, 3),
                new PlayerMove(0, 1),
                new PlayerMove(0, 3),
                new PlayerMove(0, 2),
            }, Players.First);
            Assert.AreEqual(Players.First, board.GetOwner(0, 0), "First player win at board 0, 0");
            PlayerMove[] moves = board.GetAllMoves();
            Array.Sort(moves);
            List<PlayerMove> expected = new List<PlayerMove>();
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    expected.Add(new PlayerMove(i, j + UltimateTicTacToe.LocalBoardSize * 2));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All Cells at board 0, 2");

            Assert.IsTrue(board.MakeMove(Players.Second, 0, 6), "Cell must be free at board 0, 2");
            moves = board.GetAllMoves();
            Array.Sort(moves);
            expected = new List<PlayerMove>();
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if ((i / UltimateTicTacToe.LocalBoardSize != 0 || j / UltimateTicTacToe.LocalBoardSize != 0) &&
                        (i != 0 || j != 3) && (i != 3 || j != 3) && (i != 4 || j != 4) && (i != 0 || j != 6))
                        expected.Add(new PlayerMove(i, j));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All free cells except board 0, 0");
        }
        [TestMethod]
        public void BoardAllMoves3()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Assert.AreEqual(Players.None, board.GetOwner(0, 0), "None winners at board 0, 0");
            PlayerMove[] moves = board.GetAllMoves();
            Players player = Players.First;
            while (moves.Length > 0)
            {
                Assert.IsFalse(board.IsFinished, "Game couldn't be finished if any moves are available");
                board.MakeMove(player, moves[0].x, moves[0].y);
                moves = board.GetAllMoves();
                player = (Players)(3 - (int)player);
            }
            Assert.IsTrue(board.IsFinished, "Game must finish when no any available moves");
        }
        [TestMethod]
        public void BoardMove1()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Players[,] cells = new Players[UltimateTicTacToe.BoardSize, UltimateTicTacToe.BoardSize];
            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Second player couldn't start game");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");

            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            cells[0, 0] = Players.First;
            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "First move could be placed everywhere");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(new ActiveBoard { all = false, x = 0, y = 0 }, board.ActiveBoard, "Board 0, 0");
            
            Assert.IsFalse(board.MakeMove(Players.First, 1, 0), "First player couldn't play during opponent's move");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            
            Assert.IsFalse(board.MakeMove(Players.Second, 8, 8), "Player must move only to active board");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            
            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Already taken cell");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");

            cells[1, 1] = Players.Second;
            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1), "Valid move");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            Assert.AreEqual(new ActiveBoard { all = false, x = 1, y = 1 }, board.ActiveBoard, "Board 1, 1");
        }
        [TestMethod]
        public void BoardMove2()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Assert.AreEqual(Players.None, board.GetOwner(0, 0), "None winners at board 0, 0");
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0),
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
                new PlayerMove(0, 1),
                new PlayerMove(0, 3),
                new PlayerMove(0, 2),
                new PlayerMove(0, 6),
                new PlayerMove(1, 0),
                new PlayerMove(3, 0),
                new PlayerMove(2, 2),
                new PlayerMove(8, 7),
                new PlayerMove(6, 3),
                new PlayerMove(2, 0),
                new PlayerMove(6, 0),
                new PlayerMove(1, 2),
                new PlayerMove(3, 6),
            }, Players.First);
            Assert.AreEqual(Players.None, board.GetOwner(0, 0), "None winners at board 0, 0");
            Assert.IsTrue(board.MakeMove(Players.First, 2, 1), "Last free cell at board 0, 0");
            Assert.AreEqual(Players.None, board.GetOwner(0, 0), "Draw at board 0, 0");
        }
        [TestMethod]
        public void BoardWin1()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
                new PlayerMove(2, 2),
                new PlayerMove(6, 6),
                new PlayerMove(0, 0),
                new PlayerMove(4, 3),
                new PlayerMove(3, 2),
                new PlayerMove(1, 6),
                new PlayerMove(4, 2),
                new PlayerMove(4, 6),
                new PlayerMove(5, 2),
                new PlayerMove(8, 6),
                new PlayerMove(6, 1),
                new PlayerMove(2, 3),
                new PlayerMove(7, 1),
                new PlayerMove(5, 3),
                new PlayerMove(8, 1),
            }, Players.First);
            Assert.AreEqual(Players.First, board.GetOwner(0, 0), "First win at board 0, 0");
            Assert.AreEqual(Players.First, board.GetOwner(1, 0), "First win at board 1, 0");
            Assert.AreEqual(Players.First, board.GetOwner(2, 0), "First win at board 2, 0");
            Assert.AreEqual(Players.Second, board.GetOwner(1, 1), "Second win at board 1, 1");
            Assert.AreEqual(GameResults.FirstWin, board.Winner, "First player win");

            Assert.IsFalse(board.MakeMove(Players.Second, 7, 4), "Game already finished");
            Assert.AreEqual(0, board.GetAllMoves().Length, "Game already finished");

        }
        [TestMethod]
        public void BoardWin2()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(6, 3),
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
                new PlayerMove(2, 2),
                new PlayerMove(6, 6),
                new PlayerMove(0, 0),
                new PlayerMove(4, 3),
                new PlayerMove(3, 2),
                new PlayerMove(1, 6),
                new PlayerMove(4, 2),
                new PlayerMove(4, 6),
                new PlayerMove(5, 2),
                new PlayerMove(8, 6),
                new PlayerMove(6, 1),
                new PlayerMove(2, 3),
                new PlayerMove(7, 1),
                new PlayerMove(5, 3),
                new PlayerMove(8, 1),
            }, Players.First);
            Assert.AreEqual(Players.Second, board.GetOwner(0, 0), "Second win at board 0, 0");
            Assert.AreEqual(Players.Second, board.GetOwner(1, 0), "Second win at board 1, 0");
            Assert.AreEqual(Players.Second, board.GetOwner(2, 0), "Second win at board 2, 0");
            Assert.AreEqual(Players.First, board.GetOwner(1, 1), "First win at board 1, 1");
            Assert.AreEqual(GameResults.SecondWin, board.Winner, "Second player win");

            Assert.IsFalse(board.MakeMove(Players.Second, 7, 4), "Game already finished");
            Assert.AreEqual(0, board.GetAllMoves().Length, "Game already finished");
        }
        [TestMethod]
        public void CloneEqual1()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            UltimateTicTacToe board2 = (UltimateTicTacToe)board.Clone();
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            Assert.AreEqual(board.PlayerMove, board2.PlayerMove, "PlayerMove should be equal");
            Assert.AreEqual(board.ActiveBoard, board2.ActiveBoard, "ActiveBoard should be equal");
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    Assert.AreEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board winners should be equal");
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneEqual2()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
                new PlayerMove(2, 2),
                new PlayerMove(6, 6),
                new PlayerMove(0, 0),
                new PlayerMove(4, 3),
                new PlayerMove(3, 2),
                new PlayerMove(1, 6),
                new PlayerMove(4, 2),
                new PlayerMove(4, 6),
                new PlayerMove(5, 2),
                new PlayerMove(8, 6),
                new PlayerMove(6, 1),
                new PlayerMove(2, 3),
                new PlayerMove(7, 1),
                new PlayerMove(5, 3),
                new PlayerMove(8, 1),
            }, Players.First);
            UltimateTicTacToe board2 = (UltimateTicTacToe)board.Clone();
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            Assert.AreEqual(board.PlayerMove, board2.PlayerMove, "PlayerMove should be equal");
            Assert.AreEqual(board.ActiveBoard, board2.ActiveBoard, "ActiveBoard should be equal");
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    Assert.AreEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board winners should be equal");
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef1()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 0),
                new PlayerMove(3, 0),
                new PlayerMove(0, 0),
                new PlayerMove(0, 1),
                new PlayerMove(0, 3),
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
            }, Players.First);
            UltimateTicTacToe board2 = (UltimateTicTacToe)board.Clone();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(2, 1),
            }, Players.Second);
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            Assert.AreNotEqual(board.PlayerMove, board2.PlayerMove, "PlayerMove shouldn't be equal");
            Assert.AreNotEqual(board.ActiveBoard, board2.ActiveBoard, "ActiveBoard shouldn't be equal");
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    if (i == 0 && j == 0)
                        Assert.AreNotEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board[0, 0] winners shouldn't be equal");
                    else
                        Assert.AreEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board winners should be equal");
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (i == 2 && j == 1)
                        Assert.AreNotEqual(board[i, j], board2[i, j], "Cells shouldn't be equal");
                    else
                        Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef2()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 0),
                new PlayerMove(3, 0),
                new PlayerMove(0, 0),
                new PlayerMove(0, 1),
                new PlayerMove(0, 3),
                new PlayerMove(1, 1),
                new PlayerMove(3, 3),
            }, Players.First);
            UltimateTicTacToe board2 = (UltimateTicTacToe)board.Clone();
            Utils.MakeMoves(board2, new PlayerMove[]
            {
                new PlayerMove(2, 1),
            }, Players.Second);
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            Assert.AreNotEqual(board.PlayerMove, board2.PlayerMove, "PlayerMove shouldn't be equal");
            Assert.AreNotEqual(board.ActiveBoard, board2.ActiveBoard, "ActiveBoard shouldn't be equal");
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    if (i == 0 && j == 0)
                        Assert.AreNotEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board[0, 0] winners shouldn't be equal");
                    else
                        Assert.AreEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board winners should be equal");
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (i == 2 && j == 1)
                        Assert.AreNotEqual(board[i, j], board2[i, j], "Cells shouldn't be equal");
                    else
                        Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef3()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 0),
                new PlayerMove(3, 0),
                new PlayerMove(0, 0),
                new PlayerMove(0, 1),
                new PlayerMove(0, 3),
                new PlayerMove(1, 1),
            }, Players.First);
            UltimateTicTacToe board2 = (UltimateTicTacToe)board.Clone();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(3, 3),
                new PlayerMove(2, 1),
            }, Players.First);
            Utils.MakeMoves(board2, new PlayerMove[]
            {
                new PlayerMove(3, 5),
                new PlayerMove(0, 6),
                new PlayerMove(2, 0),
            }, Players.First);
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            Assert.AreNotEqual(board.PlayerMove, board2.PlayerMove, "PlayerMove shouldn't be equal");
            Assert.AreNotEqual(board.ActiveBoard, board2.ActiveBoard, "ActiveBoard shouldn't be equal");
            for (int i = 0; i < UltimateTicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardSize; j++)
                    if (i == 0 && j == 0)
                        Assert.AreNotEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board[0, 0] winners shouldn't be equal");
                    else
                        Assert.AreEqual(board.GetOwner(i, j), board2.GetOwner(i, j), "Board winners should be equal");
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (i == 2 && j == 1 || i == 3 && j == 3 || i == 3 && j == 5 || i == 0 && j == 6 || i == 2 && j == 0)
                        Assert.AreNotEqual(board[i, j], board2[i, j], "Cells shouldn't be equal");
                    else
                        Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");

        }
    }
}
