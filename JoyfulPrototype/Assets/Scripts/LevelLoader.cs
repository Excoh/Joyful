﻿using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {


	private bool playerInZone;

	public string levelToLoad;

	// Use this for initialization
	void Start () {
		playerInZone = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") > 0 && playerInZone && DarknessCounter.getCurrentDarkness() == 0) 
		{
			Application.LoadLevel(levelToLoad);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player") 
		{
			playerInZone = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player") 
		{
			playerInZone = false;
		}
	}
}
