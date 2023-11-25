using Siccity.GLTFUtility;
using UnityEngine;
using SysTask = System.Threading.Tasks;
using System.IO;


static class ModelController{
    public static GameObject ImportModel(byte[] byteStream, Transform parent){

        GameObject result = Importer.LoadFromBytes(byteStream);

        result.transform.SetParent(parent, false);

        return result;
    }

    public static void ImportModelAsync(byte[] byteStream, Transform parent, Transform defModel){
        ModelControllerAsync mca = new ModelControllerAsync(parent,defModel);
        mca.ImportModelAsync(byteStream);
    }

}