using Cysharp.Threading.Tasks;
using MasterDataClass;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

# if UNITY_EDITOR
public static class ItemLoader
{

    [Tooltip("root(Assets)からのパス")]
    static string _rootPath = "Assets/Iwadare/ScriptableObject/Item";
    public static async UniTask LoadItem(string url)
    {
        string s = await LoadData(url);
        Debug.Log(s);
        if (s != "")
        {
            MasterDataClass<ItemData> data = JsonUtility.FromJson<MasterDataClass<ItemData>>(s);
            var itemList = new List<ItemScriptable>();

            SearchExistingItem(ref itemList);

            // ファイル内に同じIDのアイテムがあるかの確認。
            foreach (ItemData item in data.Data)
            {
                bool _chackOverLap = false;
                for (var i = 0; i < itemList.Count; i++)
                {
                    if (item.ID == itemList[i].ItemData._itemID)
                    {
                        itemList[i].ItemDataLoad(item);
                        itemList.RemoveAt(i);
                        _chackOverLap = true;
                        break;
                    }
                }
                if (!_chackOverLap)
                {
                    CreateScriptable(item);
                }
            }
        }
    }

    static async UniTask<string> LoadData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        return request.downloadHandler.text;
    }

    /// <summary>パスにあるアイテムデータ確認</summary>
    /// <param name="itemList">アイテムリスト</param>
    static void SearchExistingItem(ref List<ItemScriptable> itemList)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { _rootPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemScriptable item = AssetDatabase.LoadAssetAtPath<ItemScriptable>(path);

            if (item != null)
            {
                itemList.Add(item);
            }
        }

        Debug.Log($"ItemListCount : {itemList.Count}");
    }

    /// <summary>アイテムのScriptableObject生成メソッド</summary>
    /// <param name="item">アイテムデータ</param>
    static void CreateScriptable(ItemData item)
    {
        var itemObj = ScriptableObject.CreateInstance<ItemScriptable>();
        itemObj.ItemDataLoad(item);
        var fileName = $"Item{itemObj.ItemData._itemID}.asset";
        AssetDatabase.CreateAsset(itemObj, Path.Combine(_rootPath, fileName));
    }
}
#endif
