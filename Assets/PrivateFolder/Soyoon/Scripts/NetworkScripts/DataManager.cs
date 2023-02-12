using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace SoYoon
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField]
        private List<CollectionItem> collectionList;

        [HideInInspector]
        public List<CollectionItem> earnedCollectionItemList;
        [HideInInspector]
        public List<CollectionItem> mailedCollectionItemList;

        private Dictionary<string ,CollectionItem> collectionItemDic;

        public static DataManager Instance { get; private set; }
        public MyInfo myInfo;

        public int EarnedPhotosNum { get; private set; }
        public int EarnedBadgesNum { get; private set; }


        private void Awake()
        {
            if (GameObject.Find("DataManager") != null)
            {
                DataManager[] data = GameObject.FindObjectsOfType<DataManager>();
                if (data.Length == 1)
                {
                    Instance = this;
                    DontDestroyOnLoad(this);
                }
                else
                    Destroy(this.gameObject);
            }
            earnedCollectionItemList = new List<CollectionItem>();
            mailedCollectionItemList = new List<CollectionItem>();
            collectionItemDic = new Dictionary<string, CollectionItem>();

            string path = Path.Combine(Application.dataPath, "data.json");
            // json file이 있다면 load 없다면 생성
            if (File.Exists(path))
                LoadFromJson();
            else
                SaveToJson();

            EarnedPhotosNum = 0;
            EarnedBadgesNum = 0;

            for (int i = 0; i < collectionList.Count; i++)
                collectionItemDic.Add(collectionList[i].itemName, collectionList[i]);

            for (int i = 0; i < myInfo.earnedItem.Count; i++)
            {
                earnedCollectionItemList.Add(collectionItemDic[myInfo.earnedItem[i]]);

                if (collectionItemDic[myInfo.earnedItem[i]].type == ItemType.Photo)
                    EarnedPhotosNum++;
                else if (collectionItemDic[myInfo.earnedItem[i]].type == ItemType.Badge)
                    EarnedBadgesNum++;

            }

            for(int i=0;i< myInfo.mailedItem.Count; i++)
                mailedCollectionItemList.Add(collectionItemDic[myInfo.mailedItem[i]]);
        }

        public CollectionItem GetCollectionItem(string itemName)
        {
            return collectionItemDic[itemName];
        }

        public void EarnItem(string itemName)
        {
            CollectionItem item = collectionItemDic[itemName];
            earnedCollectionItemList.Add(item);
            myInfo.earnedItem.Add(itemName);
            if (item.type == ItemType.Photo)
                EarnedPhotosNum++;
            else if (item.type == ItemType.Badge)
                EarnedBadgesNum++;
            SaveToJson();
        }

        //public void EarnPhoto(string photoName)
        //{
        //    CollectionItem earnedItem = collectionItemDic[photoName];
        //    earnedPhotoCollectionItemList.Add(earnedItem);
        //    int itemNum = FindPhotoSpriteNum(earnedItem.itemIcon);
        //    if (itemNum == -1)
        //    { Debug.Log("error : cannot find photo sprite Num"); return; }
        //    myInfo.getPhoto[itemNum] = true;
        //    SaveToJson();
        //}
        //
        //public void EarnBadge(string badgeName)
        //{
        //    CollectionItem earnedItem = collectionItemDic[badgeName];
        //    earnedBadgeCollectionItemList.Add(earnedItem);
        //    int itemNum = FindBadgeSpriteNum(earnedItem.itemIcon);
        //    if (itemNum == -1)
        //    { Debug.Log("error : cannot find badge sprite Num"); return; }
        //    myInfo.getBadge[itemNum] = true;
        //    SaveToJson();
        //}


        public void changeName(string name)
        {

        }

        public void changeBadge1(int badge1Num)
        {

        }

        public void changeBadge2(int badge2Num)
        {

        }

        public void changePhoto(int photoNum)
        {

        }

        public void Win()
        {

        }

        public void Draw()
        {
            
        }

        public void Lose()
        {

        }

        public void SaveToJson()
        {
            string jsonData = JsonUtility.ToJson(myInfo, true);
            string path = Path.Combine(Application.dataPath, "data.json");
            File.WriteAllText(path, jsonData);
        }

        public void LoadFromJson()
        {
            string path = Path.Combine(Application.dataPath, "data.json");
            string jsonData = File.ReadAllText(path);
            myInfo = JsonUtility.FromJson<MyInfo>(jsonData);
        }
    }
}
