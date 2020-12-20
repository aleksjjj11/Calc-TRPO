using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;
using Dapper;

namespace Calc.Models
{
    public class MemorySQLite : SqLiteController, IMemory
    {
        class RowMemory
        {
            public int Id { get; set; }
            public double Value { get; set; }
        }
        private string _dbName;
        public ObservableCollection<double> MemoryCollection { get; }
        private IEnumerable<RowMemory> _rowMemories;

        public MemorySQLite(string dbName) : base(dbName)
        {
            _dbName = dbName;
            MemoryCollection = new ObservableCollection<double>();
            UpdateInfo();
        }
        public bool TryAdd()
        {
            return File.Exists(_dbName);
        }

        public bool TryDelete(int index)
        {
            try
            {
                var expressions = this.Request($"select Id={_rowMemories.ToArray()[index].Id} from Memory");

                if (expressions is not null)
                    return true;

                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TryIncrease(int index)
        {
            try
            {
                var expressions =
                    this.Request<RowMemory>($"select Id={_rowMemories.ToArray()[index].Id} from Memory") as
                        IEnumerable<RowMemory>;

                if (expressions.Any())
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TryDecrease(int index)
        {
            try
            {
                var expressions = this.Request<RowMemory>($"select Id={_rowMemories.ToArray()[index].Id} from Memory") as IEnumerable<RowMemory>;
                if (expressions.Any())
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TryClear()
        {
            var expressions = this.Request<RowMemory>("select * from Memory") as IEnumerable<RowMemory>;
            if (expressions.Any())
                return true;

            return false;
        }

        public void Add(double value)
        {
            var command = new SQLiteCommand();
            command.CommandText =
                @"Insert into Memory(Value) 
					Values(@Value)";
            command.Parameters.AddWithValue("@Value", value);
            this.Request(command);
            UpdateInfo();
        }

        public void Delete(int index)
        {
            this.Request($"Delete from Memory Where Id = {_rowMemories.ToArray()[index].Id}");
            UpdateInfo();
        }

        public void Increase(double value, int index)
        {
            var command = new SQLiteCommand();
            command.CommandText = $"Update Memory SET Value={MemoryCollection[index] + value} Where Id={_rowMemories.ToArray()[index].Id}";
            this.Request(command);
            MemoryCollection[index] += value;
        }

        public void Decrease(double value, int index)
        {
            var command = new SQLiteCommand();
            command.CommandText = $"Update Memory SET Value={MemoryCollection[index] - value} Where Id={_rowMemories.ToArray()[index].Id}";
            this.Request(command);
            MemoryCollection[index] -= value;
        }

        public void Clear()
        {
            this.Request($"Delete from Memory");
            MemoryCollection.Clear();
        }

        public void UpdateInfo()
        {
            MemoryCollection.Clear();
            _rowMemories = this.Request<RowMemory>("select * from Memory") as IEnumerable<RowMemory>;
            if (_rowMemories.Any())
            {
                foreach (var row in _rowMemories)
                {
                    MemoryCollection.Add(row.Value);
                }
            }
        }
    }
}
