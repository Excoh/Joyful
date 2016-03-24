using UnityEngine;
using System.Collections;

public class SpeedPowerUp : MonoBehaviour {

    public PlayerController playerController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            playerController = other.GetComponent<PlayerController>();
            playerController.PowerUp(2);
            Destroy(gameObject);
        }
    }
}
