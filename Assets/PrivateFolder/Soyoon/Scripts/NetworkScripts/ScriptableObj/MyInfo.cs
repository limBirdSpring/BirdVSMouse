using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    [CreateAssetMenu(menuName = "SoYoon/MyInfo")]
    public class MyInfo : ScriptableObject
    {
        public Sprite[] collectedBadges;
        public Sprite[] collectedCharacters;
        public Sprite[] collectedFrames;    // frame 미추가시 삭제

        public string lastChosenName;
        public Sprite lastChosenBadge1;
        public Sprite lastChosenBadge2;
        public Sprite lastChosenCharacter;
        public Sprite lastChosenFrame;      // frame 미추가시 삭제
    }
}
