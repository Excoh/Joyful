using UnityEngine;
using System.Collections;
public class GuiltEnemy : MonoBehaviour {

	//speed and direction guilt moves
    public float moveSpeed;
    public bool moveUp;

	//wall checks
    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool hittingWall;

	//Only kept the edge check as it seemed to throw a lot off otherwise
    private bool notAtEdge;
    public Transform edgeCheck;

	//Timers and stunned bool
    private float timeLimit = 5.0f;
	private float waitTimer = 0f;
    private float stunTime = 2.0f;
    public bool stunned;

	//increases the left and right area for checking for player
	private double leftCheck;
	private double rightCheck;

	//vars for player, projectile, and the firing point from guilt
	public static Transform target;
	public GameObject enemyProjectile;
	public Transform firingPoint;

    // Use this for initialization
    void Start () {
		//start with guilt not being stunned, sets target to player, and increases the detection range
        stunned = false;
		target = GameObject.FindWithTag ("Player").transform;
		leftCheck = transform.position.x - .5;
		rightCheck = transform.position.x + .5;
	}
	
	// Update is called once per frame
	void Update () {
		//just decrements the timer if stunned and stops motion
        if (stunned)
        {
            stunTime -= Time.deltaTime;
            GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, 0);

            if (stunTime <= 0)
            {
                stunned = false;
            }
        }
        else
        {
			//timer between direction change
            if (timeLimit > 0)
            {
                timeLimit -= Time.deltaTime;
            }

            this.hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);

            notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);
			//checks for if the timer is less than 0 and if it is already attached to a wall 
            if (hittingWall && timeLimit <= 0)
            {
				//resets timer and changes vertical direction
                moveUp = !moveUp;
                timeLimit = 5.0f;
            }

			//changes direction of travel and direction guilt is facing
            if (moveUp)
            {
                transform.localScale = new Vector3(1f, -1f, 1f);
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, moveSpeed);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, -moveSpeed);
            }

			//timer between shots
			if (waitTimer > 0)
			{
				waitTimer -= Time.deltaTime;
			}
			//if the player is between the checks
			else if (target.position.x >= leftCheck && target.position.x <= rightCheck) 
			{
				//and if guilt is turned directionaly towards the player
				if (transform.localScale.y == 1 && target.position.y > transform.position.y) 
				{
					//fire a projectile and reset the timer
					Instantiate (enemyProjectile, firingPoint.position, firingPoint.rotation);
					waitTimer = .5f;
				}
				else if (transform.localScale.y == -1 && target.position.y < transform.position.y) {
					Instantiate (enemyProjectile, firingPoint.position, firingPoint.rotation);
					waitTimer = .5f;
				}
			}

        }
    }   

	//if guilt is stunned, set stunned to true and set the timer to 2 seconds
    public void isStunned()
    {
        stunned = true;
        stunTime = 2f;
    }
}
