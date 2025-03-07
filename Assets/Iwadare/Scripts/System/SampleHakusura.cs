using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleHakusura : MonoBehaviour
{
    [SerializeField] HakusuraNum[] _hakusura;
    int defaultAttack = 100;
    int totalAttack;

    //public SampleAttack()
    //{
    //    foreach (var haku in _hakusura)
    //    {
    //        var ramdom = RamdomMethod.RamdomNumberMinMax(haku.defaultNum, haku.defaultNum + haku.ramNum);

    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        totalAttack = defaultAttack;
        // ランダムな値の評価値(平均値)
        var rank = 0f;
        foreach (var haku in _hakusura)
        {
            var num = RamdomMethod.RamdomNumberMinMax(haku.defaultNum, haku.defaultNum + haku.ramNum);
            rank += (num - haku.defaultNum) / haku.ramNum;
            Debug.Log($"{Enum.GetName(typeof(HakusuraState),haku.state)} の値は {(int)num} です。");
            switch (haku.state)
            {
                case HakusuraState.Weapon:
                    AttackNum(num);
                    break;
            }
        }
        rank = rank / _hakusura.Length;
        Debug.Log($"評価:{(int)(rank * 100)}");
        Debug.Log($"合計攻撃力:{totalAttack}");
    }

    void AttackNum(float num)
    {
        totalAttack += (int)num;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    struct HakusuraNum
    {
        public HakusuraState state;
        public float defaultNum;
        public float ramNum;
    }

    struct SeiseiHakusura
    {
        public HakusuraState state;

    }

    enum HakusuraState
    {
        None,
        Weapon,
        Armor,
        HP,
    }
}
