using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 서브 메니저는 메인 메니져에 의해 초기화 되고 메인메니저를 통해 사용되는 매니져들이다.
/// 서브 매니져는 이 함수를 상속하여만든다.
/// 다른곳에 사용시 LevelGameManager부분을 바꿔야 함
/// </summary>
public class SubLevelManager : MonoBehaviour {

    public LevelGameManager gm;

    // Use this for initialization
    public virtual void InInitialization()
    {
    }
    public void Initialization(LevelGameManager _gm, Action<bool> callback)
    {
        gm = _gm;
        InInitialization();
        callback(true);
    }
}
