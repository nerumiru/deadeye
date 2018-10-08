using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour {

    ContactFilter2D cf;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        GameObject ob = collision.gameObject;
      //  collision.collider.
    }
}
