using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace MyLittleQuiz.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public User DoesExist(int userId, string login, string password, string email, bool allOfThem = true)
        {
            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            string AndOr;

            if (allOfThem) AndOr = " AND ";
            else AndOr = " OR ";

            string sqlQuery = $"SELECT idLogon, login, email FROM logons WHERE ";

            if (userId != 0) sqlQuery += $"idLogon='{userId}'";
            if (login != null) 
            {
                if (userId != 0) sqlQuery += AndOr;
                sqlQuery += $"login='{login}'";
            }
            if (password != null)
            {
                if (userId != 0 || login != null) sqlQuery += AndOr;
                sqlQuery += $"password='{password}'";
            }
            if (email != null)
            {
                if (userId != 0 || login != null || password != null) sqlQuery += AndOr;
                sqlQuery += $"email='{email}'";
            }


            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                con.databaseConnection.Close();
                return null; 
            }

            User user = new User();

            if(dr.Read())
            {
                user.UserId = Convert.ToInt32(dr["idLogon"]);
                user.Login = dr["login"].ToString();
                user.Email = dr["email"].ToString();
            }

            con.databaseConnection.Close();

            return user;
        }

        public User SignUp(string login, string password, string email)
        {
            User user = new User();

            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            string sqlQuery = $"SELECT login, email FROM logons WHERE login='{login}' OR email='{email}'";

            MySqlCommand cmd = new MySqlCommand(sqlQuery, con.databaseConnection);
            MySqlDataReader dr = cmd.ExecuteReader();

            return user;
        }

        
    }
}
