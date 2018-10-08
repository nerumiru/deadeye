using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanBullet : MonoBehaviour
{
    public LineRenderer lr;
    public Vector2 endLay;

    //고정변수
    public bool isPercentDamage = false;
    public bool isExplosion = false;
    public float deadTime = 0.5f;
    public float range = 100f;
    public int penetrationNum = 0;
    public int reflectionNum = 0;

    public GameObject explosive, bullet;
    public float sizeX, damage, knockback;
    //유동변수
    public bool endShot = false;
    public int penetrationNow = 0;
    public int reflectionNow = 0;
    public float timeNow = 0f;


    /// <summary>
    /// 변수 할당요 함수들.
    /// </summary>
    public void SetBullet(Gun Gun)
    {
        isPercentDamage = Gun.isPercent;
        isExplosion = Gun.isExplosion;
        penetrationNum = Gun.penetrationNum;
        reflectionNum = Gun.reflectionNum;
        deadTime = Gun.hitscanTrailTime;
        range = Gun.range;

        if (isExplosion) explosive = Gun.explosion;
        bullet = Gun.bullet;
        sizeX = Gun.sizeX;
        damage = Gun.damage;
        knockback = Gun.knockback;
    }
    private void Start()
    {
        //playerState방영
        PlayerState ps = GoToGM.Instance.lgm.playerState;        
        range = range * ps.gunRangeVar;
        sizeX = sizeX * ps.gunXsize;
        reflectionNum = reflectionNum + ps.gunReflectionVar;
        penetrationNum = penetrationNum + ps.gunPenetrationVar;
        knockback = knockback * ps.gunKnockBackVar;
        if(!isPercentDamage) damage = damage * ps.gunPowerVar;


        //1 벽의 포인트를 구한다.
        Vector2 startPoint = transform.position;
        Vector2 direction = new Vector2(transform.up.x, transform.up.y);
        RaycastHit2D[] hit;
        Vector2 endPoint = startPoint;
        Vector2 inOrigin = direction;

        while (!endShot)
        {
            int i = 0;
            hit = Physics2D.RaycastAll(startPoint, direction, range);

            //벽 확인
            for (;;)
            {
                if (i >= hit.Length)
                {
                    endPoint = startPoint + direction * range;
                    break;
                }

                if (hit[i].collider.tag == "wall")
                {
                    //벽을 찾으면 벽과 닿은 점을 저장하고 없을시 그끝을 저장한다.
                    inOrigin = hit[i].normal;
                    endPoint = hit[i].point;
                    break;
                }
                else
                {
                    i = i + 1;
                }
            }
            float distance = Vector2.Distance(startPoint, endPoint);
            i = 0;
            //충돌 확인
            hit = Physics2D.BoxCastAll(startPoint - direction*0.01f, new Vector3(sizeX, 0.01f), 0f, direction, distance);
            for (;;)
            {
                if (i >= hit.Length) break;

                if (hit[i].collider.tag == "enemy" || hit[i].collider.tag == "object")
                {
                    //2d 상황에서 방향 벡터를 쿼터리언으로 변환
                    float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                    Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);

                    if (isExplosion) Instantiate(explosive,hit[i].point, qt);
                    if (hit[i].collider.tag == "enemy") HitEnemy(hit[i].collider.gameObject, qt);
                    else if (hit[i].collider.tag == "object") BreakObject(hit[i].collider.gameObject);

                    if (penetrationNow >= penetrationNum)
                    {
                        endShot = true;
                        endPoint = hit[i].centroid;
                        break;
                    }
                    penetrationNow = penetrationNow + 1;
                }
                i = i + 1;
            }

            //라이너 생성            
            float angle2 = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion qt2 = Quaternion.AngleAxis(angle2, Vector3.forward);
            GameObject go = Instantiate(bullet, startPoint,qt2, transform);
            LineRenderer lr = go.GetComponent<LineRenderer>();          

            lr.startWidth = sizeX;
            lr.endWidth = sizeX;
            lr.positionCount = 2;
            lr.SetPosition(0, startPoint - (direction * 0.1f));
            lr.SetPosition(1, endPoint);

            /*
             * 게임 오브젝트 안에 직접 넣을 파티클 시스템 수치 비정확
            ParticleSystem particle = go.GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule shapeModule = go.GetComponent<ParticleSystem>().shape;
            ParticleSystemRenderer particleR = go.GetComponent<ParticleSystemRenderer>();
            +벨로시티 오버라이프 타임으로 파티클의 움직일 방향은ㄹ 넣어야한다.
            particle.startRotation = angle2 + 90f;
            shapeModule.radius = distance / 2f;
            particleR.pivot = new Vector3(0f, distance / 2f,0f);
            */



            if (reflectionNow >= reflectionNum)
            {
                endShot = true;
            }
            else
            {
                if (endShot) continue;
                //다음 반사가 있을 시
                reflectionNow = reflectionNow + 1;
                direction = Vector2.Reflect(direction, inOrigin);
                range = range - distance;
                if (range <= 0) endShot = true;
                startPoint = endPoint + (direction * 0.1f);
                endPoint = startPoint;
            }
            
        }

    }

    

    private void HitEnemy(GameObject gameObject, Quaternion quaternion)
    {
        EnemyState es = gameObject.GetComponent<EnemyState>();
        // 물리부분
        es.rb2d.AddForce(new Vector2(
            Mathf.Sin(quaternion.eulerAngles.z / 180f * Mathf.PI) * knockback,
            Mathf.Cos(quaternion.eulerAngles.z / 180f * Mathf.PI) * knockback
            ));
        // 데미지 부분
        if (isPercentDamage) es.SetHitpoint(damage , true);
        else es.SetHitpoint(damage);
        
    }

    private void BreakObject(GameObject gameObject)
    {

        ObjectState os = gameObject.GetComponent<ObjectState>();
        /*
        // 물리부분
        es.rb2d.AddForce(new Vector2(
            Mathf.Sin(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback,
            Mathf.Cos(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback
            ));
        // 데미지 부분
        if (isPercentDamage) es.SetHitpoint(damage, true);
        else es.SetHitpoint(damage);
        */
    }

    /// <summary>
    /// 총알을 움직이는데 사용.
    /// </summary>
    void Update()
    {
        if (timeNow > deadTime) Destroy(gameObject);
        timeNow = timeNow + Time.deltaTime;
    }


}
