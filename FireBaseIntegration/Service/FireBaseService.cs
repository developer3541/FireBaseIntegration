﻿using FireBaseIntegration.DTO;
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
            Query capitalQuery = db.Collection("users").WhereEqualTo("UserName", user).WhereEqualTo("Password", pass);
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
        public async Task<bool> SourceRetrival(string barcode)
        {
            Source src = new Source();
            bool userexists = false;
            FirestoreDb db = FirestoreDb.Create("project-b86ae");
            //var usersRef = db.Collection("users");
            Query capitalQuery = db.Collection("Source").WhereEqualTo("Barcode", barcode);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
            string result = "";
            foreach (DocumentSnapshot document in capitalQuerySnapshot.Documents)
            {
                if (document.Exists)
                {
                    document.GetValue<string>("Username");
                    document.GetValue<string>("Username");
                    document.GetValue<string>("Username");
                    document.GetValue<string>("Username");
                }
                else
                {

                }
            }
            return userexists;
        }
    }
}
