using UnityEngine;
using System.Collections;

public class NinjaStarController : MonoBehaviour {

	public float speed;

	public PlayerController player;

	public GameObject enemyDeathEffect;

	public GameObject impactEffect;

	public int pointsForKill;

	public float rotationSpeed;

	public int damageToGive;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
         GetComponent<Rigidbody2D>().velocity = speed * GetComponent<Rigidbody2D>().velocity.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        
        if (other.tag == "Enemy") 
		{
			//Instantiate(enemyDeathEffect, other.transform.position, other.transform.rotation);
			//Destroy (other.gameObject);
			//ScoreManager.AddPoints(pointsForKill);

			other.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
		}

        // Damage darkness with ninja stars update (9/23)
        else if (other.tag == "Darkness")
        {
            other.GetComponent<Darkness>().GiveDarknessDamage(damageToGive);
        }
        // End new code

		//Stunning update (10/9)
        else if (tag == "Weapon" && other.tag == "Stunnable")
        {
            other.GetComponent<GuiltEnemy>().isStunned();
        }
        if (other.tag != "Player"&& other.tag != "Ladder")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}

}
