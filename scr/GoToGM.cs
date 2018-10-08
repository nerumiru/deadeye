using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// gm으로 연결하는 싱글턴
/// </summary>
public class GoToGM : Singleton<GoToGM>
{
    public int location = 0; //1 = level, 0 = nowhere

    public GameObject gmObject;
    public LevelGameManager lgm;

    /// <summary>
    /// 최초시행 $$아직지정안함.
    /// </summary>
    public void FirstLoad()
    {
        location = 0;
    }

    public void InLevelGame(GameObject gm)
    {
        location = 1;
        gmObject = gm;
        lgm = gm.GetComponent<LevelGameManager>();
    }
}
