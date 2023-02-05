using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoteManager : MonoBehaviourPun
{
    public static VoteManager Instance;

    public List<int> participantList = new List<int>();
    public List<int> spectatorList = new List<int>();
    public List<int> deadList = new List<int>();

    public bool deadBodyFinder = false;

    private void Awake()
    {
        Instance = this;
    }


    public void Vote(int target)
    {
        
    }
}
