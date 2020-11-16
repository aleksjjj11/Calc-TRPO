using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Interfaces
{
    interface IMemory
    {
        ObservableCollection<double> MemoryCollection { get; }
        //canAdd canDelete canClear ...
        bool TryAdd();
        bool TryDelete(int index);
        bool TryIncrease(int index);
        bool TryDecrease(int index);
        bool TryClear();

        void Add(double value);
        void Delete(int index);
        void Increase(double value, int index);
        void Decrease(double value, int index);
        void Clear();
        void Save();

    }
}
