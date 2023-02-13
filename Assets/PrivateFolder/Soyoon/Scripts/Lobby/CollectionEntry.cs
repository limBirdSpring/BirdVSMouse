using SoYoon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionEntry : MonoBehaviour
{
    [SerializeField]
    private Image collectionImg;
    [HideInInspector]
    public CollectionItem collectionItem;

    private void Start()
    {
        collectionImg.sprite = collectionItem.itemIcon;
        for(int i=0;i< DataManager.Instance.myInfo.earnedItem.Count;i++)
        {
            if (collectionItem.itemName == DataManager.Instance.myInfo.earnedItem[i])
            {
                collectionImg.color = new Color(1, 1, 1, 1);
                return;
            }
            else
                collectionImg.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
