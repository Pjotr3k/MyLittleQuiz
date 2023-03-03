using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Security.Claims;

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
        public ClaimsPrincipal Principal { get; set; }



        public Quiz AddQuiz(string name, User creator, string description = null)
        {
            User user = new User();
            DateTime fullCreationTime = DateTime.Now;
            string creationTime = fullCreationTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            int creatorId = creator.UserId;
            Quiz quiz = new Quiz();

            
            string sqlQuery;

            if (description != null)
            {
                sqlQuery = $"INSERT INTO `quizzes`(Name, Description, IsPublic, CreatorId, CreationDate, LastModification) " +
                    $"VALUES ('{name}', '{description}', 0, {creatorId}, '{creationTime}', '{creationTime}')";
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

            quiz = quiz.ReturnCreatedQuiz(name, creatorId, fullCreationTime.ToString("yyyy-MM-dd HH:mm:ss"));

            //quiz.AddModerator(creatorId, true);

            return quiz.GetQuizById(quiz.Id);
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
            string datePattern = "yyyy-MM-dd HH:mm:ss";
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

        public List<Quiz> GetAllQuizzes()
        {
            List<Quiz> quizzes = new List<Quiz>();

            string sqlQuery = $"SELECT * FROM quizzes";
            User user = new User();
            //Quiz quiz = new Quiz();

            User currentUser = new User();
            currentUser.Principal = this.Principal;
            currentUser = currentUser.GetUserByClaims();

            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null;
            }

            while (dr.Read())
            {
                Quiz quiz = new Quiz();

                quiz.Id = Convert.ToInt32(dr["QuizId"]);
                quiz.Name = dr["Name"].ToString();
                quiz.Description = dr["Description"].ToString();
                int creatorId = Convert.ToInt32(dr["CreatorId"]);
                quiz.Creator = user.DoesExist(creatorId, null, null, null);
                quiz.IsPublic = Convert.ToBoolean(dr["IsPublic"]);
                quiz.CreationDate = DateTime.Parse(dr["CreationDate"].ToString());
                quiz.LastModification = DateTime.Parse(dr["LastModification"].ToString());

                quiz.Moderators = quiz.PopulateModerators();

                if (quiz.IsPublic) quizzes.Add(quiz);
                else if (quiz.Moderators.Any(m => m.UserId == currentUser.UserId)) quizzes.Add(quiz);

            }

            return quizzes;
        }

        public Quiz GetQuizById(int quizId)
        {
            string sqlQuery = $"SELECT * FROM quizzes WHERE QuizId='{quizId}'";
            User user = new User();
            Quiz quiz = new Quiz();

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
                int creatorId = Convert.ToInt32(dr["CreatorId"]);
                quiz.Creator = user.DoesExist(creatorId, null, null, null);
                quiz.IsPublic = Convert.ToBoolean(dr["IsPublic"]);
                quiz.CreationDate = DateTime.Parse(dr["CreationDate"].ToString());
                quiz.LastModification = DateTime.Parse(dr["LastModification"].ToString());
            }

            con.databaseConnection.Close();

            quiz.Moderators = quiz.PopulateModerators();

            return quiz;
        }

        //---------***-MODERATORS RELATED METHODS-***------------------------

        public List<User> PopulateModerators()
        {
            List<User> mods = new List<User>();
            string sqlQuery = $"SELECT * FROM moderators WHERE IdQuiz='{this.Id}'";

            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null;
            }

            while (dr.Read())
            {
                User u = new User();
                int modId = Convert.ToInt32(dr["IdUser"]);
                u = u.DoesExist(modId, null, null, null);
                mods.Add(u);
            }

            return mods;
        }

        public void DeleteModerator(int modId)
        {
            string sqlQuery = $"DELETE FROM moderators WHERE IdQuiz={this.Id} AND IdUser={modId} AND IsCreator=0";

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