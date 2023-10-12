using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Siccity.GLTFUtility;
using Unity.Jobs;
using Unity.Collections;
using System.Text;



using System.IO;
using System;

public class DemoArticle : MonoBehaviour{

    [SerializeField]
    private int currId;

    [SerializeField]
    private Transform defModel;
    [SerializeField]
    private Transform model;
    [SerializeField]
    private GameObject currDefModel;

    [SerializeField]
    private bool isLoaded;

    [SerializeField]
    private string filter = "";

    private JobHandle jh;
    private NativeArray<char> na;

    public string GetFilter(){
        return filter;
    }

    public void SetFilter(string f){
        filter= f;
    }

    public int GetCurrId(){
        return currId;
    }

    public void SetCurrId(int i){
        currId=i;
    }


    void Reset(){
        bool check1 = transform.Find(ArticleManager.CHILD_1);
        bool check2 = transform.Find(ArticleManager.CHILD_2);

        if (!check1){
            CreateChildNode(ArticleManager.CHILD_1);
        }
        if (!check2){
            CreateChildNode(ArticleManager.CHILD_2);
        }

        SetTransforms();
    }

    private void CreateChildNode(string name){

        GameObject go = new GameObject(name);
        go.transform.SetParent(transform, false);
    }


    public void SetTransforms(){
        defModel = transform.Find(ArticleManager.CHILD_1);
        model = transform.Find(ArticleManager.CHILD_2);
    }

    void Update(){
        // load the model if the job is finished
        if(!isLoaded){
            if(jh.IsCompleted){

                jh.Complete();
                string file = new string(na.ToArray()).Trim('\0');
                na.Dispose();

                ModelController.ImportModelAsync(file,model,defModel);

                isLoaded = true;
            }
        }
        
    }

    void Start(){
        isLoaded = false;
        SetTransforms();
        jh = J_ShowCurrentModel();

    }

    public async void ChangeArticle(int inx){

        //return out if there is no inx
        if(inx ==-1){
            return;
        }

        int id = ArticleManager.MapInx(inx);
        
        //return out if the index is the same
        if (currId == id)
            return;

        
        //destroy previous model
        if (currDefModel != null){
            DestroyImmediate(currDefModel);
        }


        currId = id;
        //get file name
        JObject json_response = await APIController.GetArticleDefaultFileName(currId.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();
        
        //handle download
        await ModelController.HandleDownload(modelFileName);

        //show the model
        currDefModel = ModelController.ImportModel(modelFileName, defModel);

    }

    private JobHandle J_ShowCurrentModel(){

        na = new NativeArray<char>(45,Allocator.Persistent);

        ShowCurrentModelJob job = new ShowCurrentModelJob{
            modelId = currId,
            result = na,
        };

        return job.Schedule();
    }


    

}



public struct ShowCurrentModelJob : IJob{

    public int modelId;
    public NativeArray<char> result;
    public async void Execute(){



        //get file name
        JObject json_response = await APIController.GetCurrModelName(modelId.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();

        //hande download
        await ModelController.HandleDownload(modelFileName);

        char[] dump = modelFileName.ToCharArray();

        for (int i = 0; i < dump.Length; i++) {
            result[i] = dump[i];
        }


    }
}