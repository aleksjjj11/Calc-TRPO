using System;
using System.Collections.ObjectModel;
using Calc.Models;

namespace Calc.Interfaces
{
    public interface IHistory
    {
        ObservableCollection<Expression> HistoryCollection { get; }
        bool TryAdd();
        bool TryDelete(int index);
        bool TryClear();
        void Add(Expression expression);
        void Delete(int index);
        void Clear();
    }
}
