using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected void setDamageCooldown(double damageCooldown)
    {
        this.damageCooldown = damageCooldown;
    }

    public double getDamageCooldown()
    {
        return damageCooldown;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = (Enemy)collision.gameObject.GetComponent(typeof(Enemy));
                if (Time.time > nextDamage)
                {
                    nextDamage = Time.time + damageCooldown;
                    enemy.getDamaged(damage, pookie); //TO-DO damage modifier;
                }
        }
    }
}
