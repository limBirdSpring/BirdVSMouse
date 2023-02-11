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
        private List<CollectionItem> collectionPhotoList;
        [SerializeField]
        private List<CollectionItem> collectionBadgeList;

        [HideInInspector]
        public List<CollectionItem> earnedPhotoCollectionItemList;
        [HideInInspector]
        public List<CollectionItem> earnedBadgeCollectionItemList;

        private Dictionary<string ,CollectionItem> collectionItemDic;

        public static DataManager Instance { get; private set; }
        public MyInfo myInfo;

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
            earnedPhotoCollectionItemList = new List<CollectionItem>();
            earnedBadgeCollectionItemList = new List<CollectionItem>();
            collectionItemDic = new Dictionary<string, CollectionItem>();

            string path = Path.Combine(Application.dataPath, "data.json");
            // json file이 있다면 load 없다면 생성
            if (File.Exists(path))
                LoadFromJson();
            else
                SaveToJson();

            for(int i=0; i<collectionPhotoList.Count; i++)
            {
                collectionItemDic.Add(collectionPhotoList[i].itemName, collectionPhotoList[i]);
                if (myInfo.getPhoto[i])
                {
                    earnedPhotoCollectionItemList.Add(collectionPhotoList[i]);
                }
            }

            for (int i = 0; i < collectionBadgeList.Count; i++)
            {
                collectionItemDic.Add(collectionBadgeList[i].itemName, collectionBadgeList[i]);
                if (myInfo.getBadge[i])
                {
                    earnedBadgeCollectionItemList.Add(collectionBadgeList[i]);
                }
            }
        }

        public int FindPhotoSpriteNum(Sprite photoSprite)
        {
            for (int i = 0; i < earnedPhotoCollectionItemList.Count; i++)
            {
                if (earnedPhotoCollectionItemList[i].itemIcon == photoSprite)
                    return i;
            }
            return -1;
        }

        public Sprite FindSpriteWithPhotoNum(int photoNum)
        {
            if (photoNum < 0) Debug.Log("error : cannot find sprite with photo Num");

            return earnedPhotoCollectionItemList[photoNum].itemIcon;
        }

        public int FindBadgeSpriteNum(Sprite badgeSprite)
        {
            for (int i = 0; i < earnedBadgeCollectionItemList.Count; i++)
            {
                if (earnedBadgeCollectionItemList[i].itemIcon == badgeSprite)
                    return i;
            }
            return -1;
        }

        public Sprite FindSpriteWithBadgeNum(int badgeSprite)
        {
            if (badgeSprite < 0) Debug.Log("error : cannot find sprite with badge Num");

            return earnedBadgeCollectionItemList[badgeSprite].itemIcon;
        }

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

        public CollectionItem GetCollectionItem(string itemName)
        {
            return collectionItemDic[itemName];
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
