using UnityEngine;
using System.Collections;

public class FloatingLife : MonoBehaviour {

    public GameObject lifeManager;
    private float prevY;

	// Use this for initialization
	void Start () {
        prevY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, prevY + .1f * Mathf.Sin(Time.time * 2 * Mathf.PI));
	}

    void OnTriggerEnter2D(Collider2D other)
    {
            lifeManager.GetComponent<LifeManager>().GiveLife();
            Destroy(this.gameObject);

    }
}
