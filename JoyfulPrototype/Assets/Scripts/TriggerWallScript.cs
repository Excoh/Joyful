using UnityEngine;
using System.Collections;

public class TriggerWallScript : MonoBehaviour {
    
    private bool turnedOn;
    public bool playerInZone;
	// Use this for initialization
	void Start () {
        turnedOn = false;
        playerInZone = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Vertical") > 0 && playerInZone)
        {
            turnedOn = true;
        }
	}
    public bool getTurnedOn() 
    {
        return turnedOn;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            playerInZone = true;
        }
    }
}
