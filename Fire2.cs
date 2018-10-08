using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire2 : MonoBehaviour
{

    //ammoMax 재방영 필요
    public LevelGameManager gm;
    public GameObject projectileBulletObject;
    public GameObject hitscanBulletObject;

    public GameObject father;
    public GameObject shootPoint;

    public Gun gun;
    public string gunId = "none";
    public bool loaded = false;
    //총기 변수

    public string gunName = "none";
    public bool isProjectile = false;
    public int ammoValue = 1;
    public float reloadTime = 1.5f;
    public float interval = 0.5f;
    public int ammoMax = 5;
    //상태 변수
    public int leftReload = 0;
    //내부변수
    public int ammoNum = 5;
    public float intervalNow = 0f;
    public float reloadNow = 0f;
    public bool reloadComplete = true;

    public void Initialization()
    {
        gm = GoToGM.Instance.lgm;
        SetGun();
    }
    public void SetGun()
    {
        //착용장비확인
        gunId = gm.playerState.equippedGun1;
        if (gunId == "none") return;
        //장비 아디로 총을 확인하고 가져오기
        gunName = gm.fieldItemList.GetName(gunId);
        gun = gm.gunList.GetGun(gunName);
        //총기 변수 로드
        if (gun.bulletType == "p") isProjectile = true;
        ammoValue = gun.ammoValue;
        reloadTime = gun.reloadTime;
        interval = gun.interval;
        ammoMax = gun.ammoMax;
        ammoNum = gm.fieldItemList.GetAmmoNum(gunId);
    }

    // Update is called once per frame
    void Update()
    {
        //플레이중인지 확인
        if (!gm.isPlaying())
            return;
        //총이 없으면
        if (gunId == "none")
            return;
        //장비중인 총인지 확인
        if (gm.playerState.gunNum != 1) return;
        //총알과 발사 간격, 재장전을 따진다.
        //만들 수 있는 총알 확인
        leftReload = ((ammoValue / ammoMax * ammoNum) + gm.playerState.scrap) / ammoValue;

        //발사 사이 간격 시
        if (intervalNow > 0f)
        {
            intervalNow = intervalNow - Time.deltaTime;
            return;
        }
        // 재정시
        if (reloadNow > 0f)
        {
            reloadNow = reloadNow - Time.deltaTime;
            return;
        }

        // 재장전 완료시
        else if (!reloadComplete && reloadNow <= 0f)
        {
            //재장전 반영
            if (leftReload > 0)
            {
                //스크랩방여
                gm.playerState.CreateBullet((ammoValue / ammoMax * ammoNum), ammoValue);
                //총알 방영
                ammoNum = ammoMax;
            }
            reloadComplete = true;
        }
        // 자동 재장전 >> 설정으로 온오프  및 재장전 시도시
        // 총알이 없을 떄 혹은 해당키를 눌렀을때 이루어진다
        if (leftReload > 0) //가능한 남은 장전 횟수
        {
            if ((ammoNum < ammoMax && Input.GetKey("r")) || //재장전을 눌렀을 때.
                (ammoNum < 1 && Input.GetMouseButton(0)) || //총알이 없을 때 발사가 눌러졌을 떄
                (ammoNum < 1 && (gm.playerState.autoReload == true)))//총알이 없을이 없을 때.
            {
                if (leftReload == 0) return;

                reloadComplete = false;
                reloadNow = reloadTime;

                return;
            }
        }
        // 발사 진행
        if (ammoNum > 0 && Input.GetMouseButton(0))
        {
            //인터벌 생성
            intervalNow = interval;
            //총알 갯수 방영
            --ammoNum;
            //총알 생성
            if (isProjectile) //투사체일떄
                Instantiate(projectileBulletObject, shootPoint.transform).transform.SetParent(father.transform);
            else //히트스캔일떄.
            {

            }
        }

    }
}
