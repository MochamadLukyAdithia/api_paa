using Npgsql;
using praktimupaa2.Models;
using praktimupaa2.Helpers;
using praktimupaa2.Models.Login;

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


        public List<Login.Auth> Authentifikasi(string namaUser, string password, IConfiguration _config)
        {
            List<Login.Auth> persons = new List<Login.Auth>();
            string query = string.Format(@"SELECT  person.email ,person.password FROM peran_person
                                            JOIN peran ON peran.id_peran = peran_person.id_peran
                                            JOIN person ON person.id_person = peran_person.id_person
                                            where person.nama ='{0}' and person.password ='{1}'", namaUser, password);
            postgresHelper db = new postgresHelper(this._constr);
            try
            {
                NpgsqlCommand cmd = db.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                JwtHelper jwtHelper = new JwtHelper(_config);
                
                while (reader.Read())
                {
                    persons.Add(new Login.Auth()
                    {  
                        email = reader["email"].ToString(),
                        password = reader["password"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                _errorMsg = ex.Message;
            }
            return persons;
        }



        public Person.Person Auth(string email, string password)
        {
            Person.Person person = null;

            string query = "SELECT id_person, email, nama, password FROM person WHERE email ILIKE @email OR password ILIKE @password;";

            
            postgresHelper db = new postgresHelper(_constr);
            
                try
                {
                    using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Jika ada hasil dari query
                            {
                                person = new Person.Person()
                                {
                                    id_person = reader.GetInt32(0),
                                    email = reader.GetString(1),
                                    nama = reader.GetString(2)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _errorMsg = ex.Message;
                }
                finally
                {
                    db.closeConnection();
                }
            

            return person;
        }

        public bool RegisterPerson(Person.Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person object cannot be null.");
            }

            if (person.id_peran == null)
            {
                throw new ArgumentNullException(nameof(person.id_peran), "id_peran cannot be null.");
            }

            bool result = false;

            string query = @"INSERT INTO person (nama, alamat, email, password) 
                     VALUES (@nama, @alamat, @email, @password) 
                     RETURNING id_person;";

            try
            {
            
                postgresHelper db = new postgresHelper(this._constr);
                using (NpgsqlCommand cmd = db.GetNpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@nama", person.nama);
                    cmd.Parameters.AddWithValue("@alamat", person.alamat);
                    cmd.Parameters.AddWithValue("@email", person.email);
                    cmd.Parameters.AddWithValue("@password", person.password);

                    object resultObj = cmd.ExecuteScalar();
                   
                    if (resultObj == null)
                    {
                        Console.WriteLine("Failed to retrieve id_person. Insert might have failed.");
                        return false;
                    }

                    int idPerson = Convert.ToInt32(resultObj);
                    Console.WriteLine("Inserted id_person: " + idPerson);

                    string queryPeran = @"INSERT INTO peran_person (id_peran, id_person) VALUES(@id_peran, @id_person);";
                    postgresHelper dbPeran = new postgresHelper(this._constr);
                    using (NpgsqlCommand cmdPeran = dbPeran.GetNpgsqlCommand(queryPeran))
                    {
                        cmdPeran.Parameters.AddWithValue("@id_peran", person.id_peran);
                        cmdPeran.Parameters.AddWithValue("@id_person", idPerson);
                        cmdPeran.ExecuteNonQuery();
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RegisterPerson: " + ex.ToString());
            }

            return result;
        }



    }
}
