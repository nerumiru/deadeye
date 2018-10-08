using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : MonoBehaviour {
    public LevelGameManager gm;
    public float speed;
    public float accSpeed;
    public float approach;
    // Use this for initialization
    void Start()
    {
        gm = GoToGM.Instance.lgm;
        speed = 0.01f;
        accSpeed = 3;
        approach = 3f;
    }

    // Update is called once per frame
    void Update () {
        
        float dir = (gm.playerState.tf.position - transform.position).magnitude;

        if (dir > approach)
        {
            transform.position = Vector2.MoveTowards(transform.position, gm.playerState.tf.position, speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, gm.playerState.tf.position, speed * accSpeed);
        }
        
    }
}
