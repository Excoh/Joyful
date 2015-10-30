using UnityEngine;
using System.Collections;

public class AngerEnemyMovement : MonoBehaviour
{
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

	private bool notAtEdge;
	public Transform edgeCheck;

	//Variables for Anger's Path
	public Transform pathCheck;
	public float pathRadius;
	public LayerMask whatIsPath;
	private bool pathEnd;

	public float playerCheckRadius;
	public LayerMask whatIsPlayer;
	public Transform playerTransform;
	private bool playerFound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Checking for Player
		playerFound = Physics2D.OverlapCircle (transform.position, playerCheckRadius, whatIsPlayer);

		if (playerFound)
		{ChasePlayer();}

		else
		{FollowPath();}
	}

	void FixedUpdate()
	{this.grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);}

	void FollowPath () //When Player is not in range, Anger follows Path
	{
		//Checking for end of path
		pathEnd = Physics2D.OverlapCircle (pathCheck.position, pathRadius, whatIsPath);

		if (pathEnd)
		{moveRight = !moveRight;}
		
		if (moveRight)
		{print("right");
			transform.localScale = new Vector3(-1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		
		else 
		{print ("Left");
			transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	void ChasePlayer () //When Player is in range, Anger will chase Player
	{
		this.hittingWall = Physics2D.OverlapArea (wallCheck.position, groundCheck.position, whatIsWall);
		
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, whatIsWall);
		
		if (!notAtEdge && this.grounded)
		{Jump ();}
		
		if (hittingWall && this.grounded) 
		{Jump ();}

		float relativePoint = playerTransform.position.x - transform.position.x;
		
		if (relativePoint > 1.0)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			
			print ("Object is to the left");
		}
		
		else if (relativePoint < 1.0)
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