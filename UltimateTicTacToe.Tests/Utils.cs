using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Tests
{
    static class Utils
    {
        public static bool CompareBoardWithArray(UltimateTicTacToe board, Players[,] cells)
        {
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (board[i, j] != cells[i, j])
                        return false;
            return true;
        }
        //public static bool CompareMiniBoardWithArray(TicTacToe board, Players[,] cells)
        //{
        //    for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
        //        for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
        //            if (board[i, j] != cells[i, j])
        //                return false;
        //    return true;
        //}
        public static bool CompareWinnersWithArray(UltimateTicTacToe board, Players[,] cells)
        {
            for (int i = 0; i < UltimateTicTacToe.LocalBoardCount; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardCount; j++)
                    if (board.GetOwner(i, j) != cells[i, j])
                        return false;
            return true;
        }
        public static bool BoardsEqual(UltimateTicTacToe left, UltimateTicTacToe right)
        {
            if (left.Winner != right.Winner)
                return false;
            for (int i = 0; i < UltimateTicTacToe.LocalBoardCount; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardCount; j++)
                    if (left.GetOwner(i, j) != right.GetOwner(i, j))
                        return false;
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (left[i, j] != right[i, j])
                        return false;
            return true;
        }

        public static bool EqualArrays<T>(T[] a, T[] b)
        {
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
                if (!a[i].Equals(b[i]))
                    return false;
            return true;
        }

        public static Players MakeMoves(UltimateTicTacToe board, PlayerMove[] moves, Players player)
        {
            foreach (var move in moves)
            {
                Assert.IsTrue(board.MakeMove(player, move.x, move.y), "Move should be valid");
                player = player.GetOpponent();
            }
            return player;
        }
    }
}
