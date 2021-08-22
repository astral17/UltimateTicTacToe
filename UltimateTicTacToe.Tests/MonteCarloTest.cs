using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateTicTacToe.Tests
{
    [TestClass]
    public class MonteCarloTest
    {
        [TestMethod, Timeout(5000)]
        public void MonteCarloWinRandom()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.MonteCarloStrategy(500), new Strategies.RandomStrategy()).Wait();
            Assert.AreEqual(Players.First, gm.Board.Winner, "MonteCarlo must win Random");
            gm.StartGame(new Strategies.RandomStrategy(), new Strategies.MonteCarloStrategy(500)).Wait();
            Assert.AreEqual(Players.Second, gm.Board.Winner, "MonteCarlo must win Random");
        }
        //[TestMethod, Timeout(10000)]
        //public void MonteCarloWinAlphaBeta1()
        //{
        //    GameManager gm = new GameManager();
        //    gm.StartGame(new Strategies.MonteCarloStrategy(3000), new Strategies.AlphaBetaStrategy(2)).Wait();
        //    Assert.AreEqual(Players.First, gm.Board.Winner, "MonteCarlo must win AlphaBeta");
        //    gm.StartGame(new Strategies.AlphaBetaStrategy(2), new Strategies.MonteCarloStrategy(3000)).Wait();
        //    Assert.AreEqual(Players.Second, gm.Board.Winner, "MonteCarlo must win AlphaBeta");
        //}
        [TestMethod, Timeout(200)]
        public void MonteCarloSpeed1()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.MonteCarloStrategy(10), new Strategies.MonteCarloStrategy(10)).Wait();
            Assert.IsTrue(gm.Board.IsFinished, "Game must be finished");
        }
        [TestMethod, Timeout(2000)]
        public void MonteCarloSpeed2()
        {
            GameManager gm = new GameManager();
            gm.StartGame(new Strategies.MonteCarloStrategy(), new Strategies.MonteCarloStrategy()).Wait();
            Assert.IsTrue(gm.Board.IsFinished, "Game must be finished");
        }
        [TestMethod, Timeout(2000)]
        public void MonteCarloSpeed3()
        {
            UltimateTicTacToe board = new UltimateTicTacToe();
            IStrategy strategy = new Strategies.MonteCarloStrategy(10000);
            strategy.MakeMove(new BoardProxy(board, Players.First));
            Assert.AreEqual(Players.Second, board.PlayerMove, "Strategy must make move");
        }
    }
}
