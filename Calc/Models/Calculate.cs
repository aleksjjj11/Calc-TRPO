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
                char? operation = null;
                foreach (var symbol in expression)
                {
                    if (operation != null) break;

                    if (symbol >= '0' && symbol <= '9' || symbol == ',')
                        leftValue += symbol;
                    else if (symbol == '+' || symbol == '*' || symbol == '/' || symbol == '-')
                        operation = symbol;
                }

                if (operation is null) return Convert.ToDouble(leftValue);

                expression = expression.Remove(0, expression.IndexOf((char)operation) + 1);

                foreach (var symbol in expression)
                {
                    if (symbol >= '0' && symbol <= '9' || symbol == ',')
                        rightValue += symbol;
                    else
                        break;
                }

                if (rightValue == "")
                    return Convert.ToDouble(leftValue);

                expression = expression.Remove(0, rightValue.Length);

                switch (operation)
                {
                    case '-':
                        {
                            return Parse((Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue)) + expression);
                        }
                    case '+':
                        {
                            return Parse((Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue)) + expression);
                        }
                    case '*':
                        {
                            return Parse((Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue)) + expression);
                        }
                    case '/':
                        {
                            return Parse((Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue)) + expression);
                        }
                    default: throw new Exception("We have problem");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Interesting problem");
            }
        }
    }
}
