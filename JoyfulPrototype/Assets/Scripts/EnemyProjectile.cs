using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	//vars for projectiles

	public int damageToGive;
    public GameObject impactEffect;

	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		//fires the projectile in a verticle manner
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        Debug.Log(other.tag);
		//damages the player it comes into contact
		if (other.tag == "Player")
		{
			HealthManager.HurtPlayer (damageToGive);
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
		
	}

}

