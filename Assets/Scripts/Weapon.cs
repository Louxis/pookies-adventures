using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will grant the basic behaviour of a weapon, like damage, cooldown and hitbox detection.
/// </summary>
public class Weapon : MonoBehaviour
{
    private GameObject pookie;
    private PookieController pookieController;
    private Enemy enemy;
    private int damage;
    private double damageCooldown; //adjust? = 0.3f
    private double nextDamage;

    private void Awake()
    {
        pookie = GameObject.FindGameObjectWithTag("Player");
        pookieController = pookie.GetComponent<PookieController>();
    }

    protected void setDamage(int damage)
    {
        this.damage = damage;
    }

    public int getDamage()
    {
        return damage;
    }

    public void addDamage(int atkBonus)
    {
        damage += atkBonus;
        Debug.Log("Current damage: " + damage + " from " + this.name);
    }

    protected void setDamageCooldown(double damageCooldown)
    {
        this.damageCooldown = damageCooldown;
    }

    /// <summary>
    /// Verifies if the Player is attacking.
    /// </summary>
    /// <returns></returns>
    public bool isAttacking()
    {
        return pookieController.isAttacking();
    }

    public double getDamageCooldown()
    {
        return damageCooldown;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (pookie == null)
            pookie = GameObject.FindGameObjectWithTag("Player");
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = (Enemy)collision.gameObject.GetComponent(typeof(Enemy));
            if (Time.time > nextDamage)
            {
                nextDamage = Time.time + damageCooldown;
                enemy.getDamaged(damage, pookie);
            }
        }

        if (collision.gameObject.tag == "Destructible")
        {
            Destructible destructible = (Destructible)collision.gameObject.GetComponent(typeof(Destructible));
            if (Time.time > nextDamage)
            {
                nextDamage = Time.time + damageCooldown;
                destructible.getDamaged(damage, pookie);
            }
        }

    }
}
