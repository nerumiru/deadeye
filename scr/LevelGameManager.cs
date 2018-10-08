using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameManager : MonoBehaviour
{

    //fire 2개는 직접 연결 해야함.
    public Fire1 fire1;
    public Fire2 fire2;
    [HideInInspector]
    public FieldItemList fieldItemList;
    [HideInInspector]
    public PlayerState playerState;
    public GunList gunList;

    private bool playing = false;
    private bool pause = false;
    private bool loaded = false;
    private bool started = false;
    private bool loadedFieldList = false;
    private bool loadedState = false;
    private void Awake()
    {
        PlayerPrefs.SetString("language", "Lang/lang_kor.json");
        PlayerPrefs.Save();
        //위치 게임시할떄로 바꿔야함.
        LocalizationManager.Instance.Initialization();

        gunList = new GunList();
        GoToGM.Instance.InLevelGame(this.gameObject);
    }

    void Start ()
    {
        playerState = GetComponentInChildren<PlayerState>();
        playerState.Initialization(this, (bool end) => loadedState = end);
        fieldItemList = GetComponentInChildren<FieldItemList>();
        fieldItemList.Initialization(this, (bool end) => loadedFieldList = end);
        fire1.Initialization();
    }
    
    private bool CheckLoad()
    {
        if (!loadedState 
            && !loadedFieldList
            ) return false;
        return true;
    }

    public bool isPlaying()
    {
        return playing;
    }
    // Update is called once per frame
    void Update () {

        if (!loaded)
        {
            loaded = CheckLoad();
            return;
        }
        if (!started)
        {
            started = true;
        }
        if (!playing && !pause)
        {
            playing = true;
        }

    }
}
