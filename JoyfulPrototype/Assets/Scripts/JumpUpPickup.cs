using UnityEngine;
using System.Collections;


public class JumpUpPickup : MonoBehaviour {

    public PlayerController JumpPowerUp;
    public GameObject AfterEffect;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            JumpPowerUp = other.GetComponent<PlayerController>();
            JumpPowerUp.JumpPowerUp = true;
            GameObject myEffect = Instantiate(AfterEffect,Vector3.zero, Quaternion.identity) as GameObject;
            myEffect.transform.parent = JumpPowerUp.transform;
            myEffect.transform.position = other.transform.localPosition;
            Destroy(gameObject);
        }
    }
}
