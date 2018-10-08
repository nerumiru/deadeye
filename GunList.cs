using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunList {
    // loadgunlist를 사용하여 시작 gunddic에서 gun을 가져와 정보로 쓴다.
    public Dictionary<string, Gun> gunDic;

    public GunList()
    {
        LoadGunList();
    }
    public void LoadGunList()
    {
        gunDic = new Dictionary<string, Gun>();
        //히트스캔의 y, speed는 0으로한다.
        gunDic.Add("CursedDartGun",new Gun("CursedDartGun",100,0,5,1,1.5f,0.2f,0f,0f,1));
        AddBullet("CursedDartGun", "h",0.05f,0f,2f,0f,0f,30f,0.4f,0,0,false,false,"dart","dartXF");
        gunDic.Add("PeaceBreaker", new Gun("PeaceBreaker",200 ,0, 5,1, 1.5f, 0.5f, 0f, 5f, 3));
        AddBullet("PeaceBreaker", "h", 0.05f, 0f, 2f, 0f, 0f, 30f, 0.4f, 0, 0, false, true, "dart", "dartXF");

    }
    public void AddBullet(string Name, string BulletType, float SizeX, float SizeY, float Damage, float Speed, float Knockback, float Range, float HitscanTrailTime, int PenetrationNum, int ReflectionNum, bool Percent, bool Explosion, string Bullet, string ExplosionObj)
    {
        GameObject bullet = Resources.Load("Prefab/Bullet/" + Bullet, typeof(GameObject)) as GameObject;
        GameObject exObj = null;
        if (Explosion)
            exObj = Resources.Load("Prefab/Explosive/" + ExplosionObj, typeof(GameObject)) as GameObject;
        gunDic[Name].MakeBullet(BulletType, SizeX, SizeY, Damage, Speed, Knockback,Range,HitscanTrailTime, PenetrationNum, ReflectionNum, Percent, Explosion, bullet, exObj);

    }
    
    public Gun GetGun(string name)
    {
        return gunDic[name];
    }
}
