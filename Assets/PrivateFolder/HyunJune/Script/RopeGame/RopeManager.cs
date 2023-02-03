using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    private RopeGame[] ropeGames;

    public void ResetRope()
    {
        ropeGames = GetComponentsInChildren<RopeGame>();
        
        for (int i = 0; i < ropeGames.Length; i++)
        {
            ropeGames[i].RopeReset();
        }
    }
}
