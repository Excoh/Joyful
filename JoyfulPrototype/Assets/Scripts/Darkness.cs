using UnityEngine;

// Programmer: Josh Gebbeken
// Darkness Object Script
/* The Darkness script work similar to the HurtEnemyOnContact.cs and HurtPlayerOnContact.cs combined.
   
*/

public class Darkness : MonoBehaviour
{
    // Give player this much damage.
    public int DarknessDamage = 5;

    public int DarknessHealth; 

    public GameObject deathEffect;
    public int pointsOnDeath; 

    
    // Update called once per frame and checks if darkness health is equal to 0 or below
    // which will destroys the object if it's true
    void Update()
    {
        if (DarknessHealth <= 0)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
            ScoreManager.AddPoints(pointsOnDeath);
            DarknessCounter.decreaseDarkness(1);
            Destroy(gameObject);
        }
    }

    // This determines if a player has touched the darkness wall.
    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();


        // If an object entered the trigger and it's not the player then it needs to ignore it.
        if (player == null)
            return;

        // If the player did in fact touched the wall then give player some damage and knockback.
        HealthManager.HurtPlayer(DarknessDamage);
        player.knockbackCount = player.knockbackLength;
        if (other.transform.position.x < transform.position.x)
        {
            player.knockFromRight = true;
        }
        else
        {
            player.knockFromRight = false;
        }


    }

    // When the darkness takes damage
    public void GiveDarknessDamage(int damageToGive)
    {
        DarknessHealth -= damageToGive;
    }

}

