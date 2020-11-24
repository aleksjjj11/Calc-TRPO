using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Models;

namespace Calc.Interfaces
{
    public interface ICalculate
    {
        Expression Parse(Expression expression);
    }
}
