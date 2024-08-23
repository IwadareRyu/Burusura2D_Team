using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolActive : MonoBehaviour
{
    [Tooltip("プールするオブジェクト。")]
    [SerializeField] GameObject _poolObj;
    [Tooltip("生成した球を入れるリスト")]
    List<GameObject> _poolObjects = new List<GameObject>();
    [Tooltip("最初(Awake時)に生成する球の数")]
    [SerializeField] int _maxCount = 20;
    [Tooltip("生成した球をまとめる場所")]
    [SerializeField] GameObject Parent;

    private void Start()
    {
        //Awakeでプールを作る。
        Pool();
    }

    /// <summary>最初に球を複数個生成して、プールしておくメソッド</summary>
    private void Pool()
    {
        //_maxCount回、for文を回す。
        for (int i = 0; i < _maxCount; i++)
        {
            var newObj = CreateNewBullet(); //新しい球を作る。
            var enemyBullet = newObj.GetComponent<MoveBulletEnemy>();
            if(enemyBullet != null) 
            {
                enemyBullet.Init(); 
            }
            newObj.SetActive(false);
            if (Parent) newObj.transform.parent = Parent.transform; //Hielarceyを綺麗にしたいので一度球を空の親オブジェクトの子にした。
            _poolObjects.Add(newObj); //球をリストに追加。
        }
    }

    /// <summary>新しい球を生成するメソッド</summary>
    /// <returns>生成した球を返す。</returns>
    private GameObject CreateNewBullet()
    {
        var posistion = new Vector2(100, 100);
        var newObj = Instantiate(_poolObj, posistion, Quaternion.identity); //指定のポジションにオブジェクトを生成。
        newObj.name = _poolObj.name + (_poolObjects.Count + 1); // 名前が被らないように末尾の数字を変える。
        return newObj;
    }

    /// <summary>未使用の球の物理演算をtrueにして返すメソッド球を全て使っていたら新しく作って返す</summary>
    /// <returns>未使用の球or新しく作った球</returns>
    public GameObject GetBullet()
    {
        //使用中でないものを探して返す。
        foreach (var obj in _poolObjects)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        //全て使用中だったら新しく作り、リストに追加してから返す。
        var newObj = CreateNewBullet();
        Debug.Log("生成しました。");
        _poolObjects.Add(newObj);
        if (Parent) newObj.transform.parent = Parent.transform; //Hielarceyを綺麗にしたいので一度球を空の親オブジェクトの子にした。
        newObj.SetActive(true);
        return newObj;
    }
}
