using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A destructible big box.
/// </summary>
public class BiggerBoxCtrl : Destructible
{
    const int OBJ_LIFE = 9;

    // Use this for initialization
    void Start()
    {
        base.setPosition(GetComponent<Transform>());
        base.setObjectLife(OBJ_LIFE);
    }
}

