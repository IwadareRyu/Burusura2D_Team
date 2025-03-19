using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterDataClass
{
    [Serializable]
    public class ItemData
    {
        public int ID;
        public string ItemName;
        public string Description;
        public int BaseAttack;
        public int MaxAttack;
        public int BaseDiffence;
        public int MaxDiffence;
        public int BaseHP;
        public int MaxHP;
    }

    [Serializable]
    public class MasterDataClass<T>
    {
        public string Version;
        public T[] Data;
    }
}