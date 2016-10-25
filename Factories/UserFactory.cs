using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;

namespace theWall.Factory
{
    public class UserFactory : IFactory<User>
    {
        private string connectionString;

        public UserFactory()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=thewall;SslMode=None";
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(connectionString);
            }
        }

        public void Add(User NewUser)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, NewUser);
            }
        }
        public void AddMessage(int id, string message)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO messages (message, created_at, updated_at, user_id) VALUES ('" + message + "', NOW(), NOW(),'" + id + "')";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

        public void AddComment(int id, int messageId, string comment)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =  "INSERT INTO comments (comment, created_at, updated_at, user_id, message_id) VALUES ('" + comment + "', NOW(), NOW(),'" + id + "', '" + messageId +"')";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }


        public User FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users WHERE email = '" + email + "'").FirstOrDefault();
            }
        }

        public IEnumerable<Message> FindMessages()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Message>("SELECT message, messages.id, messages.created_at, messages.updated_at, users.first_name FROM messages JOIN users on users.id = messages.user_id");
            }
        }

        public IEnumerable<Comment> FindComments()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Comment>("SELECT comment, users.first_name, comments.created_at, comments.message_id FROM comments JOIN users on users.id = comments.user_id");
            }
        }
        
    }
}