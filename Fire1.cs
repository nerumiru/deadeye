using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1 : MonoBehaviour
{
    public GameObject projectileBulletObject;
    public GameObject hitscanBulletObject;
    public GameObject father;
    public GameObject shootPoint;

    public AimObject ao;
    [HideInInspector]
    public LevelGameManager gm;
    public PlayerController pc;
    public Gun gun;
    public string gunId = "none";
    public bool loaded = false;
    //총기 변수
    public string gunName = "none";
    public bool isProjectile = false;

    //상태 변수
    public int leftReload = 0;
    //내부변수
    public int ammoNum = 5;
    public float intervalNow = 0f;
    public float reloadNow = 0f;
    public bool reloadComplete = true;
    public bool readyToReload = false;
    float recoilLimmit = 0f;



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
        ammoNum = gm.fieldItemList.GetAmmoNum(gunId);
        //총알 오브젝트 생성
        if (isProjectile) projectileBulletObject.GetComponent<ProjectileBullet>().SetBullet(gun);
        else hitscanBulletObject.GetComponent<HitscanBullet>().SetBullet(gun);
    }
    
    // Update is called once per frame
    void Update () {
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
        if (gun.ammoValue == 0) leftReload = 1;
        else leftReload = ((gun.ammoValue / gun.ammoMax * ammoNum) + gm.playerState.scrap) / gun.ammoValue;

        //발사 사이 간격 시
        if (intervalNow > 0f)
        {
            ao.recoverRecoil = false;
            intervalNow = intervalNow - Time.deltaTime;
            if (leftReload > 0 && Input.GetKeyDown("r"))
                readyToReload = true;
            return;
        }
        else
        {
            //인터벌이후 반동 감소 방영
            if (intervalNow <= 0f) //인터벌 이후 검증
            {
                ao.recoverRecoil = true;
                recoilLimmit = recoilLimmit - (Time.deltaTime * gm.playerState.gunRecoilRecoverTime);
                if (recoilLimmit < 0f) recoilLimmit = 0f;
            }
            else
            {
                ao.recoverRecoil = false;
            }
            intervalNow = intervalNow - Time.deltaTime;
        }
        // 재정시
        if (reloadNow > 0f)
        {
            reloadNow = reloadNow - Time.deltaTime;            
            return;
        }

        // 재장전 완료시
        else if(!reloadComplete && reloadNow <= 0f)
        {
            ao.recoverRecoil = true;
            recoilLimmit = 0f;
            //재장전 반영
            if (leftReload > 0)
            {
                //스크랩방여
                gm.playerState.CreateBullet((gun.ammoValue / gun.ammoMax * ammoNum), gun.ammoValue);
                //총알 방영
                ammoNum = gun.ammoMax;
            }
            reloadComplete = true;
        }
        // 자동 재장전 >> 설정으로 온오프  및 재장전 시도시
        // 총알이 없을 떄 혹은 해당키를 눌렀을때 이루어진다
        if (leftReload > 0) //가능한 남은 장전 횟수
        {
            if (readyToReload == true||
                (ammoNum < gun.ammoMax && Input.GetKey("r")) || //재장전을 눌렀을 때.
                (ammoNum < 1 && Input.GetMouseButton(0)) || //총알이 없을 때 발사가 눌러졌을 떄
                (ammoNum < 1 && (gm.playerState.autoReload == true)))//총알이 없을이 없을 때.
            {
                readyToReload = false;                
                reloadComplete = false;
                reloadNow = gun.reloadTime * gm.playerState.gunReloadVar;

                return;
            }
        }
        // 발사 진행
        if (ammoNum > 0 && Input.GetMouseButton(0))
        {
            //인터벌 생성
            intervalNow = gun.interval * gm.playerState.gunIntervalVar;
            //총알 갯수 방영
            --ammoNum;
            //반동 계산및 방영            
            recoilLimmit = recoilLimmit + (1f / (float)gun.recoilMax);
            if (recoilLimmit > 1f)
            {
                recoilLimmit = 1f;
            }
            float recoilResult = recoilLimmit * gun.recoil * gm.playerState.gunRecoilSize;
            ao.Recoiling(new Vector2(Random.Range(-recoilResult,+recoilResult), Random.Range(-recoilResult, +recoilResult)));
            pc.SetDirection();

            //탄퍼짐 방영한 각도 변수
            for (int i = 0; i < (gun.emissionNum + gm.playerState.gunEmissionNum) * gm.playerState.gunEmissionNumVar; i++)
            {
                float eulerVar = Random.Range(-gun.shotSpread * gm.playerState.gunSpreadSize/ 2f, gun.shotSpread * gm.playerState.gunSpreadSize / 2f);
                Quaternion quaternion = Quaternion.identity;
                quaternion.eulerAngles = new Vector3(0f, 0f, eulerVar + shootPoint.transform.eulerAngles.z);
                //총알 생성
                if (isProjectile) //투사체일떄
                    Instantiate(projectileBulletObject, shootPoint.transform.position, quaternion, father.transform);
                else //히트스캔일떄.
                    Instantiate(hitscanBulletObject, shootPoint.transform.position, quaternion, father.transform);
            }
        }
		
	}
}
