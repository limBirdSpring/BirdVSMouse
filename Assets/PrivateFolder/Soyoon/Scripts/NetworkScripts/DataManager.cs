using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField]
        private Datas datas;

        public static DataManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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
    }
}
