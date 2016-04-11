using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {

    public GameObject darkness1;
    public GameObject darkness2;
    public GameObject portal;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(darkness1 == null && darkness2 == null)
        {
            portal.SetActive(true);
        }
	}
}
