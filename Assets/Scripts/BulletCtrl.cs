using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile controller
/// </summary>
public class BulletCtrl : Weapon
{
    public int speed;
    Rigidbody2D rb2d;
    private const int LASER_DAMAGE = 1;
    private const int LASER_COOLDOWN = 0;
    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(speed, 0f);
        base.setDamage(LASER_DAMAGE);
        base.setDamageCooldown(LASER_COOLDOWN);
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = new Vector2(speed, 0f);
    }
    /// <summary>
    /// Updates the projectile damage
    /// </summary>
    /// <param name="damageMod">Damage Modifier</param>
    public void updateDamage(int damageMod)
    {
        base.setDamage(LASER_DAMAGE + damageMod);
    }

    /// <summary>
    /// Destroys the laser if it gets into contact with something
    /// </summary>
    /// <param name="collider"></param>
    public new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if (collider.gameObject.CompareTag("wall") || collider.gameObject.CompareTag("Destructible") || collider.gameObject.CompareTag("Enemy"))
        {
            SoundManager.instance.playSingle(SoundManager.instance.hit);
            Destroy(transform.parent.gameObject);
        }

    }
}
