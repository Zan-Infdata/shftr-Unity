using Siccity.GLTFUtility;
using UnityEngine;
using SysTask = System.Threading.Tasks;
using System.IO;

static class ModelController{
    public static GameObject ImportModel(string file, Transform parent){
        string fp = FileManager.DOWN_PATH+file;

        if(!File.Exists(fp)){
            return null;
        }

        GameObject result = Importer.LoadFromFile(fp);

        result.transform.SetParent(parent, false);

        return result;
    }

    public static void ImportModelAsync(string file, Transform parent, Transform defModel){
        string fp = FileManager.DOWN_PATH+file;

        if(!File.Exists(fp)){
            return;
        }

        ModelControllerAsync mca = new ModelControllerAsync(parent,defModel);
        mca.ImportModelAsync(fp);
    }


    public static async SysTask.Task<bool> HandleDownload(string file){
        //check if model is already downloaded
        bool modExsits = FileManager.CheckIfModelExists(file);

        if(!modExsits){
            //download the model
            await APIController.DownoadModel(file);
        }
        return true;
    }
}