using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class allows objects to be destroyed after a certain amount of damage.
/// </summary>
public class Destructible : MonoBehaviour
{
    /// <summary>
    /// Object that can be dropped after being destroyed
    /// </summary>
    public GameObject coin;
    private const int DROP_CHANCE = 75;
    int objLife;
    Transform pos;
    // Use this for initialization
    void Start()
    {
        pos = GetComponent<Transform>();
    }

    /// <summary> Sets the object life. </summary>
    /// <param name="life"></param>
    public void setObjectLife(int life)
    {
        objLife = life;
    }
    /// <summary> Sets the object position. </summary>
    /// <param name="pos"></param>
    public void setPosition(Transform pos)
    {
        this.pos = pos;
    }
    /// <summary> Gets the object life. </summary>
    /// <returns></returns>
    public int getLife()
    {
        return objLife;
    }

    /// <summary>Damages the object. </summary>
    /// <param name="damage">Amount of damage.</param>
    /// <param name="damager">Who inflicts the dmage.</param>
    public void getDamaged(int damage, GameObject damager)
    {
        objLife -= damage;
        if (objLife <= 0)
        {
            if (Random.Range(1, 101) <= DROP_CHANCE)
                Instantiate(coin, pos.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("laser"))
        {
            BulletCtrl laser = (BulletCtrl)collider.gameObject.GetComponent(typeof(BulletCtrl));
            getDamaged(laser.getDamage(), collider.gameObject);
            Destroy(collider.gameObject);
        }
    }
}
