using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Calc.Interfaces;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace Calc.Models
{
    public class HistorySqLite : SqLiteController, IHistory
    {
        public ObservableCollection<Expression> HistoryCollection { get; }

        public HistorySqLite(string dbName) : base(dbName)
        {
            DbName = dbName;
            HistoryCollection = new ObservableCollection<Expression>();
            var expressions = this.Request<Expression>("select * from Expressions") as IEnumerable<Expression>;

            if (expressions.Any())
            {
                foreach (var expression in expressions)
                {
                    HistoryCollection.Add(expression);
                }
            }
        }

        public bool TryAdd()
        {
            return File.Exists(DbName);
        }

        public bool TryDelete(int index)
        {
            var expressions = this.Request($"select Id={index + 1} from Expressions");
            if (expressions is not null)
                return true;

            return false;
        }

        public bool TryClear()
        {
            using (var connection = new SQLiteConnection($"Data Source={DbName};Version=3;"))
            {
                connection.Open();
                var expressions = this.Request<Expression>("select * from Expressions") as IEnumerable<Expression>;
                if (expressions.Any())
                    return true;
            }

            return false;
        }

        public void Add(Expression expression)
        {
            var command = new SQLiteCommand();
            command.CommandText =
                @"Insert into Expressions(Action, MathExpression, Result, ErrorMessage, HasError, Steps) 
                    Values(@Action, @MathExpression, @Result, @ErrorMessage, @HasError, @Steps)";
            command.Parameters.AddWithValue("@Action", expression.Action);
            command.Parameters.AddWithValue("@MathExpression", expression.MathExpression);
            command.Parameters.AddWithValue("@Result", expression.Result);
            command.Parameters.AddWithValue("@ErrorMessage", expression.ErrorMessage);
            command.Parameters.AddWithValue("@HasError", expression.HasError);
            command.Parameters.AddWithValue("@Steps", expression.Steps);
            this.Request(command);
            HistoryCollection.Add(expression);
        }

        public void Delete(int index)
        {
            this.Request($"Delete from Expressions Where Id = {index + 1}");
            HistoryCollection.RemoveAt(index);
        }

        public void Clear()
        {
            this.Request($"Delete from Expressions");
            HistoryCollection.Clear();
        }
    }
}
