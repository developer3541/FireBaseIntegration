using Google.Cloud.Firestore;
using System.IO;

namespace FireBaseIntegration.Service
{
    public class FireBaseService
    {

        public void newmethods()
        {
        }
        public async Task Users()
        {

            FirestoreDb db = FirestoreDb.Create("project-b86ae");
            //var usersRef = db.Collection("users");
            Query capitalQuery = db.Collection("users").WhereEqualTo("Capital", true);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();

            //QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            //foreach (DocumentSnapshot document in snapshot.Documents)
            //{
            //    Console.WriteLine("User: {0}", document.Id);
            //    Dictionary<string, object> documentDictionary = document.ToDictionary();
            //    var irl = documentDictionary.Where(x => x.Key == "" && x.Value == "");
            //    Console.WriteLine("First: {0}", documentDictionary["First"]);
            //    if (documentDictionary.ContainsKey("Middle"))
            //    {
            //        Console.WriteLine("Middle: {0}", documentDictionary["Middle"]);
            //    }
            //    Console.WriteLine("Last: {0}", documentDictionary["Last"]);
            //    Console.WriteLine("Born: {0}", documentDictionary["Born"]);
            //    Console.WriteLine();
            //}



        }
    }
}
