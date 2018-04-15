using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A destructible small box.
/// </summary>
public class SmallBoxCtrl : Destructible
{
    const int OBJ_LIFE = 5;

    // Use this for initialization
    void Start()
    {
        base.setPosition(GetComponent<Transform>());
        base.setObjectLife(OBJ_LIFE);
    }
}
