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
    Normal,//�Ϲ���
    Special,//�Ϲ�ĳ����
    Unique//����ĳ����
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