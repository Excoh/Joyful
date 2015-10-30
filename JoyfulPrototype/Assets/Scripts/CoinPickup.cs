using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {

	public int pointsToAdd;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.GetComponent<PlayerController>() == null)
			return;

		ScoreManager.AddPoints (pointsToAdd);

        //Daniel Bueno done this part =D
        ProjectileChargeCounter.increaseProjectile(PlayerPrefs.GetInt ("ProjectileCount")/10); //recovers a tenth of the projectile count
        HealthManager.healthRecovery(PlayerPrefs.GetInt("PlayerMaxHealth")/10); //recovers a tenth of the player health

		Destroy (gameObject);
	}
}
