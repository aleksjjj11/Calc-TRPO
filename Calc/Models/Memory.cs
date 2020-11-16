using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;

namespace Calc.Models
{
    class Memory : IMemory
    {
        public ObservableCollection<double> MemoryCollection { get; }
        public bool TryAdd()
        {
            return MemoryCollection != null;
        }

        public bool TryDelete(int index)
        {
            return index > -1 && index < MemoryCollection.Count;
        }

        public bool TryIncrease(int index)
        {
            return index > -1 && index < MemoryCollection.Count;
        }

        public bool TryDecrease(int index)
        {
            return index > -1 && index < MemoryCollection.Count;
        }

        public bool TryClear()
        {
            return MemoryCollection.Count > 0;
        }

        public Memory()
        {
            MemoryCollection = MemoryCollection ?? new ObservableCollection<double>();
        }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
        }

        public void Delete(int index)
        {
            MemoryCollection.RemoveAt(index);
        }

        public void Increase(double value, int index)
        {
            MemoryCollection[index] += value;
        }

        public void Decrease(double value, int index)
        {
            MemoryCollection[index] -= value;
        }

        public void Clear()
        {
            MemoryCollection.Clear();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
