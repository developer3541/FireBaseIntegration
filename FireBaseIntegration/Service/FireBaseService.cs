using FireBaseIntegration.DTO;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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
            List<Source> srclist = new List<Source>();
            payload.source = srclist;
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
                        Source src = new Source();

                        src.Code = document.GetValue<string>("Code");
                        src.Quantity = document.GetValue<string>("Quantity");
                        src.lineNo = document.GetValue<string>("lineNo");
                        src.location = document.GetValue<string>("location");
                        srclist.Add(src);
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
        public async Task<PayLoad> DestinationUpdate(List<SourceSetPayload> srclist)
        {
            List<Destination> destinationlst = new List<Destination>();

            PayLoad payLoad = new PayLoad();
            bool updated = false;
            string proj = _configuration["DataConnection:ProjectId"];
            FirestoreDb db = FirestoreDb.Create(proj);
            CollectionReference destinationRef = db.Collection("Destination");
            foreach (var src in srclist)
            {

                Query query = destinationRef.WhereEqualTo("Code", src.code).WhereEqualTo("lineNo", src.lineno);
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
                        { "New_Quantity", src.quantity }, // Replace newQuantityValue with the new value for the quantity field
                        { "Update_Date", GetTimestamp(DateTime.Now) },
                        { "Update_Time",  GetTimestamp(DateTime.Now)}
                    };
                        try
                        {
                            // Update the document
                            await document.Reference.UpdateAsync(updates);
                            updated = true;
                            DocumentSnapshot updatedDocument = await document.Reference.GetSnapshotAsync();
                            if (updatedDocument.Exists)
                            {
                                Destination destination = new Destination();
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

                                            if (kvp.Value is string)
                                                destination.Update_Date = DateTime.ParseExact(kvp.Value.ToString(), "yyyy/MM/dd/ HH:mm", CultureInfo.InvariantCulture);
                                            break;
                                        case "Update_Time":
                                            if (kvp.Value is string)
                                                destination.Update_Time = DateTime.ParseExact(kvp.Value.ToString(), "yyyy/MM/dd/ HH:mm", CultureInfo.InvariantCulture);
                                            break;
                                        default:
                                            Console.WriteLine($"Unexpected field: {kvp.Key}");
                                            break;
                                    }
                                }
                                destinationlst.Add(destination);
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
            }

            payLoad.Destination = destinationlst;
            payLoad.updated = updated;
            return payLoad;
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyy/MM/dd/ HH:mm");
        }
    }
}
