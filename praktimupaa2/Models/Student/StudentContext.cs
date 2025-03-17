using Npgsql;
using praktimupaa2.Helpers;
using System.Collections.Generic;
using System;

namespace praktimupaa2.Models.Student
{
    public class StudentContext
    {
        private readonly string _connString;
        private string _errorMessage;

        public StudentContext(string connString)
        {
            _connString = connString;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT * FROM student";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Student student = new Student();
                    student.id_student = reader.GetInt32(0);
                    student.nama = reader.GetString(1);
                    student.alamat = reader.GetString(2);
                    student.email = reader.GetString(3);
                    student.jurusan = reader.GetString(4);
                    students.Add(student);
                }
                cmd.Dispose();
                helper.closeConnection();
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            return students;
        }

        public Student GetStudentById(int id)
        {
            Student student = null;
            string query = "SELECT * FROM student WHERE id_student = @id";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", id);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    student = new Student();
                    student.id_student = reader.GetInt32(0);
                    student.nama = reader.GetString(1);
                    student.alamat = reader.GetString(2);
                    student.email = reader.GetString(3);
                    student.jurusan = reader.GetString(4);
                }
                cmd.Dispose();
                helper.closeConnection();
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            return student;
        }

        public bool AddStudent(Student student)
        {
            string query = "INSERT INTO student (nama, alamat, email, jurusan) VALUES (@nama, @alamat, @email, @jurusan)";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", student.nama);
                cmd.Parameters.AddWithValue("@alamat", student.alamat);
                cmd.Parameters.AddWithValue("@email", student.email);
                cmd.Parameters.AddWithValue("@jurusan", student.jurusan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                helper.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateStudent(Student student)
        {
            string query = "UPDATE student SET nama = @nama, alamat = @alamat, email = @email, jurusan = @jurusan WHERE id_student = @id";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", student.id_student);
                cmd.Parameters.AddWithValue("@nama", student.nama);
                cmd.Parameters.AddWithValue("@alamat", student.alamat);
                cmd.Parameters.AddWithValue("@email", student.email);
                cmd.Parameters.AddWithValue("@jurusan", student.jurusan);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                helper.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public bool DeleteStudent(int id)
        {
            string query = "DELETE FROM student WHERE id_student = @id";
            postgresHelper helper = new postgresHelper(_connString);
            try
            {
                NpgsqlCommand cmd = helper.GetNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                helper.closeConnection();
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }
    }
}
