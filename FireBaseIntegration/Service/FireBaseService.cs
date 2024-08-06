using FireBaseIntegration.DTO;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FireBaseIntegration.Service
{
    public class FireBaseService
    {

        public void newmethods()
        {
        }
        public async Task<bool> Users(string user, string pass)
        {
            bool userexists = false;
            FirestoreDb db = FirestoreDb.Create("project-b86ae");
            //var usersRef = db.Collection("users");
            Query capitalQuery = db.Collection("Users").WhereEqualTo("username", user).WhereEqualTo("password", pass);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
            string result = "";
            foreach (DocumentSnapshot document in capitalQuerySnapshot.Documents)
            {
                if (document.Exists)
                {
                    userexists = true;
                }
                else
                {
                    userexists = false;
                }
            }
            return userexists;
        }
        public async Task<Source> SourceRetrival(string barcode)
        {
            Source src = new Source();
            bool userexists = false;
            FirestoreDb db = FirestoreDb.Create("project-b86ae");
            //var usersRef = db.Collection("users");
            Query capitalQuery = db.Collection("Source").WhereEqualTo("Code", barcode);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
            string result = "";
            foreach (DocumentSnapshot document in capitalQuerySnapshot.Documents)
            {
                if (document.Exists)
                {

                    src.Code = document.GetValue<string>("Code");
                    src.Quantity = document.GetValue<string>("Quantity");
                    src.lineNo = document.GetValue<string>("lineNo");
                    src.location = document.GetValue<string>("location");
                }
                else
                {

                }
            }
            return src;
        }
        public async Task<bool> DestinationUpdate(Source src)
        {
            bool updated = false;
            FirestoreDb db = FirestoreDb.Create("project-b86ae");
            CollectionReference destinationRef = db.Collection("Destination");
            Query query = destinationRef.WhereEqualTo("Code", src.Code);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            // Update the quantity field for each matching document
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    // Create a dictionary with the field to update
                    Dictionary<string, object> updates = new Dictionary<string, object>
                    {
                        { "New_Quantity", src.Quantity } // Replace newQuantityValue with the new value for the quantity field
                    };
                    try
                    {
                        // Update the document
                        await document.Reference.UpdateAsync(updates);
                        updated = true;
                    }
                    catch (Exception ex)
                    {
                        updated = false;
                    }
                }
                else
                {
                    Console.WriteLine($"Document {document.Id} does not exist!");
                }
            }
            return updated;
        }
    }
}
