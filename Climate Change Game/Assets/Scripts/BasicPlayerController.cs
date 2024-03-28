using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{

    public CharacterController2D controller;
    private Animator anim;
    public float runSpeed = 40f;

    public float health = 100f;

    float horizontalMove = 0f;
    bool jump = false;
    float moveInput = 0;

    void Start() 
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        horizontalMove =  moveInput * runSpeed;
        
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("takeoff");
            jump = true;
        }
        
        
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove*Time.fixedDeltaTime,jump);
        jump = false;

        anim.SetBool("isRunning", (moveInput != 0));
        anim.SetBool("isJumping", !controller.GetGrounded());
    }

    void Die()
    {

    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            transform.parent = null;
        }
    }

}