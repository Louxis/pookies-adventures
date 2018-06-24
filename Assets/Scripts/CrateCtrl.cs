using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A destructible crate.
/// </summary>
public class CrateCtrl : Destructible
{
    const int OBJ_LIFE = 6;
    // Use this for initialization
    void Start()
    {
        base.setPosition(GetComponent<Transform>());
        base.setObjectLife(OBJ_LIFE);
    }
}
