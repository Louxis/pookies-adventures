using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Future Update.
/// </summary>
public class LaserBonusController : MonoBehaviour
{
    private const int COST = 250;
    private const int LASER_ATK_MOD = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PookieController player = (PookieController)collision.gameObject.GetComponent(typeof(PookieController));
            BulletCtrl[] leftLasers = player.getLeftLasers();
            if (player.getCash() >= COST)
            {
                player.removeCash(COST);
                for (int i = 0; i < leftLasers.Length; i++)
                {
                    //leftLasers[i].addDamage(LASER_ATK_MOD);
                    leftLasers[i].updateDamage(LASER_ATK_MOD);
                    Debug.Log(leftLasers[i].getDamage() + " from " + leftLasers[i].name);
                }
                player.SetText();
                Destroy(gameObject);
            }
        }
    }
}
