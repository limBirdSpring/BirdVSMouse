using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoYoon
{
    [CreateAssetMenu(menuName = "SoYoon/NameLists")]
    public class InitializeNames : ScriptableObject
    {
        public string[] adjectives;
        public string[] names;
    }
}