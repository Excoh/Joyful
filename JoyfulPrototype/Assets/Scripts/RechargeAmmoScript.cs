using UnityEngine;
using System.Collections;

//Erick Ramirez Cordero
//Script for Recharge Station
//When Player collides with station, ammo recharges over time

public class RechargeAmmoScript : MonoBehaviour
{

    //Variables
    public float recharge;
    public float rechargeDelay;
    public int ammoCounter;
    private int ammoMax;


    //Daniel Bueno created this variables =D
    //Some variables are private, for the sake of encapsulation --- 
    //Though there is methods to change the variables, if there is a need to change then using other classes/objects
    private int ammoBefore;    //this is used to check if the recharge station "used less resources" to recharge... Example: Current Ammo = 98 and it was suppose to recover 10, but only recovers 2... it won't need to use 8 point of resource
    private int maxCounter;     //how much it recharged until now
    public int maxRecharge;    //how much it will recharge
    public int projectileRecharge; //used to change the quantity of projectile to recharge

    private static float depletion; //used as a constant for the depletionDelay
    public float depletionDelay; //how much time until the station is loaded up

    private bool wasUsed; //check if the recharg station was depleted

    public GameObject indicator;

    void Start()
    {
        ammoMax = PlayerPrefs.GetInt("ProjectileCount");
        depletion = depletionDelay;
        indicator.SetActive(false);
    }

    void Update()
    {

        ammoCounter = ProjectileChargeCounter.currentProjectile;

        //this part check if the station was depleted...
        if (isDepleted())
        {
            //...and when the delay time reaches 0 (or less, just to make sure) toggle the station on and reset maxCounter/depletionDelay
            if ((depletionDelay -= Time.deltaTime) <= 0)
            {
                toggleStation();
                maxCounter = 0;
                depletionDelay = depletion;
                indicator.SetActive(false);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isDepleted())
        {

            if (other.name == "Player") //If Object colliding is Player
            {
                if (ammoCounter < ammoMax) //If current Ammo is less than Max allowed
                {
                    if (recharge < rechargeDelay) //Delay for recharge
                    { recharge += Time.deltaTime; }

                    else //Add to ammo count and restart delay
                    {
                        ammoBefore = ProjectileChargeCounter.currentProjectile;
                        ProjectileChargeCounter.increaseProjectile(projectileRecharge);
                        //ammoCounter += projectileRecharge; //For gradual recharge DANIEL NOTES: I believe this line is not really necessary
                        //since we update it always and it's not checked immediatly=D

                        recharge = 0;
                        Debug.Log("Ammo: " + ammoCounter); //For testing purposes, replace with GUI Display (?)

                        //increases maxCounter variable then checks if the station was depleted
                        maxCounter += (ProjectileChargeCounter.currentProjectile - ammoBefore);
                        Debug.Log("Resource Used    : " + maxCounter);
                        if (maxCounter >= maxRecharge)
                        {
                            toggleStation();
                            indicator.SetActive(true);

                        }
                    }
                }
            }
        }
    }

    //Three methods by Daniel Bueno =D
    //check if the resources withing the station was completly used
    public bool isDepleted()
    {
        return wasUsed;
    }

    //if wasUsed is true, then it becomes false... and the way around.
    public void toggleStation()
    {
        wasUsed = !wasUsed;
    }

    //if you ever want to change the max amount of "ammo" to recharge during game play, use this.
    public static void updateDepletionDelay(float newValue)
    {
        depletion = newValue;
    }
}


