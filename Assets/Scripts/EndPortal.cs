using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the ending portal, will finish the level after contact with Pookie.
/// </summary>
public class EndPortal : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            ButtonScript button = (ButtonScript)GameObject.FindGameObjectWithTag("Button").GetComponent<ButtonScript>();
            button.showEndScript();
        }
    }

}
