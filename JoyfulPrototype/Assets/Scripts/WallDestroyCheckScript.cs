using UnityEngine;
using System.Collections;

public class WallDestroyCheckScript : MonoBehaviour {

    public TriggerWallScript check;
	// Use this for initialization
	void Start () {
        check = FindObjectOfType<TriggerWallScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (check.getTurnedOn()) 
        {
            Destroy(this.gameObject);
        }
	}
}
