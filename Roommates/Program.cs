﻿using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            //Instantiate a new instance of a RoomRepository
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);

            RoomateRepository roomateRepo = new RoomateRepository(CONNECTION_STRING);
           
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;                        
                    case ("Search for a roomate"):
                        Console.Write($"Roomate Id 1 - 3: ");
                        int roomateId = int.Parse(Console.ReadLine());

                        Roommate roomate = roomateRepo.GetById(roomateId);

                        Console.WriteLine($"{roomate.FirstName} - {roomate.Room.Name} Rent Portion({roomate.RentPortion})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break; 
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id} ");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnAssignedChores();
                        Console.Write("Unassigned Chores");
                        foreach (Chore c in unassignedChores)
                        {
                         Console.WriteLine($"{c.Name} has an Id of {c.Id} ");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.WriteLine("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r=>r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());
                        
                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string chore2AddName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = chore2AddName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign a chore to roommate"):
                        List<Roommate> allRoommates = roomateRepo.GetAllRoomates();
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach(Chore assignChore in allChores)
                        {
                            Console.WriteLine($"{assignChore.Id} - {assignChore.Name}");
                        }
                        Console.WriteLine("Please select the id of a chore");
                        int ChosenChore = int.Parse(Console.ReadLine());
                        foreach (Roommate assignRoommate in allRoommates)
                        {
                            Console.WriteLine($"{assignRoommate.Id} - {assignRoommate.FirstName} {assignRoommate.LastName}");
                        }
                        Console.WriteLine("Please Select the ID of a Roommate.");
                        int chosenRoommate = int.Parse(Console.ReadLine());
                        choreRepo.AssignChore( chosenRoommate, ChosenChore);
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Show all chores",
                "Show unassigned chores",
                "Search for room",
                "Search for a chore",
                "Search for a roomate",
                "Add a room",
                "Update a room",
                "Add a chore",
                "Assign a chore to roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
