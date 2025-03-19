using Npgsql;
using praktimupaa2.Models;
using praktimupaa2.Helpers;

namespace praktimupaa2.Models.Auth
{
    public class AuthContext
    {
        private readonly string _constr;
        private string _errorMsg;
        public AuthContext(string connString)
        {
            _constr = connString;
        }
        public Person.Person GetPersonByEmail(string email)
        {
            Person.Person person = null;

            string query = string.Format(@"SELECT * FROM person WHERE email = @email");
            postgresHelper db = new postgresHelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@email", email);
                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    person = new Person.Person()
                    {
                        id_person = int.Parse(reader["id_person"].ToString()),
                        nama = reader["nama"].ToString(),
                        alamat = reader["alamat"].ToString(),
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString()
                    };
                }
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }

            return person;
        }
        public bool RegisterPerson(Person.Person person)
        {
            bool result = false;
            string query = string.Format(@"INSERT INTO person (nama, alamat, email, password) 
                                  VALUES (@nama, @alamat, @email, @password)");
            postgresHelper db = new postgresHelper(this._constr);

            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", person.nama);
                cmd.Parameters.AddWithValue("@alamat", person.alamat);
                cmd.Parameters.AddWithValue("@email", person.email);
                cmd.Parameters.AddWithValue("@password", person.password);

                int rowsAffected = cmd.ExecuteNonQuery();
                result = rowsAffected > 0;

                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
                Console.WriteLine("Error in RegisterPerson: " + ex.Message);
            }

            return result;
        }
    }
}
