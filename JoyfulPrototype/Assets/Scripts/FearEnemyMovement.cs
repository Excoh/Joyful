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
    private bool _hasRoomToMove;
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
    private Animator _anim;


    void Start () 
	{
        _Init_Fear();
        _spriteOn.enabled = false; //Enemy Hidden and Idle
    }
    
    private void _Init_Fear()
    {
        try {
            _anim = this.gameObject.GetComponent<Animator>();
            _startPosition = this.gameObject.transform.position;
            _spriteOn = this.gameObject.GetComponent<SpriteRenderer>();
            _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();

            _wallCheck = new GameObject("WallCheck").transform;
            _wallCheck.position = new Vector2(_startPosition.x - 0.75f, _startPosition.y);
            _wallCheck.SetParent(this.gameObject.transform);

            _groundCheck = new GameObject("GroundCheck").transform;
            _groundCheck.position = new Vector2(_startPosition.x, _startPosition.y - 0.75f);
            _groundCheck.SetParent(this.gameObject.transform);

            _edgeCheck = new GameObject("EdgeCheck").transform;
            _edgeCheck.position = new Vector2(_startPosition.x - 0.75f, _startPosition.y - 0.75f);
            _edgeCheck.SetParent(this.gameObject.transform);

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

    void Update()
    {
        _Animation();
        _Behavior();
    }

    void _Animation()
    {
        _anim.SetFloat("VelocityY", Mathf.Abs(_rigidbody.velocity.y));
        _anim.SetFloat("VelocityX", Mathf.Abs(_rigidbody.velocity.x));
        _anim.SetBool("isHiding", _hiding);
    }

    void _Behavior()
    {
        _Sense();
        _Think();
        _Act();
    }

    private void _Sense()
    {
        this._grounded = Physics2D.OverlapCircle(_groundCheck.position, groundCheckRadius, _groundLayerMask);
        this._hittingWall = Physics2D.OverlapCircle(_wallCheck.position, wallCheckRadius, whatIsWall);
        this._hasRoomToMove = Physics2D.OverlapCircle(_edgeCheck.position, wallCheckRadius, whatIsWall);
        this._distanceFromPlayer = Vector2.Distance(_playerTransform.position, transform.position);
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
                Lunging();       
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
        this.transform.position = _startPosition;
        _hiding = true;
        _returning = false;
        _spriteOn.enabled = false; //Return to being idle and invisible.
    }

    void Lunging () //When Player is in range, Fear will chase Player
	{
        if (!_jumping)
        {
            if (this._hasRoomToMove && this._grounded || this._hittingWall && this._grounded)
            {
                _timeOfJump = Time.time;
                if (_movingLeft)
                {
                    _rigidbody.AddForce(new Vector2(-jumpDistance, jumpHeight), ForceMode2D.Force);
                }
                else if (_movingRight)
                {
                    _rigidbody.AddForce(new Vector2(jumpDistance, jumpHeight), ForceMode2D.Force);
                }
                _jumping = true;
            }
        }
	}
}
