using Npgsql;
using praktimupaa2.Helpers;

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
