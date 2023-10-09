using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;
using SysTask = System.Threading.Tasks;

public static class ArticleManager{




    private static Dictionary<int, int> inxMap = new Dictionary<int, int>();
    private static List<string> articleNames = new List<string>();

    public static Dictionary<int, int> GetInxMapp(){
        return inxMap;
    }

    public static int MapInx(int i){
        return inxMap[i];
    }

    public static string[] GetArticleNames(){
        return articleNames.ToArray();
    }


    public static async void FillArticles(){

        //return out if articles were already loaded
        if(articleNames.Any()){
            return;
        }
        
        JObject json_response = await APIController.GetArticleList();

        int i = 0;
        foreach (var art in json_response[APIController.DATA]){

            string name = art[APIController.COL2].ToString();
            int inx = int.Parse(art[APIController.COL1].ToString());



            articleNames.Add(name);
            inxMap.Add(i,inx);
            i++;
        }



    }





}
