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

        public (int, List<List<int>>) Solve()
        {
            int K = 0;
            List<List<int>> res = new List<List<int>>();            

            for(int k = 0; k < M; k++)
            {
                CountNoZeroElements.Add(B[k].GetRange(0, B[k].Count() - 1).Where(x => x != 0).Count());                
            }
            

            for(int i = 0; i < M; i++)
            {
                while (CountNoZeroElements[i] > i + 1)
                {
                    var minIndex = B[i].GetRange(0, B[i].Count() - 1).GetMinIndex(i);
                    var valIndex = B[i].GetRange(0, B[i].Count() - 1).GetLastNoZeroElementIndex(minIndex);
                    OperationSub(i, valIndex, minIndex);
                }

                ColumnSwapper(i);                                     
            }



            for (int i = 0; i < M; i++)
            {
                var minIndex = B[i].GetRange(0, B[i].Count() - 1).GetMinIndex(0);
                OperationSub(i, B[i].Count() - 1, minIndex);
                if (B[i].Last() != 0)
                    throw new Exception("NO SOLUTION");
            }

            K = N - CountNoZeroElements.Max() - 1;          

            for(int i = 0; i < N; i++)
            {
                res.Add(new List<int>());
                for(int j = 0; j <= K; j++)
                {
                    res[i].Add(B[M + i][N + j - 1]);
                }
                
            }

            return (K, res);
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

        private void OperationSub(int lineIndex, int valIndex, int minIndex)
        {
            if (valIndex != -1 && minIndex != -1)
            {
                int r = B[lineIndex][valIndex] % B[lineIndex][minIndex];
                int q = (B[lineIndex][valIndex] - r) / B[lineIndex][minIndex];

                for (int j = 0; j < B.Count; j++)
                {
                    if (B[j][valIndex] == 0 && q * B[j][minIndex] != 0 && j < M)
                        CountNoZeroElements[j]++;
                    else if(B[j][valIndex] != 0 && B[j][valIndex] - q * B[j][minIndex] == 0)
                        CountNoZeroElements[j]--;
                    B[j][valIndex] -= q * B[j][minIndex];
                }
            }
        }

        private void ColumnSwapper(int index)
        {           
            var flags = B[index].GetRange(0, N).GetMaskElements();

            for (int i = 0; i < flags.Count(); i++)
            {
                var range = flags.GetRange(0, i);

                if (flags[i] && range.Contains(false) && i != index)
                {
                    var swapIndex = flags.IndexOf(range.First(x => x == false));
                    flags.Swap(i, swapIndex);
                    for(int j = 0; j < B.Count(); j++)
                    {
                        B[j].Swap(i, swapIndex);
                    }
                }                
            }
        }
    }
}
