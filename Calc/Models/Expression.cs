using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;

namespace Calc.Models
{
    public class Expression
    {
        public Expression(string expression, ICalculate calc)
        {
            MathExpression = expression;
            Steps = MathExpression;
            if (calc is null)
            {
                Result = 0;
                ErrorMessage = "Null Calculate";
            }
            else
            {
                var res = calc.Parse(this);
                Result = res.Result;
                ErrorMessage = res.ErrorMessage;
            }
        }

        public Expression()
        {

        }
        public string Steps { get; set; }
        public string MathExpression { get; }
        public string Action { get; set; }
        public double Result { get; set; }
        public bool HasError => string.IsNullOrWhiteSpace(ErrorMessage) == false;
        public string ErrorMessage { get; set; }
    }
}
