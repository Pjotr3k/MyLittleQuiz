using System.Data;
using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public User Creator { get; set; }
        public List<User> Moderators { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModification { get; set; }

        public static void AddQuiz(string name, User creator, string description = null)
        {
            User user = new User();
            string creationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            int creatorId = creator.UserId;

            
            string sqlQuery;

            if (description != null)
            {
                sqlQuery = $"INSERT INTO `quizzes`(Name, Description, IsPublic, CreatorId, CreationDate, LastModification) " +
                    $"VALUES ('{name}','{description}', 0, {creatorId}, '{creationTime}', '{creationTime}')";
            }
            else
            {
                sqlQuery = $"INSERT INTO quizzes (Name, IsPublic, CreatorId, CreationDate, LastModification) " +
                    $"VALUES ('{name}', 0, {creatorId}, '{creationTime}', '{creationTime}')";
            }

            SqlConnection con = new SqlConnection();
            MySqlDataAdapter adp = new MySqlDataAdapter();

            con.databaseConnection.Open();

            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            adp.InsertCommand = cmd;
            adp.InsertCommand.ExecuteNonQuery();
            con.databaseConnection.Close();            
        }
    }
}