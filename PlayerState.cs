using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : SubLevelManager
{
    public Transform tf;
    public GameObject player;

    public int durability = 6;

    public float speedVar = 1f;
    public float gunPowerVar = 1f;
    public float gunIntervalVar = 1f;
    public float gunReloadVar = 1f;
    public float gunRangeVar = 1f;
    public float gunSpeedVar = 1f; //미방영
    public float gunKnockBackVar = 1f;
    public float gunRecoilRecoverTime = 0.5f;//기본값은 0.5f
    public float gunXsize = 1f;//미방영
    public float gunYsize = 1f;//미방영
    public float gunRecoilSize = 1f;
    public float gunSpreadSize = 1f;
    public int gunEmissionNum = 1; // 합적용.
    public int gunEmissionNumVar = 1;// 곱적용.
    public int gunReflectionVar = 0; // 합적용. 미방영
    public int gunPenetrationVar = 0; // 합적용. 미방영

    public bool autoReload = false;

    public int gunNum = 1;
    public string equippedGun1 = "g_0";
    public string equippedGun2 = "none";
    public string equippedColdArm = "c_0";

    public int scrap = 0;
    public int power = 0;
    public int manaMax = 5;
    public int manaNow = 5;
    public List<string> CardSet;

    // Use this for initialization
    public override void InInitialization()
    {
        player = GameObject.Find("Player");
        tf = player.transform;
    }

    public void CreateBullet(int leftValue, int ammoValue)
    {
        scrap = scrap + leftValue - ammoValue;
    }

    public void ChangeScrap(int value)
    {
        scrap = scrap - value;
    }
}
