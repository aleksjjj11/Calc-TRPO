using System.Collections.ObjectModel;
using Calc.Interfaces;

namespace Calc.Models
{
    public class History: IHistory
    {
        public History()
        {
            HistoryCollection = new ObservableCollection<Expression>();
        }

        public ObservableCollection<Expression> HistoryCollection { get; }
        public bool TryAdd()
        {
            return HistoryCollection is null == false;
        }

        public bool TryDelete(int index)
        {
            return index > -1 && index < HistoryCollection?.Count;
        }

        public bool TryClear()
        {
            return HistoryCollection?.Count > 0;
        }

        public void Add(Expression expression)
        {
            HistoryCollection.Insert(0, expression);
        }

        public void Delete(int index)
        {
            HistoryCollection.RemoveAt(index);
        }

        public void Clear()
        {
            HistoryCollection.Clear();
        }
    }
}
