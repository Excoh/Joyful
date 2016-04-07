using UnityEngine;
using System.Collections;

public class DestroyFinishedParticle : MonoBehaviour {

	private ParticleSystem thisParticleSystem;
    public AudioClip[] deathClip;
    // Use this for initialization
    void Start () {
		thisParticleSystem = GetComponent<ParticleSystem> ();
        if (GetComponent<AudioSource>() != null && deathClip != null)
        {
            GetComponent<AudioSource>().clip = deathClip[Random.Range(0, deathClip.Length)];
            GetComponent<AudioSource>().Play(); 
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (thisParticleSystem.isPlaying)
			return;

		Destroy (gameObject);
	}

	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}
}
