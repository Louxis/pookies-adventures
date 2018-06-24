using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the coin value and the coin pickup.
/// </summary>
public class CoinController : MonoBehaviour
{
    private int coinValue;
    private PookieController player;

    void Start()
    {
        coinValue = 50;
    }

    /// <summary>
    /// Sets the coin value with a specific one.
    /// </summary>
    /// <param name="newValue"></param>
    public void setValue(int newValue)
    {
        coinValue = newValue;
    }
    /// <summary>
    /// Detects coin pickup from the player.
    /// </summary>
    /// <param name="collider"></param>
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            player = (PookieController)collider.gameObject.GetComponent(typeof(PookieController));
            player.addCash(coinValue);
            player.SetText();
            SoundManager.instance.playSingle(SoundManager.instance.pickupCoin);
            Destroy(gameObject);
        }
    }

}
