using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows generic gamepad controls, like start button to start a game.
/// </summary>
public class GamePadHandler : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        GameObject buttonObject = GameObject.FindGameObjectWithTag("Button");
        ButtonScript button = buttonObject.GetComponent<ButtonScript>();
        if (Input.GetAxisRaw("Submit") >= 0.5f)
            button.TaskOnClick();
    }
}
