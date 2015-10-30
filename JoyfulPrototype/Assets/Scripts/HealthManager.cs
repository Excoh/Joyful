using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	public static int maxPlayerHealth;

	public static int playerHealth;
	
	Text text;

	private LevelManager levelManager;

	public bool isDead;

	private LifeManager lifeSystem;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();

        //playerHealth = maxPlayerHealth;

		playerHealth = PlayerPrefs.GetInt ("PlayerCurrentHealth");

		levelManager = FindObjectOfType<LevelManager> ();

		lifeSystem = FindObjectOfType<LifeManager> ();

		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0 && !isDead) 
		{
			playerHealth = 0;
			levelManager.RespawnPlayer();
			lifeSystem.TakeLife();
			isDead = true;
		}

		text.text = "" + playerHealth;
	}

	public static void HurtPlayer(int damageToGive)
	{
		playerHealth -= damageToGive;
		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
	}

	public void FullHealth()
	{
		playerHealth = PlayerPrefs.GetInt ("PlayerMaxHealth");
		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
	}
    
    //Daniel Bueno did this method =D
    //used to recover a short amount of health
    public static void healthRecovery(int healthToRecover)
    {
        /*if the player's current health plus the amount of health to recover is bigger than the max health possible
            then set health to max
        */
        if ((playerHealth + healthToRecover) > PlayerPrefs.GetInt("PlayerMaxHealth")) {
            playerHealth = PlayerPrefs.GetInt("PlayerMaxHealth");
            PlayerPrefs.SetInt("PlayerCurrentHealth", playerHealth);
        }
        //otherwise, just add more health
        else {
            playerHealth += healthToRecover;
            PlayerPrefs.SetInt("PlayerCurrentHealth", playerHealth);
        }
    }
}
