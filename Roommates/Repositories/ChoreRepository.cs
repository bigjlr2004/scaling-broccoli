using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetUnAssignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT RoommateChore.RoommateId, RoommateChore.ChoreID, Chore.Id, Chore.Name
                                        FROM RoommateChore
                                         RIGHT JOIN Chore on RoommateChore.ChoreID = Chore.Id
                                        WHERE RoommateChore.ChoreID is null";
                                        

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);
                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };
                            chores.Add(chore);
                        }
                        return chores;
                    }
                                        
                }
            }
        }
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                       OUTPUT INSERTED.Id
                                       VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();
                    chore.Id = id;
                }
            }
        }
       public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
                        using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (roommateId, choreId)
                                               OUTPUT INSERTED.Id
                                               VALUES (@roommateId, @choreId)";
                    cmd.Parameters.AddWithValue("@roommateId",roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    int id = (int)cmd.ExecuteScalar();

                    
                }
            }
        }

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                        SET Name = @name
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Chore> chores = new List<Chore>();
                            while (reader.Read())
                            {
                                int idColumnPosition = reader.GetOrdinal("Id");
                                int idValue = reader.GetInt32(idColumnPosition);
                                int nameColumnPosition = reader.GetOrdinal("Name");
                                string nameValue = reader.GetString(nameColumnPosition);

                                Chore chore = new Chore
                                {
                                    Id = idValue,
                                    Name = nameValue,

                                };
                                chores.Add(chore);
                            }
                            return chores;
                        }


                    }

                }
            }
        }
    }
}


