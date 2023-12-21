namespace SLAE_Calculator.Extensions
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }

        public static int GetMinIndex(this List<int> list, int startIndex)
        {
            var tmp = list.Select(Math.Abs).ToList();


            int min = tmp[startIndex];
            int minIndex = -1;

            for (int i = startIndex + 1; i < list.Count; ++i)
            {
                if (tmp[i] <= min && tmp[i] != 0)
                {
                    min = Math.Abs(list[i]);
                    minIndex = i;
                }
            }

            return minIndex;
        }
       
        public static int GetLastNoZeroElementIndex(this List<int> list, int ind)
        {
            return list.IndexOf(list
                    .LastOrDefault(x => x != 0
                    && list.IndexOf(x) != ind));
        }

        public static int LastIndexOfNonZeroElement(this List<int> list)
        {
            return list.IndexOf(list.Last(x => x != 0));
        }

        public static List<bool> GetMaskElements(this List<int> list) 
        {
            return list.Select(x => x != 0).ToList();
        }
    }
}
