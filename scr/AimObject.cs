using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObject : MonoBehaviour {

    Vector2 mousePosition;
    Vector2 recoilVar;
    Vector2 recoilTemp = Vector2.zero;
    private Vector3 velocity = Vector3.zero;
    public bool recoverRecoil = false;
    // Use this for initialization
    void Start ()
    {

    }

    private void FixedUpdate()
    {
        //회복되는 내용이 있음
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
        if ((recoilTemp.x != 0f || recoilTemp.y != 0f) && recoverRecoil)
        {
            //x 부분
            if (recoilTemp.x > 0f)
            {
                recoilTemp.x = recoilTemp.x - (recoilVar.x * (Time.fixedDeltaTime / GoToGM.Instance.lgm.playerState.gunRecoilRecoverTime));
                if (recoilTemp.x < 0f) recoilTemp.x = 0f;
            }
            else if (recoilTemp.x < 0f)
            {
                recoilTemp.x = recoilTemp.x - (recoilVar.x * (Time.fixedDeltaTime / GoToGM.Instance.lgm.playerState.gunRecoilRecoverTime));
                if (recoilTemp.x > 0f) recoilTemp.x = 0f;
            }
            //y부분
            if (recoilTemp.y> 0f)
            {
                recoilTemp.y = recoilTemp.y - (recoilVar.y * (Time.fixedDeltaTime / GoToGM.Instance.lgm.playerState.gunRecoilRecoverTime));
                if (recoilTemp.y < 0f) recoilTemp.y = 0f;
            }
            else if (recoilTemp.y < 0f)
            {
                recoilTemp.y = recoilTemp.y - (recoilVar.y * (Time.fixedDeltaTime / GoToGM.Instance.lgm.playerState.gunRecoilRecoverTime));
                if (recoilTemp.y > 0f) recoilTemp.y = 0f;
            }
        }

        transform.position = mousePosition + recoilTemp;
        //transform.Translate(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f));
    }

    // Update is called once per frame
    void Update ()
    {
    }
    public void Recoiling(Vector2 RecoilVar)
    {
        recoilVar = RecoilVar;
        recoilTemp = RecoilVar;
        transform.position = mousePosition + RecoilVar;
    }
}
