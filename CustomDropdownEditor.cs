using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DemoArticle))]
[CanEditMultipleObjects]
public class CustomDropdownEditor : Editor {

    private DemoArticle da;


    void OnEnable(){
        //ArticleManager.FillArticles();
        da = (DemoArticle)target;
        da.SetTransforms();

    }
    

    public override void OnInspectorGUI(){
        da.ChangeArticle(EditorGUILayout.Popup("Select the article",   da.GetCurrInx(), ArticleManager.GetArticleNames()));
        if(GUI.changed)
            EditorUtility.SetDirty(da); 
    }




}