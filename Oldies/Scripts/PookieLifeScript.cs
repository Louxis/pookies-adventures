using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PookieLifeScript : MonoBehaviour {

    private GameObject[] hearts = new GameObject[3];
    private int heartsLeft;
	// Use this for initialization
	void Start () {
        heartsLeft = 0;
		foreach(Transform child in transform)
        {
            if (!child.Equals(this.transform))
            {
                hearts[heartsLeft] = child.gameObject;
                heartsLeft++;
            }
            
        }
        foreach(GameObject h in hearts)
        {
            h.GetComponent<Animator>().SetBool("isAlive",true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage(int damage)
    {        
        for(int i = 0; i < damage; i++)
        {
            if (heartsLeft > 0)
            {
                hearts[heartsLeft - 1].GetComponent<Animator>().SetBool("isAlive", false);
                heartsLeft--;
            }                
        }               
    }

    public int getLifeLeft()
    {
        return heartsLeft;
    }


}
