using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup {
    static Startup()
    {
        Debug.Log("SHAPESHIFTER is loaded!!!");
        ArticleManager.FillArticles();
    }
}
