using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class SqlConnection
    {
        public SqlConnection()
        {
            connectionString = "datasource = 127.0.0.1; port=3306;username=root;password=;database=my_little_quiz;";
            databaseConnection = new MySqlConnection(connectionString);
        }

        string connectionString;

        public MySqlConnection databaseConnection;
    }
}
