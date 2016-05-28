using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
    public class Processor
    {
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

        public string TransformEquation(string equation)
        {            
            IdentifySummands(equation);
            if (!IsValidSummands(SummandsList))
                throw new InvalidEquationException();

            return BuildCanonicalString();
        }

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

        private bool IsValidSummands(List<Summand> summandsList)
        {
            foreach(var summand in summandsList)
            {
                if (!String.IsNullOrEmpty(summand.variable))
                    return true;
            }
            return false;
        }

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

                if (Char.IsWhiteSpace(item))
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

        private void AddSummandToList(Summand summand)
        {
            var found = SummandsList.Find(x => x.variable.Equals(summand.variable) && x.exponential == summand.exponential);

            if (found != null)
                found.coefficient += summand.coefficient;            
            else
                SummandsList.Add(summand);
        }

        private bool IsOperatorSymbol(char item)
        {
            return (PLUS_OPERATOR.Equals(item)
                    || MINUS_OPERATOR.Equals(item)
                    || EQUAL_OPERATOR.Equals(item));
        }
    }
}
