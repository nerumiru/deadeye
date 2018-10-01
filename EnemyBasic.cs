using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour {
    public float speed;
    public float accSpeed;
    public float approach;
	// Use this for initialization
	void Start () {
      speed = 0.01f;
      accSpeed = 3;
      approach = 3f;
    }
	
	// Update is called once per frame
	void Update () {
        float dir = (PlayerState.Instance.tf.position - transform.position).magnitude;

        if (dir > approach)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerState.Instance.tf.position, speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerState.Instance.tf.position, speed * accSpeed);
        }



    }
}
