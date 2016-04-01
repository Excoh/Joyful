using UnityEngine;
using System.Collections;

public class SpeedPowerUp : MonoBehaviour {

    public PlayerController playerController;
    public GameObject AfterEffect;
    public GameObject Player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            playerController = other.GetComponent<PlayerController>();
            playerController.PowerUp(2);
            GameObject myEffect = Instantiate(AfterEffect, Vector3.zero, Quaternion.identity) as GameObject;
            myEffect.transform.parent = Player.transform;
            myEffect.transform.position = other.transform.localPosition;
            Destroy(gameObject);
        }
    }
}
