using System.Collections.ObjectModel;
using System.Linq;
using Calc.Interfaces;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Calc.Models
{
    public class HistorySqLite : IHistory
    {
        private string _dbName;
        public ObservableCollection<Expression> HistoryCollection { get; }

        public HistorySqLite(string dbName)
        {
            _dbName = dbName;
            HistoryCollection = new ObservableCollection<Expression>();
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var expressions = connection.Query<Expression>("select * from Expressions");
                if (expressions.Any())
                {
                    foreach (var expression in expressions)
                    {
                        HistoryCollection.Add(expression);
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
                var expressions = connection.Query<Expression>($"select *,Id={index + 1} from Expressions");
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
                var expressions = connection.Query<Expression>("select * from Expressions",
                    new DynamicParameters());
                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public void Add(Expression expression)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText =
                    @"Insert into Expressions(Action, MathExpression, Result, ErrorMessage, HasError, Steps) 
                    Values(@Action, @MathExpression, @Result, @ErrorMessage, @HasError, @Steps)";
                command.Parameters.AddWithValue("@Action", expression.Action);
                command.Parameters.AddWithValue("@MathExpression", expression.MathExpression);
                command.Parameters.AddWithValue("@Result", expression.Result);
                command.Parameters.AddWithValue("@ErrorMessage", expression.ErrorMessage);
                command.Parameters.AddWithValue("@HasError", expression.HasError);
                command.Parameters.AddWithValue("@Steps", expression.Steps);

                command.ExecuteNonQuery();
                HistoryCollection.Add(expression);
            }
        }

        public void Delete(int index)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions Where Id = {index + 1}");
                HistoryCollection.RemoveAt(index);
            }
        }

        public void Clear()
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbName};Version=3;"))
            {
                connection.Open();
                connection.Query($"Delete from Expressions");
                HistoryCollection.Clear();
            }
        }
    }
}
