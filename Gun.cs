using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun {
    //GUN
    public string name, bulletType;
    public int bulletMax;
    public float reloadTime, interval;
    //BULLET
    //type : p - projectile, h - hitscan
    public string type;
    public float sizeX, sizeY, damage, speed, knockback;
    public bool isPercent, isExplosion;
    public GameObject bullet;
    public GameObject explosion;

    public Gun(string Name, string BulletType, int BulletMax, float ReloadTime, float Interval)
    {
        name = Name;
        bulletType = BulletType;
        bulletMax = BulletMax;
        reloadTime = ReloadTime;
        interval = Interval;
    }

    public void MakeBullet(string Type, float SizeX, float SizeY, float Damage, float Speed, float Knockback, bool Percent, bool Explosion, GameObject Bullet, GameObject ExplosionObj)
    {
        type = Type;
        sizeX = SizeX;
        sizeY = SizeY;
        damage = Damage;
        speed = Speed;
        knockback = Knockback;
        isPercent = Percent;
        isExplosion = Explosion;
        bullet = Bullet;
        explosion = ExplosionObj;
    }
    public void MakeBullet(string Type, float SizeX, float SizeY, float Damage, float Speed, float Knockback, bool Percent, GameObject Bullet)
    {
        type = Type;
        sizeX = SizeX;
        sizeY = SizeY;
        damage = Damage;
        speed = Speed;
        knockback = Knockback;
        isPercent = Percent;
        isExplosion = false;
        bullet = Bullet;
    }
    public void MakeBullet(string Type, float SizeX, float SizeY, float Damage, float Speed, float Knockback, GameObject Bullet)
    {
        type = Type;
        sizeX = SizeX;
        sizeY = SizeY;
        damage = Damage;
        speed = Speed;
        knockback = Knockback;
        isPercent = false;
        isExplosion = false;
        bullet = Bullet;
    }
    //총이 가지는 사양. 
    //총 :: 최대총알, 장전시간, 발사간격,
    //총알 :: (타입, 크기, 데미지, 속도, 넉백 ,프리팹이름,) 퍼뎀여부, 폭팔여부, 폭팔시오브젝트, 

}
