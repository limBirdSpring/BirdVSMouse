using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saebom;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class CowManager : Mission
{
    private PhotonView photon;

    private float birdCowCount;
    private float mouseCowCount;

    public void ResetGame()
    {
        
    }

    public void AddCow(bool isbirdHouse)
    {
        if(isbirdHouse)
            birdCowCount++;
        else
            mouseCowCount++;
    }

    public void DeleteCow(bool isbirdHouse)
    {
        if (isbirdHouse)
            birdCowCount--;
        else
            mouseCowCount--;
    }
}
