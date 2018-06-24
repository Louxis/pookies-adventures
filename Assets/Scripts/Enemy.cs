using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that provides the basic stats for all the enemies.
/// </summary>
public class Enemy : MonoBehaviour
{
    private PookieController pookie;

    private int health; //amount of hp one has
    private int damage;
    private float movementSpeed;
    private int sightRange;

    protected void setHealth(int health)
    {
        this.health = health;
    }

    protected void setDamage(int damage)
    {
        this.damage = damage;
    }

    protected void setMovementSpeed(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }

    protected void setSightRange(int sightRange)
    {
        this.sightRange = sightRange;
    }

    public int getHealth()
    {
        return health;
    }

    public int getDamage()
    {
        return damage;
    }

    public float getMovementSpeed()
    {
        return movementSpeed;
    }

    public int getSightRange()
    {
        return sightRange;
    }

    /// <summary>
    /// Gets damaged by some object with a certain amount of damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damager"></param>
    public void getDamaged(int damage, GameObject damager)
    {
        health -= damage;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();
        Vector2 actualPosition = body.transform.position;
        Vector2 orientation = (actualPosition - (Vector2)damager.transform.position).normalized;
        //body.velocity = new Vector2(0f, 0f);
        body.AddForce(orientation * 100000);
        if (health <= 0)
        {
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Returns if the enemy is alive.
    /// </summary>
    /// <returns></returns>
    public bool isAlive()
    {
        return health > 0;
    }
}
