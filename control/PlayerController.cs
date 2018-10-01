using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Transform mt, dt;
    public Rigidbody2D rb2d;
    public float speed, velocity;
    public Vector2 vectorDir;
	// Use this for initialization
	void Start () {
        speed = 0.1f;
        vectorDir = Vector2.zero;

    }
    public void Power(Vector2 dir, float velocity_)
    {
        //rb2d.velocity.x = 
        float xx = (vectorDir.x * velocity) + dir.x * velocity_;
        float yy = vectorDir.y * velocity + dir.y * velocity_;

    }
	// Update is called once per frame
	void Update ()
    {
        /*
        Vector2 move = Vector2.zero;
        

        if (Input.GetKey("a"))
        {
            move.x += -1f;
        }
        if (Input.GetKey("d"))
        {
            move.x += +1f;
        }
        if (Input.GetKey("w"))
        {
            move.y += +1f;
        }
        if (Input.GetKey("s"))
        {
            move.y += -1f;
        }
        
        

        move = move * speed;
        rb2d.position = rb2d.position + move;


        */
        float  x = 0f , y = 0f;
        //스피드 판정
        if (Input.GetKey("w"))
        {
            y = speed;
        }
        if (Input.GetKey("s"))
        {
            y = -speed;
        }
        if (Input.GetKey("a"))
        {
            x = -speed;
        }
        if (Input.GetKey("d"))
        {
            x = speed;
        }
        //벡터 판덩

        mt.Translate(x,y, 0f);
        //dt.localEulerAngles = new Vector3(0f, 0f, dt.localEulerAngles.z + Input.GetAxis("Mouse X") * -10f + Input.GetAxis("Mouse Y") * -10f);
        //Camera.main.ScreenToWorldPoint(Input.mousePosition) r
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        dt.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);


        //Vector3 vector = new Vector3(Mathf.Sin(dt.rotation.eulerAngles.z / 180f * Mathf.PI), Mathf.Cos(dt.rotation.eulerAngles.z / 180f * Mathf.PI), 0f);
        


    }
}
