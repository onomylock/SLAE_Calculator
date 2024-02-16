using SLAE_Calculator.Solver;
using System.Collections.Generic;
using System.Data;

namespace SLAE_Calculator
{
    public static class Pragma
    {
        public static void Main(String[] args)
        {            

            var solver = new SolverSLAE();
            try
            {

                solver.ReadInput();
                solver.GenrateMatrixB();
                var result = solver.Solve();
                Console.WriteLine(result.Item1);
                result.Item2.ForEach(i => { i.ForEach(x => Console.Write("{0}\t", x)); Console.WriteLine(); });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());                
            }
            
        }
    }
}
