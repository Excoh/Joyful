using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public string titleScreen;

	public static bool isPaused;

	public GameObject pauseMenuCanvas;
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused) {
			pauseMenuCanvas.SetActive (true);
			Time.timeScale = 0f;
		} else {
			pauseMenuCanvas.SetActive(false);
			Time.timeScale = 1f;
		}

	}

	public void Resume()
	{
		isPaused = false;
	}

	public void TitleScreen()
	{
		Application.LoadLevel (titleScreen);
	}

	public void Quit()
	{
        Application.Quit();
	}
}
