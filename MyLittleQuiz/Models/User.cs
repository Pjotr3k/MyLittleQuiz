using MySql.Data.MySqlClient;

namespace MyLittleQuiz.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public User LogIn(string login, string password)
        {
            SqlConnection con = new SqlConnection();
            con.databaseConnection.Open();
            string sqlQuery = $"SELECT idLogon, login, email FROM logons WHERE login='{login}' AND password='{password}'";

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
    }
}
