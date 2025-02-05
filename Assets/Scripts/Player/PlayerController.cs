using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Player Inventory
    public PlayerInventory playerInventory;
    public GameObject keyItem;
    
    //Variables for player and camera movement
    [SerializeField] float turnSpeed; private bool _isGrounded; private Rigidbody _rb;
    float currentAngle; float currentAngleVelocity;
    public float speed = 5.0f, jumpHeight = 5.0f, gravity = 10f; private float _rotInput;
    
    private Vector3 _input;
    private Camera _camera;
    public bool[] hasKey = new bool[2];
    public bool isWarping;
    [SerializeField] bool canInteract;
    public KeyCode interactKey = KeyCode.E;
    public Canvas pauseMenu;
    private bool _isPaused;
    
    private MyMethods MyMethods;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
        playerInventory.Container.Clear();
        MyMethods = gameObject.AddComponent<MyMethods>();
        _isPaused = false;
    }
    
    private void Update()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Move();
        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }
        
        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.down * gravity);
        }
        
        if (Input.GetKeyDown(interactKey) && canInteract)
        {
            Interact(); // not finished, need to add logic for this!
        }
        
        if (pauseMenu.gameObject.activeSelf)
        {
            _isPaused = true;
        }
        else
        {
            _isPaused = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)  
        {
            MyMethods.Unpause();
            MyMethods.DisableMenu(pauseMenu.gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            MyMethods.Pause();
            MyMethods.EnableMenu(pauseMenu.gameObject);
        }
        
    }

    private void Move()
    {
        if (_input.magnitude >= 0.1f)
        {
            // Calculate the angle to rotate the player
            float targetAngle = Mathf.Atan2(_input.x, _input.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngleVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            
            // Player movement
            Vector3 moveCorrect = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            transform.localPosition += speed * Time.deltaTime * moveCorrect;
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        _isGrounded = false;
    }
    
    private void Interact() // not finished, need to add logic for this!
    {
        
    }
    
    private void OnCollisionEnter (Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Ground":
                _isGrounded = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        switch (hit.gameObject.tag)
        {
            case "OOB":
                transform.position = new Vector3(9, 1, 0);
                break;
            case "dimensionMain":
                isWarping = false;
                break;
            case "dimensionFuture":
                isWarping = true;
                break;
        }
        
        switch (hit.GetComponent<MonoBehaviour>())
        {
            case Item item:
                playerInventory.AddItem(item.item, 1);
                Destroy(hit.gameObject);
                switch (hit.gameObject.tag)
                {
                    case "keyItem1":
                        if(hasKey.Length > 0)
                            hasKey[0] = true;
                        break;
                    case "keyItem2":
                        if(hasKey.Length > 1)
                            hasKey[1] = true;
                        break;
                }
                break;
        }
        
        
        
    }
    private void OnApplicationQuit()
    {
        playerInventory.Container.Clear();
    }
    
}
