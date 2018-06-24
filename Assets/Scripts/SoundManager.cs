using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource effectSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;
    public AudioClip pickupCoin;
    public AudioClip powerup;
    public AudioClip hit;
    public AudioClip pookieDeath;
    public AudioClip laser;
    // Use this for initialization
    void Awake () {
		if (instance == null)
        {
            instance = this;
        }else if(instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    public void playSingle(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.Play();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
