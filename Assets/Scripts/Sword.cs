using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic sword class
/// </summary>
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
    
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        SoundManager.instance.playSingle(SoundManager.instance.hit);
    }
}
