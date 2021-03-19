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
            Board board = new Board();
            CellTypes[,] cells = new CellTypes[Board.BoardSize, Board.BoardSize];
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Empty default values");
            GameResults[,] winners = new GameResults[Board.LocalBoardCount, Board.LocalBoardCount];
            Assert.IsTrue(Utils.CompareWinnersWithArray(board, winners), "None local winners");

            Assert.AreEqual(board.ActiveBoard, new ActiveBoard { all = true, x = 0, y = 0 });
            Assert.AreEqual(board.IsFinished, false, "Game couldn't start finished");
            Assert.AreEqual(board.Winner, GameResults.None, "Game couldn't start with winner");
            Assert.AreEqual(board.PlayerMove, Players.First, "Game must start with first player");
        }
        [TestMethod]
        public void BoardAllMoves1()
        {
            Board board = new Board();
            
            PlayerMove[] moves = board.GetAllMoves();
            Array.Sort(moves);
            List<PlayerMove> expected = new List<PlayerMove>();
            for (int i = 0; i < Board.BoardSize; i++)
                for (int j = 0; j < Board.BoardSize; j++)
                    expected.Add(new PlayerMove(i, j));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All Cells on start");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0));
            moves = board.GetAllMoves();
            Array.Sort(moves);
            expected.Clear();
            for (int i = 0; i < Board.LocalBoardSize; i++)
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    if (i != 0 || j != 0)
                        expected.Add(new PlayerMove(i, j));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All free cells on board 0, 0");

            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1));
            moves = board.GetAllMoves();
            Array.Sort(moves);
            expected.Clear();
            for (int i = 0; i < Board.LocalBoardSize; i++)
                for (int j = 0; j < Board.LocalBoardSize; j++)
                    expected.Add(new PlayerMove(i + Board.LocalBoardSize, j + Board.LocalBoardSize));
            expected.Sort();
            Assert.IsTrue(Utils.EqualArrays(expected.ToArray(), moves), "All cells on board 1, 1");
        }
        [TestMethod]
        public void BoardAllMoves2()
        {
            //Board board = new Board();
            //System.Drawing.Point[] moves = board.GetAllMoves();
            //Array.Sort(moves);
            //System.Drawing.Point[] expected = new System.Drawing.Point[Board.BoardSize * Board.BoardSize];
            //for (int i = 0; i < Board.BoardSize; i++)
            //    for (int j = 0; j < Board.BoardSize; j++)
            //        expected[i * Board.BoardSize + j] = new System.Drawing.Point(i, j);
            //Array.Sort(expected);
            //Assert.AreEqual(expected, moves, "All Cells on start");
        }
        [TestMethod]
        public void BoardMove1()
        {
            Board board = new Board();
            CellTypes[,] cells = new CellTypes[Board.BoardSize, Board.BoardSize];
            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Second player couldn't start game");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");

            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            cells[0, 0] = CellTypes.First;
            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "First move could be placed everywhere");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(new ActiveBoard { all = false, x = 0, y = 0 }, board.ActiveBoard, "Board 0, 0");
            
            Assert.IsFalse(board.MakeMove(Players.First, 1, 0), "First player couldn't play during opponent's move");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            
            Assert.IsFalse(board.MakeMove(Players.Second, 8, 8), "Player must move only to active board");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            
            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Already taken cell");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");

            cells[1, 1] = CellTypes.Second;
            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1), "Valid move");
            Assert.IsTrue(Utils.CompareBoardWithArray(board, cells), "Cells should be stay unchanged");
            Assert.AreEqual(new ActiveBoard { all = false, x = 1, y = 1 }, board.ActiveBoard, "Board 1, 1");
        }
    }
}
