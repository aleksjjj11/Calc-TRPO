using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;
using Newtonsoft.Json;

namespace Calc.Models
{
    public class MemoryJson : IMemory
    {
        private string _fileName;
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

        public MemoryJson(string fileName)
        {
            _fileName = fileName;
            if (File.Exists(_fileName) == false)
            {
                MemoryCollection = MemoryCollection ?? new ObservableCollection<double>();
                return;
            }

            var jsonText = File.ReadAllText(_fileName);
            MemoryCollection = JsonConvert.DeserializeObject<ObservableCollection<double>>(jsonText);
        }

        public void Add(double value)
        {
            MemoryCollection.Insert(0, value);
            Save();
        }

        public void Delete(int index)
        {
            MemoryCollection.RemoveAt(index);
            Save();
        }

        public void Increase(double value, int index)
        {
            MemoryCollection[index] += value;
            Save();
        }

        public void Decrease(double value, int index)
        {
            MemoryCollection[index] -= value;
            Save();
        }

        public void Clear()
        {
            MemoryCollection.Clear();
            Save();
        }

        public void Save()
        {
            var textToSave = JsonConvert.SerializeObject(MemoryCollection);
            File.WriteAllText(_fileName, textToSave);
        }
    }
}
