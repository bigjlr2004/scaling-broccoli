using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repositories
{
    public class RoomateRepository:BaseRepository

    {
        public RoomateRepository(string connectionString) : base(connectionString) { }

        //Returns a single roommate with their First Name, Rent Portion, and the Name of the Room they Occupy.
        //Hint You will want to use A JOIN statement in your SQL Query
        /// <summary>
        ///    public class Roommate
        ///  public int Id { get; set; }
        ///  public string FirstName { get; set; }
        ///  public string LastName { get; set; }
        ///  public int RentPortion { get; set; }
        ///  public DateTime MovedInDate { get; set; }
        ///  public Room Room { get; set; }
        /// </summary>

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, LastName, RentPortion, MoveInDate, Room.Id, Room.Name, Room.MaxOccupancy
                                    FROM Roommate
                                    JOIN Room on Room.Id = Roommate.RoomId     
                                     WHERE Roommate.Id =@id";

                    cmd.Parameters.AddWithValue("@id", id);
                     
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;
                        

                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room
                                {
                                    Id = id,
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                }

                            };
                           
                        }
                        return roommate;
                    }

                }
            }
        }

    }
}
