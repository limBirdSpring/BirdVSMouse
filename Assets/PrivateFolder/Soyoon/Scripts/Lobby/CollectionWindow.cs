using SoYoon;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionWindow : MonoBehaviour
{
    [SerializeField]
    private CollectionEntry collectionEntry;

    [SerializeField]
    private TMP_Text infoText;
    [SerializeField]
    private Transform BadgeCollect1Panel;
    [SerializeField]
    private Transform BadgeCollect2Panel;
    [SerializeField]
    private Transform specialBadgeCollectPanel;

    [SerializeField]
    private List<CollectionItem> badgeCollect1List;
    [SerializeField]
    private List<CollectionItem> badgeCollect2List;
    [SerializeField]
    private List<CollectionItem> specialBadgeCollectList;

    private List<CollectionEntry> entries;

    private void Awake()
    {
        entries = new List<CollectionEntry>();
    }

    private void OnEnable()
    {
        InitializeInfo();
        InitializeCollections();
    }

    private void InitializeInfo()
    {
        string info = string.Format(" °ÔÀÓ È½¼ö : {0} \n\n ½Â¸® È½¼ö : {1} \n ÆÐ¹è È½¼ö : {2} \n ¹«½ÂºÎ È½¼ö : {3} \n\n ½ºÆÄÀÌ ÇÃ·¹ÀÌ È½¼ö : {4}"
            , DataManager.Instance.myInfo.totalGame
            , DataManager.Instance.myInfo.win
            , DataManager.Instance.myInfo.draw,
            DataManager.Instance.myInfo.lose
            , DataManager.Instance.myInfo.totalSpy);
        infoText.text = info;
    }

    private void InitializeCollections()
    {
        for (int i = 0; i < entries.Count; i++)
            Destroy(entries[i].gameObject);
        entries.Clear();

        for (int i = 0; i < badgeCollect1List.Count; i++)
        {
            CollectionEntry obj = Instantiate(collectionEntry, BadgeCollect1Panel, false);
            obj.collectionItem = badgeCollect1List[i];
            entries.Add(obj);
        }

        for (int i = 0; i < badgeCollect2List.Count; i++)
        {
            CollectionEntry obj = Instantiate(collectionEntry, BadgeCollect2Panel, false);
            obj.collectionItem = badgeCollect2List[i];
            entries.Add(obj);
        }

        for (int i = 0; i < specialBadgeCollectList.Count; i++)
        {
            CollectionEntry obj = Instantiate(collectionEntry, specialBadgeCollectPanel, false);
            obj.collectionItem = specialBadgeCollectList[i];
            entries.Add(obj);
        }
    }
}
