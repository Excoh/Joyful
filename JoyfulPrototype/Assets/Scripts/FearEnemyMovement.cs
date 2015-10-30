using UnityEngine;
using System.Collections;

public class FearEnemyMovement : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;
	public float jumpHeight;
	
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask whatIsWall;
	private bool hittingWall;
	
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;
	
	public float playerCheckRadius;
	public LayerMask whatIsPlayer;
	public bool playerFound;
	
	private bool notAtEdge;
	public Transform edgeCheck;

	private SpriteRenderer spriteOn;

	public Transform playerTransform;

	void Start () 
	{
		spriteOn = GetComponent<SpriteRenderer> ();
		spriteOn.enabled = false; //Enemy Hidden and Idle
	}

	void FixedUpdate()
	{this.grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);}

	void Update () 
	{
		playerFound = Physics2D.OverlapCircle (transform.position, playerCheckRadius, whatIsPlayer);

		if (playerFound) //If player is within range
		{
			spriteOn.enabled = true;
			AttackPlayer();
		}

		else //Return to being idle
		{spriteOn.enabled = false;}
	}

	void AttackPlayer () //When Player is in range, Fear will chase Player
	{
		this.hittingWall = Physics2D.OverlapArea (wallCheck.position, groundCheck.position, whatIsWall);
		
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, whatIsWall);

		if (!notAtEdge && this.grounded)
		{Jump ();}
		
		if (hittingWall && this.grounded) 
		{Jump ();}

		//Vector2 relativePoint = transform.InverseTransformPoint(playerTransform.position);

		float relativePoint = playerTransform.position.x - transform.position.x;

		if (relativePoint > 0.0)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);

			print ("Object is to the left");
		}

		else if (relativePoint < 0.0)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);

			print ("Object is to the right");
		}

		else 
		{print ("conflict");}
	}

	void Jump()
	{GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);}
}
