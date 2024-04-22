using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerController : MonoBehaviour
{
    private CharacterController2D controller;
    private Animator anim;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    float moveInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        controller.Flip();
    }

    void Update()
    {
        horizontalMove =  moveInput * runSpeed;
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove*Time.fixedDeltaTime,false);

        anim.SetBool("isRunning", (moveInput != 0));
    }

    public void MoveRight()
    {
        StartCoroutine(move(1));
    }

    public void MoveLeft()
    {
        StartCoroutine(move(-1));
    }

    IEnumerator move(int input) {
        yield return new WaitForSeconds((float)0.8);
        moveInput = input;
        yield return new WaitForSeconds((float)0.5);
        moveInput = 0;
    }

}
