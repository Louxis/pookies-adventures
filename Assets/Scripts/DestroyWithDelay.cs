using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible to destroy a gameobject after a delay.
/// </summary>
public class DestroyWithDelay : MonoBehaviour
{
    /// <summary>
    /// Delay to destroy an object.
    /// </summary>
    public float delay;

    void Start()
    {
        Destroy(transform.parent.gameObject, delay);
    }
}
