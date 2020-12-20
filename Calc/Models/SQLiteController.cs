using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Calc.Models
{
    public class SqLiteController
    {
        protected string DbName;

        public SqLiteController(string dbName)
        {
            DbName = dbName;
        }

        public object Request<T>(string query, SQLiteCommand command = null)
        {
            using (var connection = new SQLiteConnection($"Data Source={DbName};Version=3;"))
            {
                connection.Open();
                if (command is not null)
                {
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    return null;
                }

                try
                {
                    return connection.Query<T>(query);
                }
                catch (SQLiteException exception)
                {
                    //"SQL logic error\r\nno such table: ";
                    if (exception.Message.Contains("no such table: Expressions"))
                    {
                        connection.Query(@"create table Expressions
                            (
                                Id             INTEGER not null
                                    primary key autoincrement,
                                Action         CHAR [200],
                                MathExpression CHAR [200],
                                Result         DOUBLE default 0,
                                ErrorMessage   CHAR [150],
                                HasError       BOOLEAN,
                                Steps          STRING
                            );");
                    }

                    if (exception.Message.Contains("no such table: Memory"))
                    {
                        connection.Query(@"create table Memory
                                            (
                                                Id    INTEGER
                                                    primary key autoincrement,
                                                Value DOUBLE
                                            );"
                                        );
                    }
                    
                    return connection.Query<T>(query);

                }
            }
        }
        public object Request(string query, SQLiteCommand command = null)
        {
            using (var connection = new SQLiteConnection($"Data Source={DbName};Version=3;"))
            {
                connection.Open();
                if (command is not null)
                {
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    return null;
                }

                return connection.Query(query);
            }
        }
        public void Request(SQLiteCommand command)
        {
            using (var connection = new SQLiteConnection($"Data Source={DbName};Version=3;"))
            {
                connection.Open();
                if (command is not null)
                {
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
