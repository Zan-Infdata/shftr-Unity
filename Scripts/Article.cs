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

public class Article : MonoBehaviour{

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
    private NativeArray<byte> bs;
    private NativeArray<int> ms;

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


                byte[] byteStream = bs.ToArray();
                byteStream = byteStream[0..ms[0]];
                
                ms.Dispose();
                bs.Dispose();

                ModelController.ImportModelAsync(byteStream,model,defModel);

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
        byte[] byteStream = await APIController.DownoadModel(modelFileName);

        //show the model
        currDefModel = ModelController.ImportModel(byteStream, defModel);

    }

    private JobHandle J_ShowCurrentModel(){

        bs = new NativeArray<byte>(2000000,Allocator.Persistent);
        ms = new NativeArray<int>(1, Allocator.Persistent);

        ShowCurrentModelJob job = new ShowCurrentModelJob{
            modelId = currId,
            model = bs,
            modelSize = ms
        };

        return job.Schedule();
    }


    

}



public struct ShowCurrentModelJob : IJob{

    public int modelId;
    public NativeArray<byte> model;
    public NativeArray<int> modelSize;
    public async void Execute(){



        //get file name
        JObject json_response = await APIController.GetCurrModelName(modelId.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();

        //hande download
        byte[] dump = await APIController.DownoadModel(modelFileName);
        
        modelSize[0] = dump.Length;
        for (int i = 0; i < dump.Length; i++) {
            model[i] = dump[i];
            
        }


    }
}