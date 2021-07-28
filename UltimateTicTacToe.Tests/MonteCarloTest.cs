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
            gm.StartGame(new Strategies.MonteCarloStrategy(), new Strategies.RandomStrategy()).Wait();
            Assert.AreEqual(Players.First, gm.Board.Winner, "MonteCarlo must win Random");
            gm.StartGame(new Strategies.RandomStrategy(), new Strategies.MonteCarloStrategy()).Wait();
            Assert.AreEqual(Players.Second, gm.Board.Winner, "MonteCarlo must win Random");
        }
        [TestMethod, Timeout(100)]
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
            strategy.Init(new BoardProxy(board, Players.First));
            strategy.MakeTurn();
            Assert.AreEqual(Players.Second, board.PlayerMove, "Strategy must make move");
        }
    }
}
