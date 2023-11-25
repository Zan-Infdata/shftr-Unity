using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class StartupEditor {

    static StartupEditor(){
        ArticleManager.SetFilter("");
        ArticleManager.ResetIdMap();
        ArticleManager.FillArticles("");
    }

}

public class StartupPlayMode{
    [InitializeOnEnterPlayMode]
    static void StartPlayMode(){
        ArticleManager.FillArticles("");
    }
}
