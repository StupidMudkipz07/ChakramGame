using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float groundedMoveSpeed = 10;
    [SerializeField] float groundedAcceleration = 400;
    [SerializeField] float aerialMoveSpeed = 9;
    [SerializeField] float aerialAcceleration = 180;

    [Header("Jump")]
    [SerializeField] float gravity = 50;
    [SerializeField] float jumpForce = 16;
    [SerializeField] float minimalJumpForce = 6;
    [SerializeField] float jumpBufferInSeconds = 0.19f;
    [SerializeField] float hangTimeInSeconds = 0.05f;
    [SerializeField] float hangTimeGravity = -4;
    [SerializeField] float roofSpeedSet = -1;
    [SerializeField] float coyoteTime = 0.1f;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 27;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0.56f;

    [Header("Wall")]
    [SerializeField] float wallSlideSpeed = 20;
    [SerializeField] float wallJumpHeight = 20;
    [SerializeField] float wallJumpSpeed = 50;
    [SerializeField] float wallGrabSpeedDecreaseMultiplier = 1;

    [Header("Chakram")]
    [SerializeField] float ChakramThrowSpeed;

    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] CollisionEvent _groundCheck;
    [SerializeField] CollisionEvent _roofCheck;
    [SerializeField] CollisionEvent _leftWallCheck;
    [SerializeField] CollisionEvent _rightWallCheck;

    [SerializeField] InputHandler _inputHandler;
    [SerializeField] ChakramController chakram;

    [Header("Debug stats")]
    [SerializeField] float _jumpBufferTime;
    float _hangTimer;
    bool _hasJumped;
    Vector2 _dashDirection;
    float _dashCooldown;
    PlayerInput _input;
    bool _isDashing;
    [SerializeField] bool _isWallSliding;
    [SerializeField] float _coyoteTimer;

    //velocity
    public Vector2 velocity;

    [Header("Player Events")]
    [SerializeField] public UnityEvent OnJump;
    [SerializeField] public UnityEvent OnWallJump;
    [SerializeField] public UnityEvent OnDash;
    [SerializeField] public UnityEvent OnEndDash;
    [SerializeField] public UnityEvent OnThrow;
    [SerializeField] public UnityEvent OnTouchingGround;
    [SerializeField] public UnityEvent OnGrabbingWall;
    [SerializeField] public UnityEvent OnBonkinghead;

    void Start()
    {
        _leftWallCheck.OnTouching.AddListener(OnTouchingWall);
        _rightWallCheck.OnTouching.AddListener(OnTouchingWall);
        _groundCheck.OnTouching.AddListener(OnGroundTouch);
        _roofCheck.OnTouching.AddListener(OnTouchingRoof);
    }

    // Capture input in the input update loop, physics may run less often. chatgpt ass comment
    void Update()
    {
        _input = _inputHandler.GetPlayerInput();

        if (_input.JumpPressed)
        {
            _jumpBufferTime = jumpBufferInSeconds;
        }

        if (_input.DashPressed)
        {
            if (_dashCooldown <= 0)
            {
                StartCoroutine(Dash());
            }
        }
        CheckThrow();
    }

    //physics here
    void FixedUpdate()
    {
        UpdateValues();

        //isnt dashing
        if (!_isDashing)
        {
            StateCheck();
        }

        ChangeVelocity();
    }

    #region Throw

    void CheckThrow()
    {
        if (_input.Throw && chakram.IsConnectedToTarget)
        {
            //Ska chakram kastet vara baserat på spelarens speed? 
            chakram.Throw(_input.AimDirection.normalized * ChakramThrowSpeed);
            OnThrow.Invoke();
        }
    }

    #endregion

    #region Movement

    void Grounded()
    {
        velocity.x = Mathf.MoveTowards(velocity.x, _input.Move.x * groundedMoveSpeed, groundedAcceleration * Time.fixedDeltaTime);
        velocity.y = 0;
        _coyoteTimer = coyoteTime;

        JumpCheck();
    }

    void WallSlide()
    {
        //Horiztonal velocity
        velocity.x = Mathf.MoveTowards(velocity.x, _input.Move.x * aerialMoveSpeed, aerialAcceleration * Time.fixedDeltaTime);

        WallSlideCheck();

        //if the player is moving up normal gravity is applied, otherwise apply wallgravity
        var actualGravity = velocity.y > 0 ? gravity : wallSlideSpeed;
        velocity.y -= actualGravity * Time.fixedDeltaTime;

        WallJumpCheck();
    }

    void Airborne()
    {
        velocity.x = Mathf.MoveTowards(velocity.x, _input.Move.x * aerialMoveSpeed, aerialAcceleration * Time.fixedDeltaTime);

        WallSlideCheck();

        //gör coyote time möjligt
        if (_coyoteTimer > 0)
        {
            JumpCheck();
        }
        //rising
        if (_rb.linearVelocity.y > 0)
        {
            //allows you to cut your jump short if you realease the jumpbutton
            if (!_input.JumpHeld)
            {
                velocity.y = MathF.Min(minimalJumpForce, velocity.y);
                //cuts the hangtime so that the player starts falling immedieatly
                _hasJumped = false;
            }
        }
        //the player is at the peak of its jump or is falling
        else
        {
            if (_hasJumped)
            {
                StartHangTime();
            }
        }
        var actualGravity = _hangTimer <= 0 ? gravity : hangTimeGravity;

        velocity.y -= actualGravity * Time.fixedDeltaTime;
    }

    void OnGroundTouch()
    {
        _hasJumped = false;

        OnTouchingGround.Invoke();
    }

    void ExecuteJump()
    {
        _jumpBufferTime = 0;
        velocity.y = jumpForce;
        _hasJumped = true;
        _coyoteTimer = 0;

        OnJump.Invoke();
    }

    void ExecuteWallJump()
    {
        _jumpBufferTime = 0;
        velocity.y = wallJumpHeight;
        _hasJumped = true;
        //launches you out of the wall
        velocity.x = wallJumpSpeed * -GetWallDirection().x;

        OnWallJump.Invoke();
    }

    void JumpCheck()
    {
        //jump if there is buffertime left
        if (_jumpBufferTime > 0)
        {
            ExecuteJump();
        }
    }

    void WallJumpCheck()
    {
        //jump if there is buffertime left
        if (_jumpBufferTime > 0)
        {
            ExecuteWallJump();
        }
    }

    void StartHangTime()
    {
        //Debug.Log("hangtime");
        _hangTimer = hangTimeInSeconds;
        _hasJumped = false;
    }

    void OnTouchingRoof()
    {
        velocity.y = roofSpeedSet;
        OnBonkinghead.Invoke();
    }
   
    IEnumerator Dash()
    {
        //before dash
        _dashDirection = _input.Move;

        velocity = _dashDirection * dashSpeed;
        _isDashing = true;
        _dashCooldown = dashCooldown + dashDuration;
        _hasJumped = false; //stänger av hangtime, kanske kan vara true

        OnDash.Invoke();

        yield return new WaitForSeconds(dashDuration);

        //after dash
        velocity = Vector2.zero;
        _isDashing = false;

        StartHangTime();
        OnEndDash.Invoke();
    }

    Vector2 GetWallDirection()
    {
        //maybe fix edge case where to you collide with both colliders at the same time
        if (_leftWallCheck.IsTouching)
        {
            //Debug.Log("touching left wall");
            return Vector2.left;
        }
        else if (_rightWallCheck.IsTouching)
        {
            //Debug.Log("touching right wall");
            return Vector2.right;
        }

        return Vector2.zero;
    }

    void WallSlideCheck()
    {
        //player is in the air and touching a wall
        if (GetWallDirection().x != 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    void OnTouchingWall()
    {
        //moving down and not dashing
        if (velocity.y < 0 && !_isDashing)
        {
            //negates one tick of grtavity times the multiplier
            velocity.y = wallSlideSpeed * Time.fixedDeltaTime * wallGrabSpeedDecreaseMultiplier;

            OnGrabbingWall.Invoke();
            Debug.Log("touched wall and slowed down");
        }

    }

    void StateCheck()
    {
        //grounded
        if (_groundCheck.IsTouching)
        {
            Grounded();
        }
        //wall Sliding
        else if (_isWallSliding)
        {
            WallSlide();
        }
        //airborne
        else
        {
            Airborne();
        }
    }

    void ChangeVelocity()
    {
        _rb.linearVelocity = velocity;
    }

    void UpdateValues()
    {
        _jumpBufferTime = MathF.Max(_jumpBufferTime - Time.fixedDeltaTime, 0);
        _hangTimer = MathF.Max(_hangTimer - Time.fixedDeltaTime, 0);
        _dashCooldown = MathF.Max(_dashCooldown - Time.fixedDeltaTime, 0);
        _coyoteTimer = MathF.Max(_coyoteTimer - Time.fixedDeltaTime, 0);
    }

    #endregion
}