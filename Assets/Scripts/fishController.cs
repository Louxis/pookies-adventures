using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modifier to Pookie Attack.
/// </summary>
public class fishController : MonoBehaviour
{
    private const int COST = 250;
    private const int ATK_MOD = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PookieController player = (PookieController)collision.gameObject.GetComponent(typeof(PookieController));
            if (player != null)
            {
                Weapon sword = (Weapon)player.GetComponentInChildren<Weapon>();
                if (player.getCash() >= COST)
                {
                    player.removeCash(COST);
                    player.addAttack(ATK_MOD);
                    sword.addDamage(ATK_MOD);
                    player.SetText();
                    SoundManager.instance.playSingle(SoundManager.instance.powerup);
                    Destroy(this.gameObject);
                    Debug.Log(sword.getDamage());
                    Debug.Log(player.getAtkModifier());
                }
            }
            else
                Debug.Log("Hm..");
            
        }
    }
}
