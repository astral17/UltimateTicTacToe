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
        public static bool CompareBoardWithArray(UltimateTicTacToe board, CellOwners[,] cells)
        {
            for (int i = 0; i < UltimateTicTacToe.BoardSize; i++)
                for (int j = 0; j < UltimateTicTacToe.BoardSize; j++)
                    if (board[i, j] != cells[i, j])
                        return false;
            return true;
        }
        public static bool CompareMiniBoardWithArray(TicTacToe board, CellOwners[,] cells)
        {
            for (int i = 0; i < TicTacToe.LocalBoardSize; i++)
                for (int j = 0; j < TicTacToe.LocalBoardSize; j++)
                    if (board[i, j] != cells[i, j])
                        return false;
            return true;
        }
        public static bool CompareWinnersWithArray(UltimateTicTacToe board, GameResults[,] cells)
        {
            for (int i = 0; i < UltimateTicTacToe.LocalBoardCount; i++)
                for (int j = 0; j < UltimateTicTacToe.LocalBoardCount; j++)
                    if ((int)board.GetOwner(i, j) != (int)cells[i, j]) // TODO: Draw fix
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

        public static Players MakeMoves(Board board, PlayerMove[] moves, Players player)
        {
            foreach (var move in moves)
            {
                Assert.IsTrue(board.MakeMove(player, move.x, move.y), "Move should be valid");
                player = (Players)(3 - (int)player);
            }
            return player;
        }
    }
}
