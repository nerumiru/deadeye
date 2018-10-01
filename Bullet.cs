using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 총알 오브젝트에 들어가는 클래스
/// </summary>
public class Bullet : MonoBehaviour {
    public float damage;
    public float speed;
    public Sprite sprite;
    public GameObject Explosive;
    public bool isPercentDamage = false;
    public float knockback;
    public bool isExplosion = false;

    /// <summary>
    /// 변수 할당요 함수들.
    /// </summary>
    public void SetBullet(float damage_, float speed_, float knockback_)
    {
        damage = damage_;
        speed = speed_;
        knockback = knockback_;
    }
    public void SetBullet(float damage_, float speed_, float knockback_, GameObject Explosive_)
    {
        damage = damage_;
        speed = speed_;
        knockback = knockback_;
        isExplosion = true;
        Explosive = Explosive_;
    }
    public void SetBullet(float damage_, float speed_, float knockback_, bool PercentDamage)
    {
        damage = damage_;
        speed = speed_;
        knockback = knockback_;
        isPercentDamage = PercentDamage;
    }
    public void SetBullet(float damage_, float speed_, float knockback_, GameObject Explosive_, bool PercentDamage)
    {
        damage = damage_;
        speed = speed_;
        knockback = knockback_;
        isExplosion = true;
        Explosive = Explosive_;
        isPercentDamage = PercentDamage;
    }
    /// <summary>
    /// 
    /// </summary>
    void Update () {
        transform.Translate(0f, speed, 0f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            return;
        }
        if (collision.tag == "enemy")
        {
            EnemyState es = collision.gameObject.GetComponent<EnemyState>();
            if(isPercentDamage) es.SetHitpoint(damage,true);
            else es.SetHitpoint(damage);            
            es.rb2d.AddForce(new Vector2(
                Mathf.Sin(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback, 
                Mathf.Cos(transform.rotation.eulerAngles.z / 180f * Mathf.PI) * knockback
                ));
        }
        Destroy(gameObject);
    }
    
}
