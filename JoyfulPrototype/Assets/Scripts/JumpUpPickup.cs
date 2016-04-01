using UnityEngine;
using System.Collections;


public class JumpUpPickup : MonoBehaviour {

    public PlayerController JumpPowerUp;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            JumpPowerUp = other.GetComponent<PlayerController>();
            JumpPowerUp.JumpPowerUp = true;
            Destroy(gameObject);
        }
    }
}
