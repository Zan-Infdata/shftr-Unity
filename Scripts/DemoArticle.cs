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
    private int currInx;
    [SerializeField]
    private Transform defModel;
    [SerializeField]
    private Transform model;
    [SerializeField]
    private GameObject currDefModel;

    [SerializeField]
    private bool isLoaded;

    private JobHandle jh;
    private NativeArray<char> na;

    public int GetCurrInx(){
        return currInx;
    }

    public void SetCurrInx(int i){
        currInx=i;
    }

    //TODO: maybe find better way to deal with this
    public void SetTransforms(){
        defModel = transform.GetChild(0);
        model = transform.GetChild(1);
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
        
        //return out if the index is the same
        if (currInx == inx)
            return;
        //destroy previous model
        if (currDefModel != null){
            DestroyImmediate(currDefModel);
        }


        int aid = ArticleManager.MapInx(inx);
        //get file name
        JObject json_response = await APIController.GetArticleDefaultFileName(aid.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();
        
        //handle download
        await ModelController.HandleDownload(modelFileName);

        //show the model
        currDefModel = ModelController.ImportModel(modelFileName, defModel);


        currInx= inx;

    }

    private JobHandle J_ShowCurrentModel(){

        na = new NativeArray<char>(45,Allocator.Persistent);

        ShowCurrentModelJob job = new ShowCurrentModelJob{
            modelInx = currInx,
            result = na,
        };

        return job.Schedule();
    }


    

}



public struct ShowCurrentModelJob : IJob{

    public int modelInx;
    public NativeArray<char> result;
    public async void Execute(){


        int aid = ArticleManager.MapInx(modelInx);

        //get file name
        JObject json_response = await APIController.GetCurrModelName(aid.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();

        //hande download
        await ModelController.HandleDownload(modelFileName);

        char[] dump = modelFileName.ToCharArray();

        for (int i = 0; i < dump.Length; i++) {
            result[i] = dump[i];
        }


    }
}