using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float hitpointMax = 10f;
    public float hitpointNow = 10f;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (hitpointNow <= 0f)
        {
            Destroy(gameObject);
        }		
	}

    public void SetHitpoint(float damage)
    {
        hitpointNow = hitpointNow - damage;
    }
    public void SetHitpoint(float damage,bool isPercent)
    {
        if (damage > 100f) damage = 100f;

        hitpointNow = hitpointNow - (hitpointMax / 100 * damage);
    }
}
