﻿using UnityEngine;
using System.Collections;

public class AngerEnemyMovement : MonoBehaviour
{

    //Psuedo-Constants
    Vector3 LEFT_SPRITE = new Vector3(1f, 1f, 1f);
    Vector3 RIGHT_SPRITE = new Vector3(-1f, 1f, 1f);

    //Public Members
    public float moveSpeed;
    public float wallCheckRadius;
    public float groundCheckRadius;
    public float playerCheckRadius;
    public LayerMask whatIsWall; //Lefted as public because Wall may have its own Layer Mask.
    public int distanceOfPace;

    //Private Members
    private Vector2 _startPosition;
    private Vector2 _LeftMostPosition;
    private Vector2 _RightMostPosition;
    private Rigidbody2D _rigidbody;
    private Transform _wallCheck;
    private Transform _groundCheck;
    private Transform _edgeCheck;
    private Transform _playerTransform;
    private bool _hasRoomToMove;
    private bool _grounded;
    private bool _hittingWall;
    private LayerMask _groundLayerMask;
    private bool _movingLeft;
    private bool _movingRight;
    private bool _returning;
    private float _distanceFromPlayer;
    private bool _isPacing;
    private bool _NeedsToMoveLeft;


    void Start()
    {
        _Init_Fear();
    }

    private void _Init_Fear()
    {
        try
        {
            _startPosition = this.gameObject.transform.position;
            _LeftMostPosition = new Vector2(_startPosition.x - distanceOfPace, _startPosition.y);
            _RightMostPosition = new Vector2(_startPosition.x + distanceOfPace, _startPosition.y);
            _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
            _wallCheck = this.gameObject.transform.FindChild("WallCheck");
            _groundCheck = this.gameObject.transform.FindChild("GroundCheck");
            _edgeCheck = this.gameObject.transform.FindChild("EdgeCheck");
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _groundLayerMask = LayerMask.GetMask("Ground");
            _movingLeft = true;
            _movingRight = false;
            _returning = false;
            _isPacing = true;
            _NeedsToMoveLeft = true;
        }
        catch
        {
            Debug.Log("Something is wrong with _Init_Anger()");
        }
    }

    void Update()
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
        if (_distanceFromPlayer < playerCheckRadius) //If player is within range.
        {
            _isPacing = false;
        }
        else if (!_isPacing)
        {
            _isPacing = true;
        }    
    }

    private void _Act()
    {
        if (_isPacing)
        {
            _Pacing();
        }
        else
        {
            if (_returning)
            {
                _ReturnToStartPosition();
            }
            else if (_distanceFromPlayer < playerCheckRadius && _distanceFromPlayer > 0.1f) //If player is within range.
            {
                _Chase();
            } 
        }
    }

    void _Pacing()
    {
        float relativePoint;
        //Determine what direction it must pace towards.
        if (_NeedsToMoveLeft)
        {
            relativePoint = _LeftMostPosition.x - transform.position.x;
        }
        else
        {
            relativePoint = _RightMostPosition.x - transform.position.x;
        }

        //If at the edge of pacing range. Change directions.
        if(Mathf.Abs(relativePoint) < 1f)
        {
            _NeedsToMoveLeft = !_NeedsToMoveLeft;
        }

        _Move(relativePoint);
    }

    private void _Chase()
    {
        if (_hasRoomToMove)
        {
            _Move(_playerTransform.position.x - transform.position.x);
        }
        else
        {
            _returning = true;
        }
    }

    private void _Move(float relativePoint)
    {
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

    private void _ReturnToStartPosition()
    {
        float relativePoint= _startPosition.x - transform.position.x;
        if (Mathf.Abs(relativePoint) > 0.1f)
        {
            _Move(relativePoint);
        }
        else
        {
            _isPacing = true;
            _returning = false;
        }
    }
}