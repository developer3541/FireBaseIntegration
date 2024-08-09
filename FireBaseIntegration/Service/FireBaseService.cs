using FireBaseIntegration.DTO;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FireBaseIntegration.Service
{
    public class FireBaseService
    {
        private readonly IConfiguration _configuration;
        public FireBaseService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<bool> Users(string user, string pass)
        {
            string proj = _configuration["DataConnection:ProjectId"];
            bool userexists = false;
            FirestoreDb db = FirestoreDb.Create(proj);
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
        public async Task<SourcePayload> SourceRetrival(string barcode)
        {
            SourcePayload payload = new SourcePayload();
            string proj = _configuration["DataConnection:ProjectId"];
            Source src = new Source();
            payload.source = src;
            bool userexists = false;
            try
            {
                FirestoreDb db = FirestoreDb.Create(proj);
                //var usersRef = db.Collection("users");
                Query capitalQuery = db.Collection("Source").WhereEqualTo("Code", barcode);
                QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
                string result = "";
                if (capitalQuerySnapshot.Documents.Count == 0)
                {
                    payload.exc = "Record not Found";
                }
                foreach (DocumentSnapshot document in capitalQuerySnapshot.Documents)
                {
                    if (document.Exists)
                    {

                        src.Code = document.GetValue<string>("Code");
                        src.Quantity = document.GetValue<string>("Quantity");
                        src.lineNo = document.GetValue<string>("lineNo");
                        src.location = document.GetValue<string>("location");
                        payload.status = true;
                        payload.exc = "Record Found";
                    }
                    else
                    {
                        payload.source = null;
                        payload.status = false;
                        payload.exc = "Record not found";
                    }
                }
            }
            catch (Exception ex)
            {
                payload.source = null;
                payload.status = false;
                payload.exc = ex.Message.ToString();
            }
            return payload;
        }
        public async Task<PayLoad> DestinationUpdate(Source src)
        {
            PayLoad payLoad = new PayLoad();
            Destination destination = new Destination();
            bool updated = false;
            string proj = _configuration["DataConnection:ProjectId"];
            FirestoreDb db = FirestoreDb.Create(proj);
            CollectionReference destinationRef = db.Collection("Destination");
            Query query = destinationRef.WhereEqualTo("Code", src.Code);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            if (snapshot.Documents.Count == 0)
            {
                payLoad.exc = "Record not found";
                payLoad.Destination = null;
            }
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
                        DocumentSnapshot updatedDocument = await document.Reference.GetSnapshotAsync();
                        if (updatedDocument.Exists)
                        {
                            Dictionary<string, object> documentData = updatedDocument.ToDictionary();
                            foreach (var kvp in documentData)
                            {

                                switch (kvp.Key)
                                {
                                    case "Code":
                                        destination.Code = kvp.Value.ToString();
                                        break;
                                    case "lineNo":
                                        destination.lineNo = kvp.Value.ToString();
                                        break;
                                    case "location":
                                        destination.location = kvp.Value.ToString();
                                        break;
                                    case "New_Quantity":
                                        destination.New_Quantity = kvp.Value.ToString();
                                        break;
                                    case "User":
                                        destination.User = kvp.Value.ToString();
                                        break;
                                    case "Update_Date":

                                        if (kvp.Value is Timestamp timestampDate)
                                            destination.Update_Date = timestampDate.ToDateTime();
                                        break;
                                    case "Update_Time":
                                        if (kvp.Value is Timestamp timestampTime)
                                            destination.Update_Time = timestampTime.ToDateTime();
                                        break;
                                    default:
                                        Console.WriteLine($"Unexpected field: {kvp.Key}");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            payLoad.exc = "Record not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        updated = false;
                        payLoad.exc = ex.Message.ToString();
                    }
                }
                else
                {
                    payLoad.exc = "Record not found";
                }
            }
            payLoad.Destination = destination;
            payLoad.updated = updated;
            return payLoad;
        }
    }
}
