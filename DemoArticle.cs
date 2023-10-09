using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Siccity.GLTFUtility;


public class DemoArticle : MonoBehaviour{
    [SerializeField]
    private int currInx;
    [SerializeField]
    private Transform defModel;
    [SerializeField]
    private Transform model;
    [SerializeField]
    private GameObject currDefModel;

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

    void Start(){
        ShowCurrentModel();
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

    private async void ShowCurrentModel(){
        int aid = ArticleManager.MapInx(currInx);

        //get file name
        JObject json_response = await APIController.GetCurrModelName(aid.ToString()); 
        string modelFileName = json_response[APIController.DATA][0][APIController.COL1].ToString();

        //hande download
        await ModelController.HandleDownload(modelFileName);


        //show model
        ModelController.ImportModel(modelFileName, model);
        //hide default model
        defModel.gameObject.SetActive(false);

    }

    

}
