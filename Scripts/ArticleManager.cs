using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;
using SysTask = System.Threading.Tasks;

public static class ArticleManager{

    public const string CHILD_1 = "DefModel";
    public const string CHILD_2 = "Model";

    [SerializeField]
    private static string filter = "";

    [SerializeField]
    private static Dictionary<int, int> idMap = new Dictionary<int, int>();

    [SerializeField]
    private static Dictionary<int, int> inxMap = new Dictionary<int, int>();
    [SerializeField]
    private static List<string> articleNames = new List<string>();

    public static string GetFilter(){
        return filter;
    }

    public static void SetFilter(string f){
        filter= f;
    }
    
    public static Dictionary<int, int> GetIdMapp(){
        return idMap;
    }

    public static int MapId(int i){

        // This article was never chosen or the filter currently doesn't include selected article.
        if(!idMap.ContainsKey(i)){
            return -1;
        }
        return idMap[i];
    }

    public static int MapInx(int i){
        return inxMap[i];
    }

    public static string[] GetArticleNames(){
        return articleNames.ToArray();
    }

    public static void ResetIdMap(){
        idMap = new Dictionary<int, int>();
        inxMap = new Dictionary<int, int>();
        articleNames = new List<string>();
    }


    public static async void FillArticles(string filter){

        //return out if articles were already loaded
        if(articleNames.Any()){
            return;
        }

        JObject json_response = await APIController.GetArticleList(filter);

        int inx = 0;
        foreach (var art in json_response[APIController.DATA]){

            string name = art[APIController.COL2].ToString();
            int id = int.Parse(art[APIController.COL1].ToString());



            articleNames.Add(name);
            idMap.Add(id,inx);
            inxMap.Add(inx,id);
            inx++;
        }



    }





}
