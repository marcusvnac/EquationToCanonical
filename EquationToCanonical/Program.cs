using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
    class Program
    {
        static void Main(string[] args)
        {
            // Validate interactive or file mode
            if (args.Length == 0)
            {
                Console.Title = "Equation To Canonical";

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
            }
            else
            {
                string filename = args[0];
                string line, result;

                StreamReader inputFile = new StreamReader(filename);
                StreamWriter outputFile = new StreamWriter(filename + ".out");

                Console.WriteLine("Processing file...");

                while ((line = inputFile.ReadLine()) != null)
                {
                    Processor pr = new Processor();
                    result = pr.TransformEquation(line);

                    outputFile.WriteLine(result);
                }
                inputFile.Close();
                outputFile.Flush();
                outputFile.Close();

                Console.WriteLine("File processing finished");
            }
        }
    }
}
