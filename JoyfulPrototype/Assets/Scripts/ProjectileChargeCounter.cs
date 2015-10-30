using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProjectileChargeCounter : MonoBehaviour {
    //variables for current and max projectiles
    public static int currentProjectile;
    public static int maxProjectile;

    public static bool hasAmmo; //checks if you have ammo

    Text text;
	// Use this for initialization
	void Start () {
        maxProjectile = PlayerPrefs.GetInt("ProjectileCount"); //sets max to the projectile count declared on main menu
        currentProjectile = maxProjectile; //sets current equal to maximum
        hasAmmo = true; 
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(currentProjectile < 0)
        {
            currentProjectile = 0;
        }
        text.text = "" + currentProjectile; //text for current projectiles left
	}

    //function for increasing projectiles
    public static void increaseProjectile(int moreProjectiles)
    {
        //checks if your current projectiles plus the added amount is higher than the max, if so sets it to max
        if(currentProjectile + moreProjectiles < maxProjectile)
        {
            currentProjectile += moreProjectiles;
        }
        else
        {
            currentProjectile = maxProjectile;
        }
        hasAmmo = true; //lets you know you have ammo
    }

    public static void decreaseProjectile()
    {
        //checks if current projectiles are higher than 0
        if(currentProjectile > 0)
        {
            currentProjectile = currentProjectile - 10;
        }

        //states that you don't have ammo if your counter is set to 0
        if(currentProjectile == 0)
        {
            hasAmmo = false;
        }
    }

    //sets current projectile to max
    public static void setToMax()
    {
        currentProjectile = maxProjectile;
        hasAmmo = true;
    }

    //returns true or false if you have ammo
    public static bool checkAmmo()
    {
        return hasAmmo;
    }

    //gives the functionality to increase the maximum amount of ammo
    public static void increaseMax(int increaseAmount)
    {
        maxProjectile += increaseAmount;
        PlayerPrefs.SetInt("ProjectileCount", maxProjectile);
    }
}
