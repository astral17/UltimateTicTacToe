using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToe
{
    static class Utils
    {
        private static readonly Random random = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int n = list.Count - 1; n > 1; n--)
            {
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static T RandomElement<T>(this IList<T> list)
        {
            int index = random.Next(list.Count);
            return list[index];
        }
    }
}
