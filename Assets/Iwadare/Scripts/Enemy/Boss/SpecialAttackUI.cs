using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackUI : MonoBehaviour
{
    GridLayoutGroup _grid;
    [SerializeField] GameObject _hpImageObj;
    List<Image> _hpImageList = new List<Image>();
    List<GameObject> _objectList = new List<GameObject>();

    public void InitHPView(float hp)
    {
        if (hp == 0) return;
        _grid = GetComponent<GridLayoutGroup>();
        var width = GetComponent<RectTransform>().sizeDelta;
        var cellSize = _grid.cellSize;
        cellSize.x = width.x / hp - _grid.spacing.x;
        _grid.cellSize = cellSize;

        for(var i = 0;i < hp;i++)
        {
            var hpImage = Instantiate(_hpImageObj,transform.position, quaternion.identity);
            hpImage.transform.SetParent(transform);
            _hpImageList.Add(hpImage.GetComponent<Image>());
            _objectList.Add(hpImage);
        }
    }

    public void HPDamageView()
    {
        if(_hpImageList.Count != 0)
        {
            var hpImage = _hpImageList[0];
            _hpImageList.Remove(hpImage);
            Destroy(hpImage);
        }
    }

    public void EndSpecial()
    {
        while(_objectList.Count != 0)
        {
            var hpObj = _objectList[0];
            _objectList.Remove(hpObj);
            Destroy(hpObj);
        }
    }


}
