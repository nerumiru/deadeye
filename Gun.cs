using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 총의 정보를 담고있는 클래스.
/// </summary>
public class Gun {
    //v1.1 총알 발사 갯수, 총에서의 탄환 탄퍼짐 정도, 총기 반동 정도, 총기 최대 반동 까지의 시간, 총알 거리, 히트스캔 총알이 사라지는데 걸리는 시간(딜과 관련 없음.)
    //GUN
    public string name;
    public int ammoValue, emissionNum, ammoMax, recoilMax, value;
    public float reloadTime, interval, shotSpread, recoil;
    //BULLET
    //type : p - projectile, h - hitscan
    public string bulletType;
    public int penetrationNum, reflectionNum;
    public float sizeX, sizeY, damage, speed, knockback, range, hitscanTrailTime;
    public bool isPercent, isExplosion;
    public GameObject bullet;
    public GameObject explosion;
    /// <summary>
    /// type : p - projectile, h - hitscan
    /// </summary>
    public Gun(string Name,int GunValue, int AmmoValue, int AmmoMax,int EmissionNum, float ReloadTime, float Interval, float ShotSpread, float Recoil, int RecoilMax)
    {
        name = Name;
        value = GunValue;
        ammoValue = AmmoValue;
        emissionNum = EmissionNum;
        ammoMax = AmmoMax;
        reloadTime = ReloadTime;
        interval = Interval;
        shotSpread = ShotSpread;
        recoil = Recoil;
        if (RecoilMax > 0) recoilMax = 0;
        else recoilMax = RecoilMax;
    }
    /// <summary>
    /// 총알 정보를 초기화하는데 사용
    /// </summary>
    public void MakeBullet(string BulletType, float SizeX, float SizeY, float Damage, float Speed, float Knockback, float Range,float HitscanTrailTime, int PenetrationNum, int ReflectionNum, bool Percent, bool Explosion, GameObject Bullet, GameObject ExplosionObj)
    {
        bulletType = BulletType;
        sizeX = SizeX;
        sizeY = SizeY;
        damage = Damage;
        speed = Speed;
        hitscanTrailTime = HitscanTrailTime;
        penetrationNum = PenetrationNum;
        reflectionNum = ReflectionNum;
        knockback = Knockback;
        isPercent = Percent;
        isExplosion = Explosion;
        bullet = Bullet;
        explosion = ExplosionObj;
        range = Range;
    }
    //총이 가지는 사양. 
    //총 :: 최대총알, 장전시간, 발사간격,
    //총알 :: (타입, 크기, 데미지, 속도, 넉백 ,프리팹이름,) 퍼뎀여부, 폭팔여부, 폭팔시오브젝트, 

}
