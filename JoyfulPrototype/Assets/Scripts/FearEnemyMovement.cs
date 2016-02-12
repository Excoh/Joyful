using UnityEngine;
using System.Collections;

public class FearEnemyMovement : MonoBehaviour {

    //Psuedo-Constants
    Vector3 LEFT_SPRITE = new Vector3(1f, 1f, 1f);
    Vector3 RIGHT_SPRITE = new Vector3(-1f, 1f, 1f);

    //Public Members
    public float moveSpeed;
	public float jumpHeight;
    public float jumpDistance;
	public float wallCheckRadius;
    public float groundCheckRadius;
    public float playerCheckRadius;
    public float jumpCooldownTime;
    public LayerMask whatIsWall; //Lefted as public because Wall may have its own Layer Mask.
    public float timeAllowedToBeAwake;

    //Private Members
    private Vector2 _startPosition;
	private SpriteRenderer _spriteOn;
    private Rigidbody2D _rigidbody;
    private Transform _wallCheck;
    private Transform _groundCheck;
    private Transform _edgeCheck;
    private Transform _playerTransform;
    private bool _atEdge;
    private bool _grounded;
    private bool _hittingWall;
    private bool _playerFound;
    private LayerMask _groundLayerMask;
    private bool _movingLeft;
    private bool _movingRight;
    private bool _jumping;
    private bool _returning;
    private bool _hiding;
    private float _timeOfJump;
    private float _timeWokenUp;
    private float _distanceFromPlayer;


    void Start () 
	{
        _Init_Fear();
        _spriteOn.enabled = false; //Enemy Hidden and Idle
    }
    
    private void _Init_Fear()
    {
        try {
            _startPosition = this.gameObject.transform.position;
            _spriteOn = this.gameObject.GetComponent<SpriteRenderer>();
            _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
            _wallCheck = this.gameObject.transform.FindChild("WallCheck");
            _groundCheck = this.gameObject.transform.FindChild("GroundCheck");
            _edgeCheck = this.gameObject.transform.FindChild("EdgeCheck");
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _groundLayerMask = LayerMask.GetMask("Ground");
            _movingLeft = true;
            _movingRight = false;
            _hiding = true;
            _returning = false;
        }
        catch{
            Debug.Log("Something is wrong with _Init_Fear()");
        }
    }

	void Update () 
	{
        _Sense();
        _Think();
        _Act();
	}

    private void _Sense()
    {
        this._grounded = Physics2D.OverlapCircle(_groundCheck.position, groundCheckRadius, _groundLayerMask);
        this._distanceFromPlayer = Vector2.Distance(_playerTransform.position, transform.position);

        //If enemy returned to original location
        if (_returning && Vector2.Distance(transform.position,_startPosition) < 0.8f)
        {
            _hiding = true;
            _returning = false;
            _spriteOn.enabled = false; //Return to being idle and invisible.
        }
    }

    private void _Think()
    {
        //If awake for set time, go back to original position.
        if (!_hiding && _timeWokenUp + timeAllowedToBeAwake < Time.time)
        {
            _returning = true;
        }
        else
        {
            if (_distanceFromPlayer < playerCheckRadius) //If player is within range.
            {
                if (_hiding)
                {
                    _hiding = false;
                    _timeWokenUp = Time.time;
                    _spriteOn.enabled = true;
                }
            }

            //Checks if the enemy can jump again.
            if (_timeOfJump + jumpCooldownTime < Time.time)
            {
                _jumping = false;
            }
        }
    }

    private void _Act()
    {
        //Time limit has been reached. Enemy must go back to sleep.
        if (_returning)
        {
            _ReturnToStartPosition();
        }
        else
        {
            if (_distanceFromPlayer < playerCheckRadius && _rigidbody.velocity.y == 0 && _distanceFromPlayer > 1f) //If player is within range.
            {
                _Move();
                AttackPlayer();       
            }
        }
    }

    private void _Move()
    {
        float relativePoint = _playerTransform.position.x - transform.position.x;

        //Moving Left
        if (relativePoint < 0.0 )
        {
            transform.localScale = LEFT_SPRITE;
            _rigidbody.velocity = new Vector2(-moveSpeed, _rigidbody.velocity.y);
            _movingLeft = true;
            _movingRight = false;
        }
        //Moving Right
        else if (relativePoint >= 0.0 )
        {
            transform.localScale = RIGHT_SPRITE;
            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y); ;
            _movingRight = true;
            _movingLeft = false;
        }     
    }

    private void _ReturnToStartPosition()
    {
        float relativePoint = _startPosition.x - transform.position.x;

        //Moving Left
        if (relativePoint < 0.0)
        {
            transform.localScale = LEFT_SPRITE;
            _rigidbody.velocity = new Vector2(-moveSpeed, _rigidbody.velocity.y);
            _movingLeft = true;
            _movingRight = false;
        }
        //Moving Right
        else if (relativePoint >= 0.0)
        {
            transform.localScale = RIGHT_SPRITE;
            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y); ;
            _movingRight = true;
            _movingLeft = false;
        }
    }

    void AttackPlayer () //When Player is in range, Fear will chase Player
	{
        if (!_jumping)
        {
            this._hittingWall = Physics2D.OverlapArea(_wallCheck.position, _groundCheck.position, whatIsWall);

            this._atEdge = Physics2D.OverlapCircle(_edgeCheck.position, wallCheckRadius, whatIsWall);

            if (this._atEdge && this._grounded || this._hittingWall && this._grounded)
            {
                _timeOfJump = Time.time;
                Jump();
                print(_rigidbody.velocity);
                _jumping = true;
            }
        }
	}

	void Jump()
	{
        if (_movingLeft)
        {
            _rigidbody.AddForce(new Vector2(-jumpDistance, jumpHeight), ForceMode2D.Force);
        }
        else if (_movingRight)
        {
            _rigidbody.AddForce(new Vector2(jumpDistance, jumpHeight), ForceMode2D.Force);
        }
    }
}
