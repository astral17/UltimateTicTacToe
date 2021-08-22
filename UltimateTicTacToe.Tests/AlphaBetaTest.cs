using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateTicTacToe.Tests
{
    [TestClass]
    public class AlphaBetaTest
    {
        [TestMethod, Timeout(2000)]
        public void AlphaBetaWinRandom()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.AlphaBetaStrategy(), new Strategies.RandomStrategy()).Wait();
            Assert.AreEqual(Players.First, gm.Board.Winner, "AlphaBeta must win Random");
            gm.StartGame(new Strategies.RandomStrategy(), new Strategies.AlphaBetaStrategy()).Wait();
            Assert.AreEqual(Players.Second, gm.Board.Winner, "AlphaBeta must win Random");
        }
        [TestMethod, Timeout(200)]
        public void AlphaBetaSpeed1()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.AlphaBetaStrategy(3), new Strategies.AlphaBetaStrategy(3)).Wait();
            Assert.IsTrue(gm.Board.IsFinished, "Game must be finished");
        }
        [TestMethod, Timeout(2000)]
        public void AlphaBetaSpeed2()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.AlphaBetaStrategy(), new Strategies.AlphaBetaStrategy()).Wait();
            Assert.IsTrue(gm.Board.IsFinished, "Game must be finished");
        }
        [TestMethod, Timeout(5000)]
        public void AlphaBetaSpeed3()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            IStrategy strategy = new Strategies.AlphaBetaStrategy(8);
            strategy.MakeMove(new BoardProxy(board, Players.First));
            Assert.AreEqual(Players.Second, board.PlayerMove, "Strategy must make move");
        }
        [TestMethod, Timeout(10000)] // 8.3s -> 7.6s
        public void AlphaBetaSpeed4()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            IStrategy strategy = new Strategies.AlphaBetaStrategy(10);
            strategy.MakeMove(new BoardProxy(board, Players.First));
            Assert.AreEqual(Players.Second, board.PlayerMove, "Strategy must make move");
        }
    }
}
