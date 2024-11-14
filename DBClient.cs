using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace HotelDBConnection
{
    class DBClient
    {
        //Database connection string - replace it with the connnection string to your own database 
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDB;Integrated Security=True;";

        private int GetMaxFacilityId(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityId");

            //This SQL command will fetch one row from the DemoHotel table: The one with the max Hotel_No
            string queryStringFacilityId = "SELECT  MAX(Facility_id)  FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringFacilityId}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringFacilityId, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max Facility id
            int maxFacilityId = 0;

            //Is there any rows in the query
            if  (reader.Read())
            {
                //Yes, get max hotel_no
                maxFacilityId = reader.GetInt32(0); //Reading int from 1st column
            }

            //Close reader
            reader.Close();
            
            Console.WriteLine($"Max Facility Id: {maxFacilityId}");
            Console.WriteLine();

            //Return max hotel_no
            return maxFacilityId;
        }

        private int DeleteFacility(SqlConnection connection, int facilityId)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            //This SQL command will delete one row from the DemoHotel table: The one with primary key hotel_No
            string deleteCommandString = $"DELETE FROM Facility  WHERE Facility_id = {facilityId}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting facility #{facilityId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            //This SQL command will update one row from the DemoHotel table: The one with primary key hotel_No
            string updateCommandString = $"UPDATE Facility SET Facility_Name='{facility.Facility_Name}' WHERE Facility_id = {facility.Facility_id}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating facility #{facility.Facility_id}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            //This SQL command will insert one row into the DemoHotel table with primary key hotel_No
            string insertCommandString = $"INSERT INTO Facility (Facility_Name) VALUES ('{facility.Facility_Name}')";

            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);
            
            Console.WriteLine($"Creating facility #{facility.Facility_id}");
            int numberOfRowsAffected = command.ExecuteNonQuery();
            
            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Facility> ListAllFacility(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacility");

            //This SQL command will fetch all rows and columns from the DemoHotel table
            string queryStringAllFacility = "SELECT * FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringAllFacility}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all facility:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facility in database");
                reader.Close();
                
                //Return null for 'no facility found'
                return null;
            }

            //Create list of facility found
            List<Facility> facility = new List<Facility>();
            while (reader.Read())
            {
                //If we reached here, there is still one facility to be put into the list 
                Facility nextFacility = new Facility()
                {
                    Facility_id = reader.GetInt32(0), //Reading int from 1st column
                    Facility_Name = reader.GetString(1),    //Reading string from 2nd column
                };

                //Add facility to list
                facility.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of facility
            return facility;
        }

        private Facility GetFacility(SqlConnection connection, int facility_id)
        {
            Console.WriteLine("Calling -> GetFacility");

            //This SQL command will fetch the row with primary key hotel_no from the DemoHotel table
            string queryStringOneFacility = $"SELECT * FROM Facility WHERE Facility_id = {facility_id}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            //Prepare SQK command
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding facility#: {facility_id}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facility in database");
                reader.Close();

                //Return null for 'no facility found'
                return null;
            }

            //Fetch facility object from the database
            Facility facility = null; 
            if (reader.Read())
            {
                facility = new Facility()
                {
                    Facility_id = reader.GetInt32(0), //Reading int from 1st column
                    Facility_Name = reader.GetString(1),    //Reading string from 2nd column
                };

                Console.WriteLine(facility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found facility
            return facility;
        }
        public void Start()
        {
            //Apply 'using' to connection (SqlConnection) in order to call Dispose (interface IDisposable) 
            //whenever the 'using' block exits
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();
                
                //List all hotels in the database
                ListAllFacility(connection);

                //Create a new facility equal to current max plus 1
                Facility newFacility = new Facility()
                {
                    Facility_id = GetMaxFacilityId(connection) + 1,
                    Facility_Name = "New Facility"
                };

                //Insert the facility into the database
                InsertFacility(connection, newFacility);

                //List all facility including the the newly inserted one
                ListAllFacility(connection);

                //Get the newly inserted facility from the database in order to update it 
                Facility facilityToBeUpdated = GetFacility(connection, newFacility.Facility_id);

                //Alter Name properties
                facilityToBeUpdated.Facility_Name += "(updated)";

                //Update the facility in the database 
                UpdateFacility(connection, facilityToBeUpdated);

                //List all facility including the updated one
                ListAllFacility(connection);

                //Get the updated facility in order to delete it
                Facility facilityToBeDeleted = GetFacility(connection, facilityToBeUpdated.Facility_id);

                //Delete the facility
                DeleteFacility(connection, facilityToBeDeleted.Facility_id);

                //List all facility - now without the deleted one
                ListAllFacility(connection);
            }
        }
    }
}
