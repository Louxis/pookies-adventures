using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private const int SWORD_DAMAGE = 1;
    private const double SWORD_COOLDOWN = 0.3f;

    // Use this for initialization
    void Start()
    {
        base.setDamage(SWORD_DAMAGE);
        base.setDamageCooldown(SWORD_COOLDOWN);
    }

    // Update is called once per frame
    void Update()
    {

    }



}
