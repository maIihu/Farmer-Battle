using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject pickCell;
    [FormerlySerializedAs("crop")] [SerializeField] private MapManager map;
    [SerializeField] private Tilemap tileMap;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    
    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    public int Score { get; set; }
    
    private bool _isHit;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Score = 0;
    }

    private void Update()
    {
        InputHandle();
        pickCell.transform.position = new Vector3((int)(this.transform.position.x) + 0.5f, 
            (int)(this.transform.position.y) - 0.5f, this.transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            map.Crop(this.transform.GetChild(0).transform.position, tileMap);
            _isHit = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            _isHit = false;
        }
        
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * moveSpeed;
    }

    private void InputHandle()
    {
        _moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            _moveInput.x = -1f;
        if (Input.GetKey(KeyCode.D))
            _moveInput.x = 1f;
        if (Input.GetKey(KeyCode.W))
            _moveInput.y = 1f;
        if (Input.GetKey(KeyCode.S))
            _moveInput.y = -1f;
        
        SetAnimation();
        FlipCharacter();
        
        _moveInput.Normalize();
    }

    private void SetAnimation()
    {
        _animator.SetFloat("Horizontal", _moveInput.x);
        _animator.SetFloat("Vertical", _moveInput.y);
        _animator.SetFloat("Speed", _moveInput.magnitude);
    }

    private void FlipCharacter()
    {
        if (_moveInput.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_moveInput.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bomb") && _isHit)
        {
            Debug.Log("Va cham vao bomb");
            other.gameObject.GetComponent<BombController>().ThrowingBomb(new Vector3(transform.position.x + 10, transform.position.y, 0));
        }
    }
}
