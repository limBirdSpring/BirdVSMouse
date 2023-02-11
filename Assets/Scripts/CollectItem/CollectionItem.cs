using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ItemType
{
    Photo,
    Badge
}

public enum ItemGrade
{
    Normal,//일반템
    Special,//일반캐시템
    Unique//한정캐시템
}

[CreateAssetMenu(menuName = "CollectItem")]
public class CollectionItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int price;
    public ItemType type;
    public ItemGrade grade;
}