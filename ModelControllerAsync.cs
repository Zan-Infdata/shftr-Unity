using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;

public class ModelControllerAsync{
    
    private Transform parent;
    private Transform defModel;

    public ModelControllerAsync(Transform p, Transform dm){
        this.parent = p;
        this.defModel = dm;
    }

    public void ImportModelAsync(string fp) {
        Importer.LoadFromFileAsync(fp, new ImportSettings(), OnFinishAsync);
    }

    private void OnFinishAsync(GameObject result, AnimationClip[] animations) {

        result.transform.SetParent(parent, false);
        //hide default model
        this.defModel.gameObject.SetActive(false);
    }


}
