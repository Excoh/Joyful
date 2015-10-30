using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //basic physics fields
	public float moveSpeed;
	private float moveVelocity;
	public float jumpVel = 10f;
    private bool doubleJumped;

    //more in depth physics fields
    public float baseGravity = .75f;
    public AnimationCurve timedGravityScale;
    public float secondsToReachMaxGravity = 1.0f;
    public float jumpGravityReduction = .5f;
    public float highGravityThresholdVelocity = 3f;
    public float terminalVelocity;
    public float aerialDragModifier = .05f;
    public float aerialDriftModifier = .1f;
    public float maxAerialDrift = 5f;
    private float timeFalling;

    //fields for dealing with detecting if the player is grounded or not
    public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	private Animator anim;

    //fields dealing with player projectiles
	public Transform firePoint;
	public GameObject ninjaStar;
	public float shotDelay;
	private float shotDelayCounter;

    //fields dealing with knockback
	public float knockback;
	public float knockbackLength;
	public float knockbackCount;
	public bool knockFromRight;

    public bool onLadder;
    public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    // Use this for initialization
    private void Start () {
		anim = GetComponent<Animator> ();

        gravityStore = GetComponent<Rigidbody2D>().gravityScale;

	}

    //FixedUpdate is called once every physics tic (60 times a second?)
    //Determines if the player is touching the ground using the physics API
    private void FixedUpdate() {
		this.grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}

	//Update is called once per frame
    //Takes care of player input and physics
	private void Update () {
        //shows a vector between the player and where the mouse is
        Vector3 mouseV = Input.mousePosition;
        mouseV.z = 10f;
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(mouseV);
        Vector3 mouseTargeting = (mouseWP - transform.position);
        Debug.DrawRay(transform.position, mouseTargeting);

        //determine if the player is falling, and other cleanup
        bool falling = false;
        if (grounded)
        {
            doubleJumped = false;
        }
        else if (GetComponent<Rigidbody2D>().velocity.y <= highGravityThresholdVelocity)
        {
            falling = true; 
        }

        if (falling)
        {
            timeFalling += Time.deltaTime;
        }
        else
        {
            timeFalling = 0;
        }

        //getting input for jumping and double jumping, and if the player can jump, run the jump method
        bool jumping = Input.GetButton("Jump");

        if (jumping)
		{
            if (grounded)
                Jump();
            else if (!doubleJumped)
            {
                Jump();
                doubleJumped = true;
            }
		}

        //major physics evaluation: knockback, gravity, player input, velocity changes, etc.
		if (knockbackCount <= 0) {
            
            //evaluluate the gravity value at the current time of falling, using the base value and the timed scale
            float gravityValue = baseGravity*timedGravityScale.Evaluate(timeFalling/secondsToReachMaxGravity);

            //if holding the jump button while moving upwards in a jump, gravity is reduced by some factor
            if (jumping && !falling)
            {
                gravityValue *= jumpGravityReduction;
            }

            //calculate the new y velocity after gravity, capping negative y vels at the terminal velocity
            float yVelAfterGravity = GetComponent<Rigidbody2D>().velocity.y - gravityValue;
            if (yVelAfterGravity < terminalVelocity)
                yVelAfterGravity = terminalVelocity;

            //determine the new x velocity
            float xVel = GetComponent<Rigidbody2D>().velocity.x;
            float xVelAfterModifiers;

            if (!grounded)
            {
                if (Mathf.Abs(xVel) > Mathf.Abs(maxAerialDrift))
                {
                    //if in the air and going faster than the max aerial horizontal velocity, slow down per the drag modifier
                    xVelAfterModifiers = Mathf.Lerp(xVel, maxAerialDrift * (xVel > 0 ? 1 : -1), aerialDragModifier);
                }
                else
                {
                    //if in the air and going slower than the max aerial horizontal velocity, increase velocity in the direction the player is inputting per the drift modifier
                    xVelAfterModifiers = Mathf.Lerp(xVel, maxAerialDrift * Input.GetAxisRaw("Horizontal"), aerialDriftModifier);
                }
            }
            else
            {
                //if on the ground, move as fast as the ground speed in the direction the player is inputting
                xVelAfterModifiers = moveSpeed * Input.GetAxisRaw("Horizontal");
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2 (xVelAfterModifiers, yVelAfterGravity);
		} 
		else {
			if(knockFromRight)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-knockback, knockback);
			}
			if(!knockFromRight)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(knockback, knockback);
			}
			knockbackCount -= Time.deltaTime;
		}

        //setting properties for animation
		anim.SetBool("Grounded", grounded);
		anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x));

		if (GetComponent<Rigidbody2D> ().velocity.x > 0) {
			transform.localScale = new Vector3 (1f, 1f, 1f);
		} 
		else if (GetComponent<Rigidbody2D> ().velocity.x < 0) 
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}

        shotDelayCounter -= Time.deltaTime;
        //detect input for firing projectiles and using the sword
        if (Input.GetButton ("Fire1"))
		{
            if (ProjectileChargeCounter.checkAmmo())
            {
                if (shotDelayCounter <= 0)
                {
                    shotDelayCounter = shotDelay;
                    GameObject starInstance = (GameObject)Instantiate(ninjaStar, firePoint.position, firePoint.rotation);
                    starInstance.GetComponent<Rigidbody2D>().velocity = mouseTargeting;
                    ProjectileChargeCounter.decreaseProjectile();
                }
            }
		}
		if (anim.GetBool("Sword")) 
		{
			anim.SetBool("Sword", false);
		}
		if (Input.GetButtonDown ("Fire2")) 
		{
			anim.SetBool("Sword", true);
		}
        if (onLadder) 
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;

            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");

            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, climbVelocity);
        }

        if (!onLadder) 
        {
            GetComponent<Rigidbody2D>().gravityScale = gravityStore;
        }
	}

    //the player jumps, by setting a new velocity for the rigid body with the y value changed to the jumpVel field
	private void Jump() {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpVel);
	}
}
