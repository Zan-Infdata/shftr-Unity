using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DemoArticle))]
[CanEditMultipleObjects]
public class CustomDropdownEditor : Editor {

    private DemoArticle da;
    private SerializedProperty filterProp;


    void OnEnable(){

        filterProp = serializedObject.FindProperty ("filter");

        
        da = (DemoArticle)target;
        ArticleManager.FillArticles(da.GetFilter());

    }
    

    public override void OnInspectorGUI(){

        string prevFilter = filterProp.stringValue.Trim();


        EditorGUILayout.PropertyField(filterProp, new GUIContent ("Filter"));

        da.ChangeArticle(EditorGUILayout.Popup("Select the article",   ArticleManager.MapId(da.GetCurrId()), ArticleManager.GetArticleNames()));
        if(GUI.changed)
            EditorUtility.SetDirty(da); 


        if(prevFilter != filterProp.stringValue.Trim()){
            da.SetFilter(filterProp.stringValue.Trim());
            ArticleManager.SetFilter(filterProp.stringValue.Trim());

            ArticleManager.ResetIdMap();
            ArticleManager.FillArticles(filterProp.stringValue);

            Repaint();
        }
        else if(filterProp.stringValue.Trim() != ArticleManager.GetFilter()){ 
            da.SetFilter(ArticleManager.GetFilter());
            
        }

        serializedObject.ApplyModifiedProperties ();
    }




}