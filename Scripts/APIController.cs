using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using SysTask = System.Threading.Tasks;

public static class APIController{
    public const string API_URL = "http://localhost:3001/";
    public const string API_ART_LIST = "article/list";
    public const string API_DEF_ART_MOD="article/file";
    public const string API_CURR_ART_MOD="model/file";
    public const string API_MOD_DOWN = "model/download";
    public const string API_DEF_ART_MOD_PARAM = "?aid=";
    public const string API_CURR_ART_MOD_PARAM = "?aid=";
    public const string API_MOD_DOWN_PARAM = "?file=";
    public const string COL1 = "c01";
    public const string COL2 = "c02";
    public const string DATA = "DATA";
    public const string COUNT = "CNT";



    public static async SysTask.Task<JObject> GetArticleList(string f){
        string testURL = API_URL+API_ART_LIST;
        string urlParameters = "?filter="+f;

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(testURL+urlParameters);

        //add an Accept header for application/octet-stream
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = client.GetAsync("").Result;

        if (response.IsSuccessStatusCode) {

            string jsonRes = await response.Content.ReadAsStringAsync();
            JObject res = JObject.Parse(jsonRes);

            return res;


        } else {

            //TODO: handle error
            return null;

        } 
    }


    public static async SysTask.Task<JObject> GetArticleDefaultFileName(string aid){

        string testURL = API_URL+API_DEF_ART_MOD;
        string urlParameters = API_DEF_ART_MOD_PARAM+aid;

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(testURL);

        //add an Accept header for application/octet-stream
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = client.GetAsync(urlParameters).Result;

        if (response.IsSuccessStatusCode) {

            string jsonRes = await response.Content.ReadAsStringAsync();
            JObject res = JObject.Parse(jsonRes);

            return res;


        } else {

            //TODO: handle error
            return null;

        } 
    }

    public static async SysTask.Task<bool> DownoadModel(string file){

        string testURL = API_URL+API_MOD_DOWN;
        string urlParameters = API_MOD_DOWN_PARAM+file;

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(testURL);

        //add an Accept header for application/octet-stream
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
        HttpResponseMessage response = client.GetAsync(urlParameters).Result;

        if (response.IsSuccessStatusCode) {

            var byteArray = await response.Content.ReadAsByteArrayAsync();


            FileManager.SaveModel(file, byteArray);

            return true;

        } else {
            //TODO: handle error
            return false;
        }
    }

        public static async SysTask.Task<JObject> GetCurrModelName(string aid){

        string testURL = API_URL+API_CURR_ART_MOD;
        string urlParameters = API_CURR_ART_MOD_PARAM+aid;

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(testURL);

        //add an Accept header for application/octet-stream
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = client.GetAsync(urlParameters).Result;

        if (response.IsSuccessStatusCode) {

            string jsonRes = await response.Content.ReadAsStringAsync();
            JObject res = JObject.Parse(jsonRes);

            return res;


        } else {

            //TODO: handle error
            return null;

        } 
    }


}