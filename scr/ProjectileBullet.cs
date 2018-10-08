using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public LineRenderer lr;
    public Vector2 endLay;

    //고정변수
    public bool isPercentDamage = false;
    public bool isExplosion = false;
    public int penetrationNum = 0;
    public int reflectionNum = 0;
    public float range = 100f;
    public float speed = 1f;

    public GameObject explosive, bullet;
    public float sizeX, sizeY, damage, knockback;
    //농동 변수
    public bool endShot = false;
    public int penetrationNow = 0;
    public int reflectionNow = 0;
    float distance = 0;
    
    Vector2 startPoint;
    Vector2 direction;
    Vector2 endPoint;
    Vector2 inOrigin;
    Vector2 beforPoint;
    /// <summary>
    /// 변수 할당요 함수들.
    /// </summary>
    public void SetBullet(Gun Gun)
    {
        isPercentDamage = Gun.isPercent;
        isExplosion = Gun.isExplosion;
        penetrationNum = Gun.penetrationNum;
        reflectionNum = Gun.reflectionNum;
        range = Gun.range;

        if (isExplosion) explosive = Gun.explosion;
        bullet = Gun.bullet;
        sizeX = Gun.sizeX;
        sizeY = Gun.sizeY;
        speed = Gun.speed;
        damage = Gun.damage;
        knockback = Gun.knockback;
    }
    private void Start()
    {
        PlayerState ps = GoToGM.Instance.lgm.playerState;

        range = range * ps.gunRangeVar;
        sizeX = sizeX * ps.gunXsize;
        sizeY = sizeY * ps.gunYsize;
        reflectionNum = reflectionNum + ps.gunReflectionVar;
        penetrationNum = penetrationNum + ps.gunPenetrationVar;
        knockback = knockback * ps.gunKnockBackVar;
        speed = speed * ps.gunSpeedVar;
        if (!isPercentDamage) damage = damage * ps.gunPowerVar;

        //여기부터하애햐
        startPoint = transform.position;
        direction = new Vector2(transform.up.x, transform.up.y);
        endPoint = startPoint;
        inOrigin = direction;

        transform.localScale = new Vector3(sizeX, sizeY, 0f);
    }
    /// <summary>
    /// 총알을 움직이는데 사용.
    /// </summary>
    void Update()
    {
        transform.Translate(0f, speed, 0f);
        

        if (penetrationNow > penetrationNum) endShot = true;
        if (distance >= range) endShot = true;
        else
        {
            distance = distance + Vector2.Distance(beforPoint, transform.position);
            beforPoint = transform.position;
        }
        if (endShot) Destroy(gameObject);
    }

    /// <summary>
    /// 충돌 확인에 사용
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "object")
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
            if (isExplosion) Instantiate(explosive, transform.position, qt);

            penetrationNow = penetrationNow + 1;
            return;
        }
        else if (collider.tag == "enemy")
        {
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
            if (isExplosion) Instantiate(explosive, transform.position, qt);

            penetrationNow = penetrationNow + 1;
            EnemyState es = collider.gameObject.GetComponent<EnemyState>();
            if (isPercentDamage) es.SetHitpoint(damage, true);
            else es.SetHitpoint(damage);
            es.rb2d.AddForce(new Vector2(
                Mathf.Sin(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback,
                Mathf.Cos(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback
                ));
        }
        else if (collider.tag == "wall")
        {
            if (reflectionNow >= reflectionNum)
            {
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);

                if (isExplosion) Instantiate(explosive, transform.position, qt);

                endShot = true;
            }
            else
            {
                reflectionNow = reflectionNow + 1;

                RaycastHit2D[] hit;
                hit = Physics2D.RaycastAll(startPoint, direction, range);
                for (int i = 0; i < hit.Length; i++)
                {
                    if (collider.Equals(hit[i]))
                    {
                        //전처리
                        inOrigin = hit[i].normal;
                        endPoint = hit[i].point;
                        direction = Vector2.Reflect(direction, inOrigin);
                        //적용
                        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                        Quaternion qt = Quaternion.AngleAxis(angle, Vector3.forward);
                        transform.rotation = qt;
                        //후처리
                        startPoint = endPoint + (direction * 0.1f);
                        endPoint = startPoint;
                        break;
                    }
                }

            }

        }
    }
}
