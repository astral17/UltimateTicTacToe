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
        public static bool CompareBoardWithArray(Board board, CellTypes[,] cells)
        {
            for (int i = 0; i < Board.BoardSize; i++)
                for (int j = 0; j < Board.BoardSize; j++)
                    if (board.boards[i / Board.LocalBoardSize, j / Board.LocalBoardSize]
                                    [i % Board.LocalBoardSize, j % Board.LocalBoardSize] != cells[i, j])
                        return false;
            return true;
        }
        public static bool CompareMiniBoardWithArray(MiniBoard board, CellTypes[,] cells)
        {
            for (int i = 0; i < MiniBoard.LocalBoardSize; i++)
                for (int j = 0; j < MiniBoard.LocalBoardSize; j++)
                    if (board[i, j] != cells[i, j])
                        return false;
            return true;
        }
        public static bool CompareWinnersWithArray(Board board, GameResults[,] cells)
        {
            for (int i = 0; i < Board.LocalBoardCount; i++)
                for (int j = 0; j < Board.LocalBoardCount; j++)
                    if (board.boards[i, j].Winner != cells[i, j])
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

        public static Players MakeMoves(AbstractBoard board, PlayerMove[] moves, Players player)
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
