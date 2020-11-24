using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;

namespace Calc.Models
{
    public class HistorySqLite : IHistory
    {
        private string _dbName;
        public ObservableCollection<Expression> HistoryCollection { get; }

        public HistorySqLite(string dbName)
        {
            _dbName = dbName;
        }
        public bool TryAdd()
        {
            throw new NotImplementedException();
        }

        public bool TryDelete(int index)
        {
            throw new NotImplementedException();
        }

        public bool TryClear()
        {
            throw new NotImplementedException();
        }

        public void Add(Expression expression)
        {
            throw new NotImplementedException();
        }

        public void Delete(int index)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
