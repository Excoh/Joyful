using UnityEngine;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour
{

    public int enemyHealth;
    public GameObject deathEffect;
    public int pointsOnDeath;

    public AudioSource soundEffectsSource;
    public AudioClip[] deathClip;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void giveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
        if (enemyHealth <= 0)
        {
            Debug.Log("Doing Something");
            Instantiate(deathEffect, transform.position, transform.rotation);
            ScoreManager.AddPoints(pointsOnDeath);
            Destroy(gameObject);
        }
    }
}