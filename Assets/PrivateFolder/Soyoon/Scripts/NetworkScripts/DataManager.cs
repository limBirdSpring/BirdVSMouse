using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public enum PlayResult { Play, Win, Draw, Lose, Spy }

namespace SoYoon
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField]
        private List<CollectionItem> collectionList;

        [HideInInspector]
        public List<CollectionItem> earnedCollectionItemList;
        [HideInInspector]
        public LinkedList<CollectionItem> mailedCollectionItemList;

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
            mailedCollectionItemList = new LinkedList<CollectionItem>();
            collectionItemDic = new Dictionary<string, CollectionItem>();

            string path = Path.Combine(Application.persistentDataPath, "data.json");

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
                mailedCollectionItemList.AddLast(collectionItemDic[myInfo.mailedItem[i]]);
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
            {
                GameObject.Find("Canvas").transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
                EarnedBadgesNum++;
            }

            mailedCollectionItemList.Remove(item);
            myInfo.mailedItem.Remove(itemName);
            SaveToJson();
        }

        public void EarnItemToMail(string itemName)
        {
            if(mailedCollectionItemList.Contains(collectionItemDic[itemName]))
                return;
            for (int i=0;i<earnedCollectionItemList.Count;i++)
            {
                if (earnedCollectionItemList[i].itemName == itemName)
                    return;
            }

            CollectionItem item = collectionItemDic[itemName];
            mailedCollectionItemList.AddLast(item);
            myInfo.mailedItem.Add(itemName);
            SaveToJson();
        }

        public void EarnCoin(int coin)
        {
            myInfo.coin += coin;
            SaveToJson();
        }

        public void SaveResult(PlayResult result)
        {
            switch(result)
            {
                case PlayResult.Play:
                    myInfo.totalGame++;
                    break;
                case PlayResult.Win:
                    myInfo.win++;
                    break;
                case PlayResult.Draw:
                    myInfo.draw++;
                    break;
                case PlayResult.Lose:
                    myInfo.lose++;
                    break;
                case PlayResult.Spy:
                    myInfo.totalSpy++;
                    break;
                default:
                    break;
            }
            SaveToJson();
        }

        public void SaveToJson()
        {
            string path = Path.Combine(Application.persistentDataPath, "data.json");
            string jsonData = JsonUtility.ToJson(myInfo, true);
            // 암호화
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);
            string code = System.Convert.ToBase64String(byteData);
            File.WriteAllText(path, code);
        }

        public void LoadFromJson()
        {
            string path = Path.Combine(Application.persistentDataPath, "data.json");
            string code = File.ReadAllText(path);
            byte[] byteData = System.Convert.FromBase64String(code);
            string jsonData = System.Text.Encoding.UTF8.GetString(byteData);
            myInfo = JsonUtility.FromJson<MyInfo>(jsonData);
        }
    }
}
