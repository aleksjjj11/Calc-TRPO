using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Models;

namespace Calc.Interfaces
{
    interface ICalculate
    {
        Expression Parse(string expression);
    }
}
