using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [Header("Components")]
    [SerializeField] private  LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;   
    private Rigidbody2D _rb;
    public Animator _anim;
    [Header("State Variables")]
    public bool _isWaiting;
    [Header("Movement Values")]
    [SerializeField] private float _movementAcceleration = 75f;
    [SerializeField] private float _maxMovementSpeed = 5f;
    private bool _canMove => !_isWaiting && !_canWallSlide;
    private bool _isMoving => _horizontalDirection != 0;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);
    private float _verticalDirection;
    private float _horizontalDirection;
    public bool _facingRight = true;

    [Header("Jump Values")]
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private int _extraJumps = 1;
    private bool _jumpActivated;
    private bool _canJump => Input.GetButtonDown("Jump") && _verticalDirection != -1 && _extraJumps > 0 &&!_isWaiting &&!_isAttacking;
    private bool _isJumping => _rb.velocity.y > 0f;
    private bool _isFalling => _rb.velocity.y < 0f &&!_canWallSlide;
    private bool _canWallSlide => _onWall && !_onGround &&!_isJumping;
    [Header("Dash Values")]
    [SerializeField] private float _dashDuration = .3f;
    
    [SerializeField] private float _dashForce = 20;
    private bool _isDashing;
    private bool _canDash => Input.GetButtonDown("Jump") && _verticalDirection == -1 && _horizontalDirection != 0 &&!_isWaiting &&!_isAttacking;
    [Header("Attack Values")]
    public GameObject _attackArea = default;
    public bool _isAttacking;
    private bool _canAttack => Input.GetButtonDown("Fire1")&&!_isAttacking &&!_isDashing &&!_canWallSlide;
    [Header("Physics Values")]
    [SerializeField] private float _groundLinearDrag = 10f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 7f;
    [SerializeField] private float _lowFallMultiplier = 10f;
    [SerializeField] private float _gravitationAcceleration = .3f;
    [Header("Collision Variables")]
    [SerializeField] private float _wallRaycastLength;
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;
    private bool _onWall;
    private bool _onRightWall;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {    
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalDirection = GetInput().x;
        _verticalDirection = GetInput().y;
        if (_canJump) Jump(Vector2.up);
        if(_onWall) StartCoroutine(WallJump());
        if (_canAttack)Attack();
        if (_canDash) StartCoroutine(Dash(_horizontalDirection));
        //if (_canWallSlide) WallSlide();
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        ApplyLinearDrag();  
        if (_canMove) MoveCharacter();
        
        if (_onGround) 
            _extraJumps = 1;
        //Animation
        Animation();
        _anim.SetBool("onGround", _onGround);
        _anim.SetFloat("horizontalDirection", Mathf.Abs( _horizontalDirection));
        _anim.SetBool("isJumping", _isJumping);
        _anim.SetBool("isFalling", _isFalling);
        _anim.SetBool("isDashing", _isDashing);
        _anim.SetBool("canWallSlide", _canWallSlide);
        _anim.SetBool("isAttacking", _isAttacking);
    }
    private void Animation()
    {
        if ((_horizontalDirection < 0f && _facingRight || _horizontalDirection > 0f && !_facingRight) &&!_canWallSlide) Flip();
    }
    //////////////////////////////////////////////////////////////Animtions//////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////Animtions_END//////////////////////////////////////////////////////////////////////
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    /////////////////////////////////////////////////////////////Player_Controls////////////////////////////////////////////////////////////////////

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _isAttacking = true;
            _rb.velocity = new Vector2(_horizontalDirection, 0f);
        }
    }
    
    private void MoveCharacter()
    {      
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);
        if(Mathf.Abs(_rb.velocity.x) > _maxMovementSpeed) _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x)*_maxMovementSpeed,_rb.velocity.y);
    }
    private void Jump(Vector2 direction)
    {     
        if (!_onGround && !_onWall) _extraJumps--;
        _rb.gravityScale = 1f;
        _rb.velocity = new Vector2(0f, 0f);
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
    }
    private IEnumerator WallJump()
    {
        float jumpTime = Time.time;
        _rb.velocity = new Vector2(0f, 0f);
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;      
        Jump(Vector2.up *1.3f + jumpDirection);
        Flip();
        while (Time.time < jumpTime + 0.3f)
        {
            _isWaiting = true;
            yield return null;          
            if (_onGround)
            {
                _isWaiting = false;
                yield break;     
            }
        }
        _isWaiting = false;
    }
    private IEnumerator Dash(float x)
    {
        float dashStartTime = Time.time;
        Vector2 dir = new Vector2(x, -.4f);
        while(Time.time < dashStartTime + _dashDuration)
        {
            _isDashing = true;
            _isWaiting = true;
            _rb.velocity = dir.normalized * _dashForce;
            yield return null; 
            if (Input.GetButtonDown("Jump"))
            {
                _isDashing = false;
                _isWaiting = false;
                Jump(Vector2.up / 3);
                yield break;
            }    
        }
        _isDashing = false;
        _isWaiting = false;
        if(_onGround)Jump(Vector2.up/1.5f);
    }
    /////////////////////////////////////////////////////////////Player_Controls-END///////////////////////////////////////////////////////////////
    //-------------------------------------------------------------------------------------------------------------------------------------------//
    //////////////////////////////////////////////////////////Player_Enviroment_Physics///////////////////////////////////////////////////////////
    private void Flip()
    {
            _facingRight = !_facingRight;
            transform.Rotate(0f, 180f, 0f);
    }
 
    private void ApplyLinearDrag()
    {
        //Ground Linear Drag//
        if (_onGround)
        {
            if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection) _rb.drag = _groundLinearDrag;
            else _rb.drag = 0f;
            _rb.gravityScale = 1f;
            FallMultiplier();
        }
        //Air Linear Drag//
        else
        {
            _rb.drag = _airLinearDrag;
            FallMultiplier();
        }
    }
    private void WallSlide()
    {
        if(_onWall && !_facingRight && !_onRightWall) Flip();
        else if(_onWall && _facingRight && _onRightWall) Flip();
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMovementSpeed);    
    }
    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0) _rb.gravityScale = _fallMultiplier;
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump")) _rb.gravityScale = _lowFallMultiplier;
        else if (_rb.velocity.y > 0) _rb.gravityScale += _gravitationAcceleration;
    }
    private void CheckCollisions()
    {
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);  
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        //Wall Check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }
    //////////////////////////////////////////////////////////Player_Enviroment_Physics-End///////////////////////////////////////////////////////
    //------------------------------------------------------------------------------------------------------------------------------------------//
}
