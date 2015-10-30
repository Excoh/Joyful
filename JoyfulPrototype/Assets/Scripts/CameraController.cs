using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerController player;
    private Rigidbody2D rbody2d;

    public enum FollowModes
    {
        SimpleFollow, SimpleLerp, AdvancedLerp
    }

    public FollowModes selectedMode = FollowModes.SimpleFollow;

    public bool isFollowing;

    public float xOffset;
    public float yOffset;

    public float simpleLerpSpeed = .1f;
    public float advancedLerpSpeed = 3f;
    public float playerVelocityModifier = .2f;
    public float negativeVelocityModifier = .2f;

    private float xVel;
    private float yVel;

    // Use this for initialization
    void Start () {
		player = FindObjectOfType<PlayerController> ();
        rbody2d = player.GetComponent<Rigidbody2D>();
        isFollowing = true;
        simpleLerpSpeed = Mathf.Clamp(simpleLerpSpeed, float.Epsilon, 1f);
        advancedLerpSpeed = Mathf.Clamp(advancedLerpSpeed, float.Epsilon, float.MaxValue);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 finalSpot = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);

        if (isFollowing)
        {
            if (selectedMode.Equals(FollowModes.SimpleFollow))
            {
                transform.position = finalSpot;
            }
            else if (selectedMode.Equals(FollowModes.SimpleLerp))
            {
                Debug.DrawLine(player.transform.position, finalSpot, Color.red);
                transform.position = Vector3.Lerp(transform.position, finalSpot, simpleLerpSpeed);
            }
            else
            {
                Vector3 advancedFollowSpot = finalSpot + (Vector3)((rbody2d.velocity + new Vector2(0f, Mathf.Min(0f, rbody2d.velocity.y * negativeVelocityModifier))) * playerVelocityModifier);

                Debug.DrawLine(player.transform.position, advancedFollowSpot, Color.red);

                float towards = Vector3.Dot((transform.position - advancedFollowSpot).normalized, rbody2d.velocity.normalized);
                float lerpTime = 1f / advancedLerpSpeed;

                float newX = Mathf.SmoothDamp(transform.position.x, advancedFollowSpot.x, ref xVel, lerpTime);
                float newY = Mathf.SmoothDamp(transform.position.y, advancedFollowSpot.y, ref yVel, lerpTime);
                transform.position = new Vector3(newX, newY, finalSpot.z);
            }
        }
	}
}
