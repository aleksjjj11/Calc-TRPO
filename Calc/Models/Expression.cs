using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class Expression
    {
        public Expression(double result, string errorMessage = "")
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public string Action { get; set; }
        public double Result { get; }
        public bool HasError => string.IsNullOrWhiteSpace(ErrorMessage) == false;
        public string ErrorMessage { get;  }
    }
}
