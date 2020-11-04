using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public static class Calculate
    {
        public static double Parse(string expression)
        {
            try
            {
                expression = expression.Replace(" ", "").Replace(".", ",");
                string leftValue = "", rightValue = "";
                int i = 0, lessIndexOperation = -1;
                char? operation = null;
                if (expression.Contains("*") && expression.Contains("/"))
                {
                    lessIndexOperation = Math.Min(expression.IndexOf("*"), expression.IndexOf("/"));
                } 
                else if (expression.Contains("*"))
                {
                    lessIndexOperation = expression.IndexOf("*");
                }
                else if (expression.Contains("/"))
                {
                    lessIndexOperation = expression.IndexOf("/");
                } 
                else if (expression.Contains("+") && expression.Contains("-"))
                {
                    lessIndexOperation = Math.Min(expression.IndexOf("+"), expression.LastIndexOf("-"));
                }
                else if (expression.Contains("+"))
                {
                    lessIndexOperation = expression.IndexOf("+");
                }
                else if (expression.Contains("-"))
                {
                    lessIndexOperation = expression.LastIndexOf("-");
                    if (lessIndexOperation == 0)
                        return Convert.ToDouble(expression);
                }
                
                if (lessIndexOperation == -1)
                    return Convert.ToDouble(expression);

                leftValue = FindLeftValue(expression, lessIndexOperation);
                rightValue = FindRightValue(expression, lessIndexOperation);
                operation = expression[lessIndexOperation];
                    
                if (rightValue == "" || operation is null)
                    return Convert.ToDouble(leftValue);
                
                int removeIndex = lessIndexOperation - leftValue.Length;
                expression = expression.Remove(removeIndex, leftValue.Length + rightValue.Length + 1);
                double result;
                
                switch (operation)
                {
                    case '-':
                        {
                            result = Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
                            break;
                        }
                    case '+':
                        {
                            result = Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue);
                            break;
                        }
                    case '*':
                        {
                            result = Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue);
                            break;
                        }
                    case '/':
                        {
                            result = Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue);
                            break;
                        }
                    default: throw new Exception("We have problem");
                }

                return Parse(expression.Insert(removeIndex, result.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
        private static string FindLeftValue(string expression, int indexOperation)
        {
            string res = "";

            for (int i = indexOperation - 1; i >= 0; i--)
            {
                char symbol = expression[i];
                if (symbol >= '0' && symbol <= '9' || symbol == ',')
                    res += symbol;
                else if (symbol == '-')
                {
                    res += symbol;
                    break;
                }
                else break;
            }
            
            return new string(res.ToCharArray().Reverse().ToArray());
        }

        private static string FindRightValue(string expression, int indexOperation)
        {
            string res = "";

            for (int i = indexOperation + 1; i < expression.Length; i++)
            {
                char symbol = expression[i];
                if (symbol >= '0' && symbol <= '9' || symbol == ',')
                    res += symbol;
                else break;
            }
            
            return res;
        }
    }
}
