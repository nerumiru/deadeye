using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 point = Camera.main.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta + MouseVar();
        
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }

    private Vector3 MouseVar()
    {
        //100픽셀은 1유닛
        float x, y;
        float tempX = Screen.width / 2f;
        float tempY = Screen.height / 2f;

        //사이즈계산
        float sizeX = (tempX * 0.8f) / 100f;
        float sizeY = (tempY * 0.8f) / 100f;

        //마우스 좌표를 가져옴
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
        //마우스 좌표의 한계값을 적용        
        if (x > tempX * 2f) x = tempX * 2;
        else if (x < 0f) x = 0f;
        if (y > tempY * 2f) y = tempY * 2;
        else if (y < 0f) y = 0f;
        //중앙과의 거리를 계산
        x = x - tempX;
        y = y - tempY;
        //거리의 퍼센티지를 계산
        tempX = Mathf.Abs(x) / tempX;
        tempY = Mathf.Abs(y) / tempY;
        //퍼센티지에 따른 좌표 계산
        tempX = Mathf.Lerp(0f, sizeX, tempX);
        tempY = Mathf.Lerp(0f, sizeY, tempY);
        if (x < 0)
            x = -tempX;
        else
            x = tempX;
        if (y < 0)
            y = -tempY;
        else
            y = tempY;
        return new Vector3(x,y,0f);
    }
}
