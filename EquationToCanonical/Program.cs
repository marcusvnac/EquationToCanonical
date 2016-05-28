using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Equation To Canonical";

            Console.WriteLine("Choose operation mode:");
            Console.WriteLine("1: File");
            Console.WriteLine("2: Interactive");
            Console.WriteLine("==> ");

            int userOption;
            if (int.TryParse(Console.ReadLine().ToString(), out userOption))
            {
                Console.WriteLine();

                switch (userOption)
                {
                    case 1:

                        break;
                    case 2:
                        Console.WriteLine();
                        Console.WriteLine("***** Press CTRL+C to exit the program *****");
                        while (true)
                        {
                            Console.WriteLine();
                            Console.Write("Enter the equation: ");

                            string equation = Console.ReadLine();

                            if (String.IsNullOrEmpty(equation))
                                continue;

                            Console.WriteLine("Transformed Canonical Form: ");

                            Processor pr = new Processor();
                            try
                            {
                                string result = pr.TransformEquation(equation);
                                Console.Write(result);
                                Console.WriteLine();
                            }
                            catch (InvalidEquationException)
                            {
                                Console.WriteLine("=> Enter a valid equation <=");
                            }
                        }
                    default:
                        Console.WriteLine("Run the program again and choose an avaiable option");
                        break;
                }
            }
            else
                Console.WriteLine("Run the program again and choose an avaiable option");

            Console.ReadKey();
        }
    }
}
