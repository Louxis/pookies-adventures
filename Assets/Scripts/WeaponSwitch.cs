using UnityEngine;

/// <summary>
/// This class will handle the weapon switching, by default will be used the Fire2 button.
/// </summary>
public class WeaponSwitch : MonoBehaviour
{
    private int currentWeapon;
    private GameObject[] weapons;
    private PookieController pookie;
    private GameObject[] goldenEffects;
    private GameObject[] rainbowEffects;

    // Use this for initialization
    void Start()
    {
        this.currentWeapon = 0;
    }

    private bool axisFireInUse = false;
    // Update is called once per frame
    void Update()
    {
        if (pookie == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                pookie = GameObject.FindGameObjectWithTag("Player").GetComponent<PookieController>();
                weapons = pookie.getWeapons();
                goldenEffects = GameObject.FindGameObjectsWithTag("GoldenParticle");
                rainbowEffects = GameObject.FindGameObjectsWithTag("RainbowParticle");
                changeWeapon(1);
            }
        }
        if (Input.GetAxisRaw("Fire2") != 0)
        {
            if (!axisFireInUse)
            {
                int number = currentWeapon + 1;
                changeWeapon(number);
                axisFireInUse = true;
            }

        }
        if (Input.GetAxisRaw("Fire2") == 0)
        {
            axisFireInUse = false;
        }
    }

    //changes to another weapon, hidding all the other ones.
    private void changeWeapon(int num)
    {
        if (pookie == null)
        {
            pookie = GameObject.FindGameObjectWithTag("Player").GetComponent<PookieController>();
            weapons = pookie.getWeapons();
            goldenEffects = GameObject.FindGameObjectsWithTag("GoldenParticle");
            rainbowEffects = GameObject.FindGameObjectsWithTag("RainbowParticle");
        }
        if (num == weapons.Length)
            num = 0;
        currentWeapon = num;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == num)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }
        //check for golden effects
        for (int i = 0; i < goldenEffects.Length; i++)
        {
            if (num != 0)
                goldenEffects[i].SetActive(true);
            else
                goldenEffects[i].SetActive(false);
        }
        for (int i = 0; i < rainbowEffects.Length; i++)
        {
            if (num != 0)
                rainbowEffects[i].SetActive(false);
            else
                rainbowEffects[i].SetActive(true);
        }
    }
}
