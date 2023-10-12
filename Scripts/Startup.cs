using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class StartupEditor {

    static StartupEditor(){

        if(!Directory.Exists(FileManager.DOWN_PATH)){
            System.IO.Directory.CreateDirectory(FileManager.DOWN_PATH);
            Debug.LogWarning("Target download directory was not found so it was created at: " + FileManager.DOWN_PATH);
        }
    }

}


public class StartupPlayMode{
    [InitializeOnEnterPlayMode]
    static void StartPlayMode(){
        ArticleManager.FillArticles("");
    }
}
