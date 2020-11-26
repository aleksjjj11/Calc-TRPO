using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Interfaces;
using Dapper;

namespace Calc.Models
{
    public class MemorySQLite : IMemory
    {
        class RowMemory
        {
            public int Id { get; set; }
            public double Value { get; set; }
        }
        private string _dbName;
        public ObservableCollection<double> MemoryCollection { get; }

        public MemorySQLite(string dbName)
        {
            _dbName = dbName;
            MemoryCollection = new ObservableCollection<double>();
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var rows = connection.Query<RowMemory>("select * from Memory");
                if (rows.Any())
                {
                    foreach (var row in rows)
                    {
                        MemoryCollection.Add(row.Value);
                    }
                }
            }
        }
        public bool TryAdd()
        {
            return File.Exists(_dbName);
        }

        public bool TryDelete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<RowMemory>($"select *,Id={index + 1} from Memory");

                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public bool TryIncrease(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<RowMemory>($"select *,Id={index + 1} from Memory");

                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public bool TryDecrease(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<RowMemory>($"select *,Id={index+1} from Memory");
                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public bool TryClear()
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<RowMemory>("select * from Memory",
                    new DynamicParameters());
                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public void Add(double value)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Memory(Value) 
                    Values(@Value)";
                command.Parameters.AddWithValue("@Value", value);

                command.ExecuteNonQuery();
                MemoryCollection.Add(value);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Memory Where Id = {index + 1}");
                MemoryCollection.RemoveAt(index);
            }
        }

        public void Increase(double value, int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = $"Update Memory SET Value={MemoryCollection[index] + value} Where Id={index + 1}";
                command.ExecuteNonQuery();
                MemoryCollection[index] += value;
            }
        }

        public void Decrease(double value, int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = $"Update Memory SET Value={MemoryCollection[index] - value} Where Id={index + 1}";
                command.ExecuteNonQuery();
                MemoryCollection[index] -= value;
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Memory");
                MemoryCollection.Clear();
            }
        }
    }
}
