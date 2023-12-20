using SLAE_Calculator.Extensions;

namespace SLAE_Calculator.Solver
{
    public class SolverSLAE
    {
        private List<List<int>> A { get; set; } = new List<List<int>>();
        private List<int> C { get; set; } = new List<int>();
        private List<List<int>> B { get; set; } = new List<List<int>>();
        private List<int> CountNoZeroElements { get; set; } = new List<int>();
        private int N, M;

        public SolverSLAE() {}

        public void ReadInput()
        {
            FileInfo sourceFile = new FileInfo(@"../../../input.txt");
            TextReader sourceFileReader = new StreamReader(sourceFile.FullName);
            Console.SetIn(sourceFileReader);

            Console.WriteLine("Set N, M");
            var parameters = Array.ConvertAll(Console.ReadLine().Trim().Split(' '), Convert.ToInt32);
            N = parameters[0];
            M = parameters[1];

            Console.WriteLine("set elements");
            for (int i = 0; i < M; i++)
            {
                var param = Array.ConvertAll(Console.ReadLine().Trim().Split(' '), Convert.ToInt32).ToList();
                if (param.Count != N + 1)
                {
                    throw new Exception("Not valid parameters count");
                }

                A.Add(param.GetRange(0, param.Count() - 1));
                C.Add(-param.Last());
            }
        }

        public void GenrateMatrixB()
        {
            B = new List<List<int>>(A);

            foreach(var line in B)
            {
                line.Add(C[B.IndexOf(line)]);
            }

            for(int i = 0; i < N; i++)
            {
                List<int> line = Enumerable.Repeat(0, N + 1).ToList();
                line[i] = 1;
                B.Add(line);
            }
        }

        public (int, List<int>) Solve()
        {
            int K = 0, i = 0;
            List<int> res = new List<int>();            

            for(int k = 0; k < M; k++)
            {
                CountNoZeroElements.Add(B[k].Where(x => x != 0).Count());
                if (B[k].Last() != 0)
                    CountNoZeroElements[k]--;
            }

            Iteration(0);


            return (K, res);
        }

        private void Iteration(int lineIndex)
        {
            int minIndex = B[lineIndex].GetMinIndex();
            int valIndex = B[lineIndex].GetFirstNoZeroElementIndex(minIndex);

            if (valIndex != -1)
            {
                int r = B[lineIndex][valIndex] % B[lineIndex][minIndex];
                int q = (B[lineIndex][valIndex] - r) / B[lineIndex][minIndex];

                for (int j = 0; j < B.Count; j++)
                {
                    if (B[j][valIndex] == 0 && q * B[j][minIndex] != 0 && j < M)
                        CountNoZeroElements[j]++;
                    B[j][valIndex] -= q * B[j][minIndex];
                }

                //if (r == 0 && (minIndex > valIndex || CountNoZeroElements[lineIndex] > minIndex))
                //{
                //    CountNoZeroElements[lineIndex]--;

                //    for (int j = 0; j < B.Count; j++)
                //    {
                //        B[lineIndex].Swap(valIndex, minIndex);
                //    }
                //}

                LineSwapper();
                ColumnSwapper();
            }
        }

        private void LineSwapper()
        {
            for(int i = 1; i < CountNoZeroElements.Count(); i++)
            {
                for(int j = 0; j < CountNoZeroElements.Count() - i; j++)
                {
                    if (CountNoZeroElements[j] > CountNoZeroElements[j + 1])
                    {
                        CountNoZeroElements.Swap(i, j);
                        B.Swap(i, j);
                    }
                }
            }
        }

        private void ColumnSwapper()
        {
            for(int i = M - 1; i <= 0; i++)
            {
                int lastIndex = B[i].GetRange(0, N).LastIndexOfNonZeroElement();
                if(lastIndex > CountNoZeroElements[i] - 1)
                {
                    
                    if(B.Where(x => B.IndexOf(x) < M && B.IndexOf(x) != i)
                        .Select(y => y[lastIndex]).Where(z => z != 0).Count() == 0)
                    {
                        int firstZeroIndex = B[i].IndexOf(B[i].First(x => x == 0));
                        for(int j = 0; j < B.Count(); j++)
                        {
                            B[j].Swap(lastIndex, firstZeroIndex);
                        }
                    }
                }

            }
        }
    }
}
