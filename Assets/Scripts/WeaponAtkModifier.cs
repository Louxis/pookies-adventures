using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifier to Weapon Attack.
/// </summary>
public class WeaponAtkModifier : MonoBehaviour
{
    private const int COST = 250;
    private const int WEAPON_ATK_MOD = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PookieController player = (PookieController)collision.gameObject.GetComponent(typeof(PookieController));
            Weapon sword = (Weapon)player.GetComponentInChildren<Weapon>();
            if (player.getCash() >= COST)
            {
                player.removeCash(COST);
                sword.addDamage(WEAPON_ATK_MOD);
                player.SetText();
                SoundManager.instance.playSingle(SoundManager.instance.powerup);
                Destroy(gameObject);
                Debug.Log(sword.getDamage());
                Debug.Log(player.getAtkModifier());
            }
        }
    }

}
