using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangSpriteTest : MonoBehaviour {
    public SpriteRenderer sp;
	// Use this for initialization
	void Start () {
        sp.sprite = Resources.Load("Sprites/aim", typeof(Sprite)) as Sprite;

    }
}
