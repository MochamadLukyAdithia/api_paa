using Npgsql;
using praktimupaa2.Helpers;
using System.Data;

namespace praktimupaa2.Models.Person
{
    public class PersonContext
    {
        private readonly string _connString;
        private  string _errorMessage;
        public PersonContext(string connString)
        {
            _connString = connString;
        }
        public List<Person> allPerson()
        {
            List<Person> persons = new List<Person>();
            string query = "SELECT * FROM person";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand  cmd = helper.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person();
                    person.id_person = reader.GetInt32(0);
                    person.nama = reader.GetString(1);
                    person.alamat = reader.GetString(2);
                    person.email = reader.GetString(3);
                    person.password = reader.GetString(4);
                    persons.Add(person);
                }
                cmd.Dispose();
                helper.closeConnection();
                
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;

            }
            return persons;

        }
        public List<Person> getPersonWithAuth()
        {
            List<Person> persons = new List<Person>();
           
            string query = "SELECT * FROM person JOIN peran_person USING(id_person) JOIN peran USING(id_peran)";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person();
                    person.id_person = int.Parse(reader["id_person"].ToString());
                    person.nama = reader["nama"].ToString();
                    person.alamat = reader["alamat"].ToString();
                    person.email = reader["email"].ToString();
                    person.password = reader["password"].ToString();
                    person.id_peran = int.Parse(reader["id_peran"].ToString());
                    person.nama_peran = reader["nama_peran"].ToString();
                    persons.Add(person);
                }
                cmd.Dispose();
                helper.closeConnection();

            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;

            }
            return persons;
        }
    }
}
