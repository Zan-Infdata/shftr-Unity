using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.IO;
using Siccity.GLTFUtility;


public class ShftrArticle : MonoBehaviour {

    // TODO: Remove serialize fields
    [SerializeField]
    private int id;
    
    [SerializeField]
    private GameObject defaultModel;
    private GameObject model;

    // TODO: remove
    [SerializeField]
    private Transform testParent;


    void OnAwake(){

        
    }


    public void Test(){

        string testURL = "http://localhost:3001/test/download";
        string urlParameters = "";

        var path = @"./Assets/Test/test.glb";

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(testURL);

        //add an Accept header for application/octet-stream
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
        HttpResponseMessage response = client.GetAsync(urlParameters).Result;

        if (response.IsSuccessStatusCode) {

            var byteArray = response.Content.ReadAsByteArrayAsync().Result;

            File.WriteAllBytes(path, byteArray);

            Debug.Log("OKAY - Importing model");


            ImportModel(path);



        } else {

            Debug.Log("BAD!!!");

        }
    }


    private void ImportModel(string fp){
        GameObject result = Importer.LoadFromFile(fp);
        Debug.Log("Imported the model!!!");

        result.transform.SetParent(testParent, false);
    }


}
