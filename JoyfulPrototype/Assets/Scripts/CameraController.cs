using UnityEngine;
using UnityEditor;
using System.Collections;

public class CameraController : MonoBehaviour {

	private PlayerController player;
    private Rigidbody2D rbody2d;

    //public enum FollowModes
    //{
    //    SimpleFollow, SimpleLerp, AdvancedLerp
    //}

    //public FollowModes selectedMode = FollowModes.SimpleLerp;

    public bool isFollowing;

    public float xOffset;
    public float yOffset;


    public float advancedLerpSpeed = 3f;
    public float playerVelocityModifier = .2f;
    public float negativeVelocityModifier = .2f;


    private float xVel;
    private float yVel;

    public Transform[] backgrounds; //Array of all the back and foregrounds to be parallaxed
    public float smoothing = 1f; //how smooth the parallax is going to be.


    private float[] parallaxScales; //the proportion of the camera's movement to move the background
    private Transform cam;
    private Vector3 previousCamPos;
    private bool _allowParallax;

    void Awake()
    {
        cam = this.gameObject.transform;
    }
    // Use this for initialization
    void Start () {
		player = FindObjectOfType<PlayerController> ();
        rbody2d = player.GetComponent<Rigidbody2D>();
        isFollowing = true;
        advancedLerpSpeed = Mathf.Clamp(advancedLerpSpeed, float.Epsilon, float.MaxValue);
        _allowParallax = false;


        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        Vector3 finalSpot = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);

        if (isFollowing)
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

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        previousCamPos = cam.position;
    }
}
