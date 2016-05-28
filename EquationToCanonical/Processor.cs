using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
    /// <summary>
    /// Process an equation to transform it into a canonical form
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// Type to store a summand information
        /// </summary>
        private class Summand
        {
            public double coefficient { get; set; }

            public string variable { get; set; }

            public int exponential { get; set; }
        }        

        private static char PLUS_OPERATOR = '+';
        private static char MINUS_OPERATOR = '-';
        private static char EQUAL_OPERATOR = '=';
        
        private List<Summand> SummandsList;

        public Processor()
        {
            SummandsList = new List<Summand>();
        }

        /// <summary>
        /// Transform a given equation into its canonical form.
        /// </summary>
        /// <param name="equation">Equation to be transformed</param>
        /// <returns><code>String</code> with a canonical for of the given equation</returns>
        public string TransformEquation(string equation)
        {            
            IdentifySummands(equation);
            if (!IsValidSummands(SummandsList))
                throw new InvalidEquationException();

            return BuildCanonicalString();
        }

        /// <summary>
        /// Reads the <code>Summand</code> list and transforms it into a canonical string
        /// </summary>
        /// <returns><code>String</code> with the canonical form</returns>
        private string BuildCanonicalString()
        {
            StringBuilder result = new StringBuilder();
            List<Summand> orderedList = SummandsList.OrderByDescending(x => x.exponential).ToList<Summand>();

            foreach (var summand in orderedList)
            {
                if (summand.coefficient != 0)
                {
                    if (result.Length > 0)
                        result.Append(" ");

                    if (summand.coefficient > 0)
                    {
                        if (result.Length > 0)
                            result.Append("+ ");
                        if (summand.coefficient > 1)
                            result.Append(summand.coefficient.ToString(CultureInfo.InvariantCulture));
                    }

                    else if (summand.coefficient < 0)
                    {
                        if (result.Length > 0)
                            result.Append("- ");
                        else
                            result.Append("-");

                        if (summand.coefficient < -1)
                        {
                            summand.coefficient = summand.coefficient * -1;
                            result.Append(summand.coefficient.ToString(CultureInfo.InvariantCulture));
                        }
                    }

                    if (!String.IsNullOrEmpty(summand.variable))
                        result.Append(summand.variable);

                    if (summand.exponential != 1)
                    {
                        result.Append("^");
                        result.Append(summand.exponential.ToString());
                    }
                }
            }
            result.Append(" = 0");

            return result.ToString();
        }

        /// <summary>
        /// Validade if the summands of the equation are valid
        /// </summary>
        /// <param name="summandsList">List of summands of the equation</param>
        /// <returns><code>True</code> if the summands are valid, <code>False</code> otherwise</returns>
        private bool IsValidSummands(List<Summand> summandsList)
        {
            foreach(var summand in summandsList)
            {
                if (!String.IsNullOrEmpty(summand.variable))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Reads the equation and identifies the summands, creating a list of the summands identified
        /// </summary>
        /// <param name="equation"><code>String</code> with given equation</param>
        private void IdentifySummands(string equation)
        {
            string coefficient = "";
            string variable = "";
            string exponential = "";
            char op = '0';
            bool passEqual = false;
            char item = '0';

            for (int i=0; i<equation.Length; i++)
            {
                item = equation[i];  

                if (Char.IsWhiteSpace(item) || IsBracket(item))
                {
                    if (!op.Equals('0') && (String.IsNullOrEmpty(coefficient) && String.IsNullOrEmpty(variable) && String.IsNullOrEmpty(exponential)))
                        continue;

                    AddSummandToList(BuildSummand(coefficient, exponential, variable, op, passEqual));

                    coefficient = "";
                    variable = "";
                    exponential = "";
                    op = '0';

                    continue;
                }

                if (IsOperatorSymbol(item))
                {
                    if (EQUAL_OPERATOR.Equals(item))
                        passEqual = true;
                    
                    op = item;
                    continue;
                }

                if ((Char.IsNumber(item) || Char.IsPunctuation(item)) && (String.IsNullOrEmpty(variable)))
                    coefficient = coefficient + item;
                else if (Char.IsLetter(item))
                    variable = variable + item;
                else if (Char.IsSymbol(item) || (!String.IsNullOrEmpty(variable)))
                {
                    i++;
                    exponential = equation[i].ToString();
                }
                else if (Char.IsNumber(item))
                    exponential = exponential + item;                
            }

            AddSummandToList(BuildSummand(coefficient, exponential, variable, op, passEqual));
        }

        /// <summary>
        /// Builds a <code>Summand</code> object with the information read from the equation string
        /// </summary>
        /// <param name="coefficient">Coefficient of the summand</param>
        /// <param name="exponential">Exponent of the summant</param>
        /// <param name="variable">Variable of the summand</param>
        /// <param name="op">Operation. Can be a sum (+) or subtration (-)</param>
        /// <param name="passEqual">Informs if this summand is on the right or left side of the equality</param>
        /// <returns></returns>
        private Summand BuildSummand(string coefficient, string exponential, string variable, char op, bool passEqual)
        {
            if (String.IsNullOrEmpty(coefficient))
                coefficient = "1";
            if (String.IsNullOrEmpty(exponential))
                exponential = "1";

            Summand summand = new Summand { coefficient = Double.Parse(coefficient, CultureInfo.InvariantCulture), exponential = int.Parse(exponential), variable = variable };

            if (MINUS_OPERATOR.Equals(op))
                summand.coefficient = summand.coefficient * -1;

            if (passEqual)
                summand.coefficient = summand.coefficient * -1;

            return summand;
        }

        /// <summary>
        /// Adds a <code>Summand</code> object to the SummandsList, verifying if there is another variable already in list to the sum or subtracted.
        /// </summary>
        /// <param name="summand"><code>Summand</code> object to be added to the list</param>
        private void AddSummandToList(Summand summand)
        {
            var found = SummandsList.Find(x => x.variable.Equals(summand.variable) && x.exponential == summand.exponential);

            if (found != null)
                found.coefficient += summand.coefficient;            
            else
                SummandsList.Add(summand);
        }

        /// <summary>
        /// Verify if a character read from the equation string is a operator symbol, like equals (=), minus (-) or plus (+)
        /// </summary>
        /// <param name="item">Character read from the equation string</param>
        /// <returns></returns>
        private static bool IsOperatorSymbol(char item)
        {
            return (PLUS_OPERATOR.Equals(item)
                    || MINUS_OPERATOR.Equals(item)
                    || EQUAL_OPERATOR.Equals(item));
        }

        /// <summary>
        /// Verify if a character read from the equation string is a bracket
        /// </summary>
        /// <param name="item">Character read from the equation string</param>
        /// <returns></returns>
        private static bool IsBracket(char item)
        {
            return ('('.Equals(item)
                    || ')'.Equals(item)
                    || ']'.Equals(item)
                    || '['.Equals(item));
        }
    }
}
