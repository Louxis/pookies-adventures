using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy {

    private const int RAT_HEALTH = 3;
    private const int RAT_DAMAGE = 1;
    private const float RAT_SPEED = 3.0f;
    private const int RAT_SIGHT = 20;

    // Use this for initialization
    void Start () {
        base.setHealth(RAT_HEALTH);
        base.setDamage(RAT_DAMAGE);
        base.setMovementSpeed(RAT_SPEED);
        base.setSightRange(RAT_SIGHT);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
