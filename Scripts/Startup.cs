using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StartupPlayMode{
    [InitializeOnEnterPlayMode]
    static void StartPlayMode(){
        ArticleManager.FillArticles("");
    }
}
