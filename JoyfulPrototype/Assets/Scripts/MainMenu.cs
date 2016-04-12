using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {


	public int playerLives;

	public int playerHealth;

	public void NewGame()
	{
		Application.LoadLevel (1);

		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);

        PlayerPrefs.SetInt("ProjectileCount", 100);
	}

	public void LevelSelect()
	{
		//PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		//PlayerPrefs.SetInt ("CurrentScore", 0);

		//PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		//PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);
  //      PlayerPrefs.SetInt ("ProjectileCount", 100);

  //      Application.LoadLevel (levelSelect);
	}

    public void ReturnToTitle()
    {
        Application.LoadLevel(0);
    }

    public void QuitGame()
	{
		Application.Quit ();
	}
}
