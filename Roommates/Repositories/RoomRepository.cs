using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Room data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoomRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRepository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoomRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Add a new room to the databse 
        /// NOTE: this methods sends data to the database
        /// it does not get anything from the database, so there is nothing to return.
        /// </summary>
        
        public void Update(Room room)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Update Room
                                        SET Name = @name,
                                        MaxOccupancy = @maxOccupancy
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    cmd.Parameters.AddWithValue("@id", room.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Delete the room with the given id
        /// </summary>
       
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Insert(Room room)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Room (Name, MaxOccupancy)
                                                OUTPUT INSERTED.Id
                                                VALUES (@name, @maxOccupancy)";
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    int id = (int)cmd.ExecuteScalar();

                    room.Id = id;
                }
            }
        }



        // ...We'll add some methods shortly...
        /// <summary>
        ///  Returns a single room with the given id.
        /// </summary>
        public Room GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, MaxOccupancy FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Room room = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            room = new Room
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                            };
                        }
                        return room;
                    }

                }
            }
        }

        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<Room> GetAll()
        {
            
            using (SqlConnection conn = Connection)
            {
                
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = "SELECT Id, Name, MaxOccupancy FROM Room";

                 
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        List<Room> rooms = new List<Room>();
                
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");


                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            int maxOccupancyColumPosition = reader.GetOrdinal("MaxOccupancy");
                            int maxOccupancy = reader.GetInt32(maxOccupancyColumPosition);

                            Room room = new Room
                            {
                                Id = idValue,
                                Name = nameValue,
                                MaxOccupancy = maxOccupancy,
                            };

                            // ...and add that room object to our list.
                            rooms.Add(room);
                        }
                        // Return the list of rooms who whomever called this method.
                        return rooms;
                    }

                }
            }
        }
    }
}
