using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField]
        private Datas datas;
        [SerializeField]
        private List<CollectionItem> collectionItemList;

        [HideInInspector]
        public List<CollectionItem> earnedCollectionItemList;
        private Dictionary<string ,CollectionItem> collectionItemDic;

        public static DataManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            earnedCollectionItemList = new List<CollectionItem>();
            collectionItemDic = new Dictionary<string, CollectionItem>();
        }

        private void Start()
        {
            for(int i=0; i<collectionItemList.Count; i++)
            {
                collectionItemDic.Add(collectionItemList[i].itemName, collectionItemList[i]);
                if (collectionItemList[i].get)
                    earnedCollectionItemList.Add(collectionItemList[i]);
            }
        }

        public int FindCharSpriteNum(Sprite charSprite)
        {
            for(int i=0;i<datas.Characters.Length;i++)
            {
                if(datas.Characters[i] == charSprite)
                    return i;
            }
            return -1;
        }
        public Sprite FindSpriteWithNum(int charNum)
        {
            if (charNum < 0) Debug.Log("¹üÀ§ ¹þ¾î³²");

            return datas.Characters[charNum];
        }

        public CollectionItem GetCollectionItem(string itemName)
        {
            return collectionItemDic[itemName];
        }

    }
}
