using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunList {
    public Dictionary<string, Gun> gunDic;


    public void LoadGunList()
    {
        gunDic = new Dictionary<string, Gun>();
        //히트스캔의 y, speed는 0으로한다.
        AddGun("CursedDart", "basic", 4, 0.5f, 2f);
        AddBullet("CurseDart", "h", 0.05f, 0f, 2f, 0f, 1f, false, false, "dart", "");
        AddGun("PeaceBreaker", "pistol", 4, 0.5f, 2f);
        AddBullet("PeaceBreaker", "h", 0.1f, 0f, 10f, 0f, 100f, false, false, "pistol_1", "");

    }
    public void AddGun(string Name,string BulletType,int BulletMax, float ReloadTime, float Interval)
    {
        Gun temp = new Gun(Name, BulletType, BulletMax, ReloadTime, Interval);
        gunDic.Add(Name, temp);
    }
    public void AddBullet(string Name, string Type, float SizeX, float SizeY, float Damage, float Speed,float Knockback, bool Percent, bool Explosion, string BulletName, string ExplosionObjName)
    {
        GameObject bullet = Resources.Load("Assets/Prefab/Bullet/" + BulletName, typeof(GameObject)) as GameObject;
        gunDic[Name].MakeBullet(Type, SizeX, SizeY, Damage, Speed, Knockback, Percent, bullet);

        if (Explosion)
        {
            GameObject exObj = Resources.Load("Assets/Prefab/Bullet/" + ExplosionObjName, typeof(GameObject)) as GameObject;
            gunDic[Name].MakeBullet(Type, SizeX, SizeY, Damage, Speed, Knockback, Percent, Explosion, bullet, exObj);

        }

    }

    public Gun GetGun(string name)
    {
        return gunDic[name];
    }
}
