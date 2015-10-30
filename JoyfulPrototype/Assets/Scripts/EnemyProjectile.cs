using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	//vars for projectiles
	public float speed;

	public GuiltEnemy guilt;

	public GameObject impactEffect;

	public float rotationSpeed;

	public int damageToGive;

	void Start () {
		//determine traveling up or down
		guilt = FindObjectOfType<GuiltEnemy> ();

		if (guilt.transform.localScale.y < 0) {
			speed = -speed;
			rotationSpeed = -rotationSpeed;
		}
	}
	// Update is called once per frame
	void Update () {
		//fires the projectile in a verticle manner
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, speed);

		GetComponent<Rigidbody2D> ().angularVelocity = rotationSpeed;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//damages the player it comes into contact
		if (other.tag == "Player")
		{
			HealthManager.HurtPlayer (damageToGive);

			var player = other.GetComponent<PlayerController> ();
			player.knockbackCount = player.knockbackLength;

			if (other.transform.position.x < transform.position.x) {
				player.knockFromRight = true;
			} else {
				player.knockFromRight = false;
			}
		}

		Instantiate (impactEffect, transform.position, transform.rotation);
		Destroy (gameObject);
	}

}

