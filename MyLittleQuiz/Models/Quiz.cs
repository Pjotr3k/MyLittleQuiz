using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Globalization;

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

        
        
        public Quiz AddQuiz(string name, User creator, string description = null)
        {
            User user = new User();
            string creationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            int creatorId = creator.UserId;
            Quiz quiz = new Quiz();

            
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

            quiz = quiz.ReturnCreatedQuiz(name, creatorId, creationTime);

            quiz.AddModerator(creatorId, true);

            return quiz;
        }

        public void AddModerator(int modId, bool isCreator = false)
        {
            string sqlQuery = $"INSERT INTO moderators(IdQuiz, IdUser, IsCreator) VALUES ({this.Id}, {modId}, {Convert.ToInt32(isCreator)})";

            SqlConnection con = new SqlConnection();
            MySqlDataAdapter adp = new MySqlDataAdapter();

            con.databaseConnection.Open();

            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            adp.InsertCommand = cmd;
            adp.InsertCommand.ExecuteNonQuery();
            con.databaseConnection.Close();

        }

        public Quiz ReturnCreatedQuiz(string name, int creatorId, string creationTime)
        {
            Quiz quiz = new Quiz();

            string sqlQuery = $"SELECT * FROM quizzes WHERE Name='{name}' AND CreatorId={creatorId} AND CreationDate='{creationTime}'";
            string datePattern = "yy-MM-ddyyyy-MM-dd HH:mm:ss";
            User user = new User();
            user = user.DoesExist(creatorId, null, null, null);

            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null;
            }

            if (dr.Read())
            {
                quiz.Id = Convert.ToInt32(dr["QuizId"]);
                quiz.Name = dr["Name"].ToString();
                quiz.Description = dr["Description"].ToString();
                quiz.Creator = user;
                quiz.IsPublic = Convert.ToBoolean(dr["IsPublic"]);
                quiz.CreationDate = DateTime.Parse(creationTime);
                quiz.LastModification = DateTime.Parse(creationTime);

                //quiz.CreationDate = DateTime.ParseExact(dr["CreationDate"].GetTime, datePattern, CultureInfo.InvariantCulture);
                //quiz.LastModification = DateTime.ParseExact(dr["LastModification"].ToString(), datePattern, CultureInfo.InvariantCulture);                
            }

            con.databaseConnection.Close();

            return quiz;
        }
    }
}