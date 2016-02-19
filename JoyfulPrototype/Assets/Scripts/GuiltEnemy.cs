using UnityEngine;
using System.Collections;
public class GuiltEnemy : MonoBehaviour {

    //Psuedo-Constants
    Vector3 UP_SPRITE = new Vector3(1f, 1f, 1f);
    Vector3 DOWN_SPRITE = new Vector3(1f, -1f, 1f);

    //Public Members
    public float moveSpeed;
    public float wallCheckRadius;
    public float playerCheckRadius;
    public LayerMask whatIsWall;
    public float cooldownTimeUntilNextMovement;
    public float stunTime;
	public GameObject enemyProjectile;
    public float projectileSpeed;

    //Private Members
    private bool _movingDown;
    private bool _isStunned;
    private bool _hittingWall;
    private Transform _playerTransform;
    private Transform _wallCheck;
    private Transform _edgeCheck;
    private Rigidbody2D _rigidbody;
    private float waitTimer = 0f;
    private float _distanceFromPlayer;
    private float _movementTimer;

    // Use this for initialization
    void Start () {
        _Init_Guilt();
	}

    private void _Init_Guilt()
    {
        try
        {
            //start with guilt not being stunned, sets target to player, and increases the detection range
            _isStunned = false;
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
            _wallCheck = this.gameObject.transform.FindChild("WallCheck");
            _edgeCheck = this.gameObject.transform.FindChild("EdgeCheck");
            _movementTimer = cooldownTimeUntilNextMovement;
        }
        catch
        {
            Debug.Log("Something wrong with _Init_Guilt");
        }
    }
	
	// Update is called once per frame
	void Update () {
        _Sense();
        _Think();
        _Act();
		
    }   

    private void _Sense()
    {
        this._distanceFromPlayer = Vector2.Distance(_playerTransform.position, transform.position);
        this._hittingWall = Physics2D.OverlapCircle(_wallCheck.position, wallCheckRadius, whatIsWall);
    }

    private void _Think()
    {
        if (!_isStunned)
        {
            //timer between direction change
            if (_movementTimer > 0)
            {
                _movementTimer -= Time.deltaTime;
            }         
            //checks for if the timer is less than 0 and if it is already attached to a wall 
            if (_hittingWall && _movementTimer <= 0)
            {
                //resets timer and changes vertical direction
                _movingDown = !_movingDown;
                _movementTimer = cooldownTimeUntilNextMovement;
            }
        }
    }

    private void _Act()
    {
        //just decrements the timer if stunned and stops motion
        if (_isStunned)
        {
            _Stunned();
        }
        else
        {
            _Move();
            _Shooting();
        }
    }

    //is called from player projectile. Possible refactor?
    public void isStunned()
    {
        _isStunned = true;
        stunTime = 2f;
    }

    private void _Stunned()
    {
        stunTime -= Time.deltaTime;
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

        if (stunTime <= 0)
        {
            _isStunned = false;
        }
    }

    private void _Move()
    {
        //changes direction of travel and direction guilt is facing
        if (_movingDown)
        {
            transform.localScale = DOWN_SPRITE;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, moveSpeed);
        }
        else
        {
            transform.localScale = UP_SPRITE;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -moveSpeed);
        }
    }

    private void _Shooting()
    {
        //timer between shots
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
        }
        //if the player is between the checks
        else if (_distanceFromPlayer < playerCheckRadius && this._distanceFromPlayer > 0.5f)
        {
            GameObject projectile = (GameObject)Instantiate(enemyProjectile, this.transform.position, Quaternion.identity);
            Vector2 directionToShoot = _playerTransform.position - this.transform.position;
            projectile.GetComponent<Rigidbody2D>().velocity = directionToShoot.normalized * projectileSpeed;
            waitTimer = .5f;
        }
    }
}
