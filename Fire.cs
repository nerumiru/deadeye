using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject father;
    public GameObject shootPoint;

    public float interval = 0.5f;
    public int bulletMax = 5;
    public int bulletNum = 5;
    public int bulletTotal = 25;
    public float intervalNow = 0f;
    public float reloadTime = 1.5f;
    public float reloadNow = 0f;
    public bool reloadComplete = true;


    public void SetGun()
    {
        //총이 가지는 사양. 
        //총 :: 총알타입, 최대총알, 장전시간, 발사간격,
        //총알 :: (타입, 크기, 데미지, 속도, 넉백 ,프리팹이름,) 퍼뎀여부, 폭팔여부, 폭팔시오브젝트, 





    }
// Update is called once per frame
void Update () {
        if (bulletTotal == 0) return;

        if (intervalNow > 0f)
        {
            intervalNow = intervalNow - Time.deltaTime;
            return;
        }
        if (reloadNow > 0f)
        {
            reloadNow = reloadNow - Time.deltaTime;            
            return;
        }
        else if(!reloadComplete && reloadNow <= 0f)
        {
            if ((bulletTotal / bulletMax) > 0)
            {
                bulletNum = bulletMax;
            }
            else
            {
                bulletNum = bulletTotal;
            }

            reloadComplete = true;
        }
        if ((bulletNum < bulletMax && Input.GetKey("r")) || bulletNum < 1)
        {
            if (bulletTotal == 0) return;
            reloadComplete = false;
            reloadNow = reloadTime;
            return;
        }
        if (bulletNum > 0 && Input.GetMouseButton(0))
        {
            intervalNow = interval;
            --bulletNum;
            --bulletTotal;
            Instantiate(bulletObject, shootPoint.transform).transform.SetParent(father.transform);
        }
		
	}
}
