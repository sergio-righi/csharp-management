using System;
using System.Collections.Generic;
using System.Text;

namespace Tool.Extensions
{
    public static class IListExtension
    {
        public static int RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            int count = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    ++count;
                    list.RemoveAt(i);
                }
            }

            return count;
        }

        public static void RemoveLast<T>(this IList<T> list, int quantity)
        {
            int count = list.Count;

            for (var i = count; i != 0 && quantity != 0; i--, quantity--)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static void Swap<T>(this IList<T> list, int from, int to)
        {
            if (to <= list.Count)
            {
                T aux = list[from];
                list[from] = list[to];
                list[to] = aux;
            }
        }
    }
}
