using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Singleton<PlayerState>
{
    public Transform tf;
    public GameObject player;

    public int durability = 6;

    public float speedVar = 1f;
    public float gunPowerVar = 1f;
    public float gunIntervalVar = 1f;
    public float gunReloadVar = 1f;
    public float gunSpeedVar = 1f;
    public float gunKnockBackVar = 1f;



    public string equip_gun_1 = "g_0000";
    public string equip_gun_2 = "none";
    public string equip_coldarm = "c_0000";

    public int scrap = 0;
    public int power = 0;
    public int manaMax = 5;
    public int manaNow = 5;
    public List<string> CardSet;

    // Use this for initialization

    public void Initialization() {
        player = GameObject.Find("Player");
        tf = player.transform;
	}

}
