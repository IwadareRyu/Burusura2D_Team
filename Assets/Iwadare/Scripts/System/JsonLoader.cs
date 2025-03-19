using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class JsonLoader: EditorWindow
{
    URLData _urlData = new URLData();

    [MenuItem("CustomTools/JsonInput")]
    public static void ShowWindow()
    {
        GetWindow<JsonLoader>("Json Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("取りたいデータとURL入力",EditorStyles.boldLabel);

        _urlData._select = (SelectData)EditorGUILayout.EnumPopup("選択肢",_urlData._select);

        _urlData._url = EditorGUILayout.TextField("URLを入力", _urlData._url);

        GUILayout.Space(20);
        GUILayout.Label($"データ選択 : {_urlData._select}");
        GUILayout.Label($"URL入力 : {_urlData._url}?sheet=");

        if(GUILayout.Button("ロード開始"))
        {
            if (_urlData._select == SelectData.Item)
            {
                ItemLoader.LoadItem(_urlData._url + "?sheet=").Forget();
            }
            Close();
        }
    }
}

#endif

public class URLData
{
    public SelectData _select;
    public string _url;
}

public enum SelectData
{
    Item,
    Enemy,
    Skill,
}
