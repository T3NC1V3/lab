using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playermanager;
    public GameObject player;

    private void Awake()
    {
        if (playermanager == null)
        {
            playermanager = this;
        }
    }
}
