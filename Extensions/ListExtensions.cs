namespace SLAE_Calculator.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }

        public static int GetMinIndex(this List<int> list)
        {
            return list.IndexOf(list
                    .Where(x => x != 0
                    && list.IndexOf(x) != list.Count() - 1).Min());
        }

        public static int GetFirstNoZeroElementIndex(this List<int> list, int ind)
        {
            return list.IndexOf(list
                    .FirstOrDefault(x => x != 0
                    && list.IndexOf(x) != ind
                    && list.IndexOf(x) != list.Count() - 1));
        }

        public static int LastIndexOfNonZeroElement(this List<int> list)
        {
            return list.IndexOf(list.Last(x => x != 0));
        }
    }
}
