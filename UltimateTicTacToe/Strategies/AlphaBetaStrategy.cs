﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe.Strategies
{
    public class AlphaBetaStrategy : IStrategy
    {
        protected struct StrategyMove
        {
            public int id, score;
            public StrategyMove(int score)
            {
                this.score = score;
                id = -1;
            }
        }

        protected readonly int maxDepth;
        public AlphaBetaStrategy(int maxDepth = 6)
        {
            this.maxDepth = maxDepth;
        }

        private static readonly int[] lineScore = new int[64];
        static AlphaBetaStrategy()
        {
            int[] scorePerCell = new int[] { 0, 1, 10, 100 };
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        int cur = (i << 4) + (j << 2) + k;
                        lineScore[cur] = scorePerCell[i + j + k];
                        lineScore[cur << 1] = -scorePerCell[i + j + k];
                    }
        }
        protected virtual int GetBoardScore(Players[] board, Players player)
        {
            int score = 0, line;
            // Vertical
            for (int i = 0; i < 3; i++)
            {
                line = 0;
                for (int j = i; j < 9; j += 3)
                    line = (line << 2) + (int)board[j];
                score += lineScore[line];
            }
            // Horizontal
            for (int i = 0; i < 9; i += 3)
            {
                line = 0;
                for (int j = 0; j < 3; j++)
                    line = (line << 2) + (int)board[i + j];
                score += lineScore[line];
            }
            // Main Diagonal
            line = 0;
            for (int i = 0; i < 9; i += 4)
                line = (line << 2) + (int)board[i];
            score += lineScore[line];
            // Side Diagonal
            line = 0;
            for (int i = 2; i < 8; i += 2)
                line = (line << 2) + (int)board[i];
            score += lineScore[line];
            if (player == Players.Second)
                score *= -1;
            return score;
        }
        protected virtual int GetScore(UltimateTicTacToe board) // TODO: Extract heuristics
        {
            if (board.Winner == board.PlayerMove)
                return 100000;
            if (board.Winner == board.PlayerMove.GetOpponent())
                return -100000;
            Players player = board.PlayerMove;
            int score = GetBoardScore(board.GetRawWinners(), player) * 100;
            for (int x = 0; x < UltimateTicTacToe.LocalBoardCount; x++)
                for (int y = 0; y < UltimateTicTacToe.LocalBoardCount; y++)
                {
                    if (board.GetOwner(x, y) == Players.None)
                        score += GetBoardScore(board.GetRawBoard(x, y), player);
                }
            return score;
        }
        // alpha shouldn't be int.MinValue, because int.MinValue == -int.MinValue
        private StrategyMove AlphaBeta(UltimateTicTacToe board, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue)
        {
            if (depth == 0 || board.IsFinished)
                return new StrategyMove(GetScore(board));
            int[] moves = board.GetAllMovesId(); // TODO: Sort by score
            moves.Shuffle();
            StrategyMove bestMove = new StrategyMove(-int.MaxValue), curMove;
            foreach (int move in moves)
            {
                board.MakeMove(move); // TODO: Check is moved
                curMove.id = move;
                curMove.score = -AlphaBeta(board, depth - 1, -beta, -alpha).score;
                board.Undo();
                if (bestMove.score < curMove.score)
                    bestMove = curMove;
                if (beta <= curMove.score)
                    break;
                alpha = Math.Max(alpha, curMove.score);
            }
            return bestMove;
        }
        public void MakeMove(BoardProxy board)
        {
            StrategyMove move = AlphaBeta(board.GetBoardCopy(), maxDepth);
            PlayerMove tmp = UltimateTicTacToe.ConvertIdToPlayerMove(move.id);
            Console.WriteLine("AlphaBetaDebug: score = {0}, at [{1}, {2}]", move.score, tmp.x, tmp.y);
            board.MakeMove(move.id);
        }
    }
}
