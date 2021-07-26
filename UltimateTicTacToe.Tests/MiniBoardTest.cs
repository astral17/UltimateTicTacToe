using System;
using System.Collections.Generic;
using System.Linq;
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
            Players[,] cells = new Players[TicTacToe.LocalBoardSize, TicTacToe.LocalBoardSize];
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Empty default values");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");
        }

        [TestMethod]
        public void BoardNoPlayerMoveCheck()
        {
            TicTacToe board = new TicTacToe();
            Players[,] cells = new Players[TicTacToe.LocalBoardSize, TicTacToe.LocalBoardSize];

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "Move should be valid");
            cells[0, 0] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 2, 2), "Move should be valid");
            cells[2, 2] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 1, 1), "Move should be valid");
            cells[1, 1] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.First, board.Winner, "First Win");
            Assert.AreEqual(0, board.GetAllMoves().Length, "No moves after finished game");
        }

        [TestMethod]
        public void BoardMove1()
        {
            TicTacToe board = new TicTacToe();
            Players[,] cells = new Players[TicTacToe.LocalBoardSize, TicTacToe.LocalBoardSize];
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Empty default values");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 0), "100\n000\n000");
            cells[0, 0] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsFalse(board.MakeMove(Players.Second, 0, 0), "Cell already taken");
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell shouldn't be updated");

            Assert.IsTrue(board.MakeMove(Players.Second, 1, 1), "100\n020\n000");
            cells[1, 1] = Players.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 2, 2), "100\n020\n001");
            cells[2, 2] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.Second, 2, 0), "100\n020\n201");
            cells[2, 0] = Players.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 0, 2), "101\n020\n201");
            cells[0, 2] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.Second, 0, 1), "121\n020\n201");
            cells[0, 1] = Players.Second;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.None, board.Winner, "Game shouldn't be finished yet");

            Assert.IsTrue(board.MakeMove(Players.First, 1, 2), "121\n021\n201");
            cells[1, 2] = Players.First;
            Assert.IsTrue(Utils.CompareMiniBoardWithArray(board, cells), "Cell should be updated");
            Assert.AreEqual(Players.First, board.Winner, "First win");

            Assert.AreEqual(0, board.GetAllMoves().Length, "No moves after finished game");
        }
        [TestMethod]
        public void BoardDraw1()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(1, 1), new PlayerMove(0, 0),
                new PlayerMove(0, 2), new PlayerMove(2, 0),
                new PlayerMove(1, 0), new PlayerMove(1, 2),
                new PlayerMove(0, 1), new PlayerMove(2, 1),
                new PlayerMove(2, 2),
            }, Players.First);
            Assert.AreEqual(Players.Draw, board.Winner, "Draw");
        }
        [TestMethod]
        public void BoardDraw2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 1), new PlayerMove(2, 2),
                new PlayerMove(0, 0), new PlayerMove(1, 0),
                new PlayerMove(1, 2),
            }, Players.First);
            Assert.AreEqual(Players.Draw, board.Winner, "Draw");
        }
        [TestMethod]
        public void BoardWin1()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0), new PlayerMove(1, 0),
                new PlayerMove(2, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            }, Players.First);
            Assert.AreEqual(Players.First, board.Winner, "First win");
            Assert.IsFalse(board.MakeMove(Players.Second, 1, 2), "Move after win");
        }
        [TestMethod]
        public void BoardWin2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(1, 1),
                new PlayerMove(2, 2), new PlayerMove(0, 2),
                new PlayerMove(0, 1), new PlayerMove(2, 0),
            }, Players.First);
            Assert.AreEqual(Players.Second, board.Winner, "Second win");
            Assert.IsFalse(board.MakeMove(Players.First, 1, 2), "Move after win");
        }
        [TestMethod]
        public void CloneEqual1()
        {
            TicTacToe board = new TicTacToe();
            TicTacToe board2 = (TicTacToe)board.Clone();
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneEqual2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0),
            }, Players.First);
            TicTacToe board2 = (TicTacToe)board.Clone();
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneEqual3()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0), new PlayerMove(1, 0),
                new PlayerMove(2, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            }, Players.First);
            TicTacToe board2 = (TicTacToe)board.Clone();
            Assert.AreEqual(board.Winner, board2.Winner, "Winner should be equal");
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef1()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0), new PlayerMove(1, 0),
            }, Players.First);
            TicTacToe board2 = (TicTacToe)board.Clone();
            PlayerMove[] newMoves = new PlayerMove[]
            {
                new PlayerMove(2, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            };
            Utils.MakeMoves(board, newMoves, Players.First);

            Assert.AreNotEqual(board.Winner, board2.Winner, "Winner should be different");
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    if (newMoves.Contains(new PlayerMove(i, j)))
                        Assert.AreNotEqual(board[i, j], board2[i, j], "Cells shouldn't be equal");
                    else
                        Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef2()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0), new PlayerMove(1, 0),
            }, Players.First);
            TicTacToe board2 = (TicTacToe)board.Clone();
            PlayerMove[] newMoves = new PlayerMove[]
            {
                new PlayerMove(2, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            };
            Utils.MakeMoves(board2, newMoves, Players.First);

            Assert.AreNotEqual(board.Winner, board2.Winner, "Winner should be different");
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    if (newMoves.Contains(new PlayerMove(i, j)))
                        Assert.AreNotEqual(board[i, j], board2[i, j], "Cells shouldn't be equal");
                    else
                        Assert.AreEqual(board[i, j], board2[i, j], "Cells should be equal");
        }
        [TestMethod]
        public void CloneRef3()
        {
            TicTacToe board = new TicTacToe();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(0, 0), new PlayerMove(0, 1),
                new PlayerMove(2, 0), new PlayerMove(1, 0),
            }, Players.First);
            TicTacToe board2 = (TicTacToe)board.Clone();
            Utils.MakeMoves(board, new PlayerMove[]
            {
                new PlayerMove(2, 2), new PlayerMove(1, 1),
                new PlayerMove(2, 1),
            }, Players.First);;
            Utils.MakeMoves(board2, new PlayerMove[]
            {
                new PlayerMove(1, 1), new PlayerMove(2, 2),
                new PlayerMove(1, 2), new PlayerMove(0, 2),
                new PlayerMove(2, 1),
            }, Players.First);

            Assert.AreNotEqual(board.Winner, board2.Winner, "Winner should be different");
            Utils.CompareMiniBoardWithArray(board, new Players[,]
            {
                { Players.First, Players.Second, Players.None },
                { Players.Second, Players.Second, Players.None },
                { Players.First, Players.First, Players.First },
            });
            Utils.CompareMiniBoardWithArray(board2, new Players[,]
            {
                { Players.First, Players.Second, Players.First },
                { Players.Second, Players.First, Players.Second },
                { Players.First, Players.First, Players.Second },
            });
        }
    }
}
