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

	public int Arc_Counter
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().gravityScale  = 0;
		player = FindObjectOfType<PlayerController> ();
         GetComponent<Rigidbody2D>().velocity = speed * GetComponent<Rigidbody2D>().velocity.normalized;
	}
	Arc_Counter = 0;
	
	// Update is called once per frame
	void Update () {
		Arc_Counter +=1;
		if (Arc_Counter == 30){
			GetComponent<Rigidbody2D>().gravityScale  = 1;
		}
		
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
        if (other.tag != "Player")
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}

}
