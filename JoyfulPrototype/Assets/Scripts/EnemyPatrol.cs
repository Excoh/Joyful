using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {
    
    //Psuedo-Constants
    Vector3 LEFT_SPRITE = new Vector3(1f, 1f, 1f);
    Vector3 RIGHT_SPRITE = new Vector3(-1f, 1f, 1f);
    
    //Public Members
    public float moveSpeed;

    //Private Members
    private Rigidbody2D _rigidbody;
    private bool _movingRight;
	private Transform _wallCheck;
	private float _wallCheckRadius;
	private LayerMask _whatIsWall;
    private bool _hittingWall;
    private bool _hasRoomToMove;
    private Transform _edgeCheck;

	// Use this for initialization
	void Start () {
        _Init_Enemy();
	}

    private void _Init_Enemy()
    {
        try {
            _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
            _wallCheck = this.gameObject.transform.FindChild("WallCheck");
            _edgeCheck = this.gameObject.transform.FindChild("EdgeCheck");
            _whatIsWall = LayerMask.GetMask("Ground"); ;
            _movingRight = false;
            _wallCheckRadius = 0.1f;
        }
        catch
        {
            Debug.Log("Something went wrong with _Init_Enemy()");
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
        this._hittingWall = Physics2D.OverlapCircle(_wallCheck.position, _wallCheckRadius, _whatIsWall);
        this._hasRoomToMove = Physics2D.OverlapCircle(_edgeCheck.position, _wallCheckRadius, _whatIsWall);
    }

    private void _Think()
    {
        if (_hittingWall || !_hasRoomToMove)
        {
            _movingRight = !_movingRight;
        }
    }

    private void _Act()
    {
        if (_movingRight)
        {
            transform.localScale = RIGHT_SPRITE;
            _rigidbody.velocity = new Vector2(moveSpeed, _rigidbody.velocity.y);
        }
        else
        {
            transform.localScale = LEFT_SPRITE;
            _rigidbody.velocity = new Vector2(-moveSpeed, _rigidbody.velocity.y);
        }
    }
}
