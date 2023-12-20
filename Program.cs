using SLAE_Calculator.Solver;
using System.Data;

namespace SLAE_Calculator
{
    public static class Pragma
    {
        public static void Main(String[] args)
        {
            

            var solver = new SolverSLAE();
            solver.ReadInput();
            solver.GenrateMatrixB();
            solver.Solve();


        }
    }
}
