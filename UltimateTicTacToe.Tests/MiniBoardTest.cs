using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateTicTacToe.Tests
{
    [TestClass]
    public class MiniBoardTest
    {
        [TestMethod]
        public void BoardInit()
        {
            TicTacToe board = new TicTacToe();
            CellOwners[,] cells = new CellOwners[TicTacToe.LocalBoardSize, TicTacToe.LocalBoardSize];
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Empty default values");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");
        }

        [TestMethod]
        public void BoardMove1()
        {
            TicTacToe board = new TicTacToe();
            CellOwners[,] cells = new CellOwners[TicTacToe.LocalBoardSize, TicTacToe.LocalBoardSize];
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Empty default values");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "100\n000\n000");
            cells[0, 0] = CellOwners.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Cell already taken");

            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1), "100\n020\n000");
            cells[1, 1] = CellOwners.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 2, 2), "100\n020\n001");
            cells[2, 2] = CellOwners.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.Second, 2, 0), "100\n020\n201");
            cells[2, 0] = CellOwners.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 2), "101\n020\n201");
            cells[0, 2] = CellOwners.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.Second, 0, 1), "121\n020\n201");
            cells[0, 1] = CellOwners.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 1, 2), "121\n021\n201");
            cells[1, 2] = CellOwners.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(GameResults.FirstWin, board.Winner, "First win");

            Assert.IsTrue(board.GetAllMoves().Length == 0, "No moves after finished game");
        }
        [TestMethod]
        public void BoardDraw1()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 1),
                new PlayerMove(0, 0),
                new PlayerMove(0, 2),
                new PlayerMove(2, 0),
                new PlayerMove(1, 0),
                new PlayerMove(1, 2),
                new PlayerMove(0, 1),
                new PlayerMove(2, 1),
                new PlayerMove(2, 2),
            }, Players.First);
            Assert.AreEqual(GameResults.Draw, board.Winner, "Draw");
        }
        [TestMethod]
        public void BoardDraw2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 2),
                new PlayerMove(1, 1),
                new PlayerMove(2, 0),
                new PlayerMove(0, 1),
                new PlayerMove(2, 1),
                new PlayerMove(2, 2),
                new PlayerMove(0, 0),
                new PlayerMove(1, 0),
                new PlayerMove(1, 2),
            }, Players.First);
            Assert.AreEqual(GameResults.Draw, board.Winner, "Draw");
        }
        [TestMethod]
        public void BoardWin1()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0),
                new PlayerMove(0, 1),
                new PlayerMove(2, 0),
                new PlayerMove(1, 0),
                new PlayerMove(2, 2),
                new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            }, Players.First);
            Assert.AreEqual(GameResults.FirstWin, board.Winner, "First win");
            Assert.IsFalse(board.MakeMove(Players.Second, 1, 2), "Move after win");
        }
        [TestMethod]
        public void BoardWin2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0),
                new PlayerMove(1, 1),
                new PlayerMove(2, 2),
                new PlayerMove(0, 2),
                new PlayerMove(0, 1),
                new PlayerMove(2, 0),
            }, Players.First);
            Assert.AreEqual(GameResults.SecondWin, board.Winner, "Second win");
            Assert.IsFalse(board.MakeMove(Players.First, 1, 2), "Move after win");
        }
    }
}
