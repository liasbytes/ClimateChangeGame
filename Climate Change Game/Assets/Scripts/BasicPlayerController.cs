using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BasicPlayerController : MonoBehaviour
{

    private CharacterController2D controller;
    private Animator anim;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    public float startHealth;
    public float health;
    public float damageTimerStart = 0.4f;
    private float damageTimer;
    private bool canTakeDamage = true;
    public RectTransform healthBar;
    private Color initialColor;
    private Color finalColor;
    public Volume defaultVolume;
    private Vignette vignette;


    public Transform respawnLocation;
    public int cpNum;
    public Image deathScreen;
    private bool isDead;
    private bool isRespawned;
    private float deathTimer;

    InputAction moveAction;
    InputAction jumpAction;

    bool atHospital = false;

    void Awake() 
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        damageTimer = damageTimerStart;
        health = startHealth;
        cpNum = 0;
        isDead = false;
        isRespawned = false;
        deathTimer = 2;

        initialColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        finalColor = new Color(0.6f, 0.0f, 0.0f, 1.0f);
    }

    void Update()
    {

        if (isDead)
        {
            if (deathTimer > 1)
            {
                deathScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), deathTimer - 1f);
                deathTimer -= Time.deltaTime;
            }
            else if (deathTimer > 0)
            {
                if (!isRespawned) {
                    transform.position = respawnLocation.position;
                    healthBar.localScale = new Vector3(Math.Max(health / startHealth, 0), 1, 1);
                    isRespawned=true;
                }
                deathScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), deathTimer);
                deathTimer -= Time.deltaTime;
            }
            else
            {
                deathScreen.color = new Color(0,0,0,0);
                deathTimer = 2f;
                isRespawned = false;
                isDead = false;
                canTakeDamage = true;
            }
        }
        else
        {
            if (canTakeDamage)
            {
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Enemy"));
                if (hits.Length > 0)
                {
                    canTakeDamage = false;
                    health -= hits[0].gameObject.GetComponent<EnemyController>().damageValue;
                    healthBar.localScale = new Vector3(Math.Max(health / startHealth, 0), 1, 1);
                    if (health <= 0)
                    {
                        Die();
                    }
                }
            }
            else
            {
                defaultVolume.profile.TryGet(out vignette);
                {          
                    if (damageTimer > damageTimerStart * 3f / 4)
                    {
                        vignette.color.Override(Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 3f / 4) * 6)));
                    }
                    else if (damageTimer > damageTimerStart * 2f / 4)
                    {
                        vignette.color.Override(Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 0.5f) * 6)));
                    }
                    else if (damageTimer > damageTimerStart * 1f / 4)
                    {
                        vignette.color.Override(Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 1f / 4) * 6)));
                    }
                    else
                    {
                        vignette.color.Override(Color.Lerp(finalColor, initialColor, 1f - (float)(damageTimer * 4)));
                    }
                }

                damageTimer -= Time.deltaTime;
                if (damageTimer < 0)
                {
                    damageTimer = damageTimerStart;
                    canTakeDamage = true;
                }
            }

            horizontalMove = moveAction.ReadValue<Vector2>().x*runSpeed;
        
            if (jumpAction.IsPressed())
            {
                float currentVelo = GetComponent<Rigidbody2D>().velocity.y;
                /*float parentVelo = 0;
                if (transform.parent != null)
                {
                    Rigidbody2D parentRigidbody = transform.parent.GetComponent<Rigidbody2D>();
                    if (parentRigidbody != null)
                    {
                        parentVelo = parentRigidbody.velocity.y;
                    }
                }*/
                
                if (currentVelo <= 0.1f )
                {
                    if (controller.GetGrounded())
                    {
                        anim.SetTrigger("takeoff");
                    }
                    jump = true;
                }
            }
        }        
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            controller.Move(horizontalMove*Time.fixedDeltaTime,jump);
            anim.SetBool("isRunning", (horizontalMove != 0));
            anim.SetBool("isJumping", !controller.GetGrounded());
        }
        jump = false;
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.y);

    }

    void Die()
    {
        health = startHealth;
        isDead = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        //GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            transform.parent = other.transform;
            atHospital = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            CheckPointData cpd = other.transform.parent.GetComponent<CheckPointData>();
            if (cpd.checkpointNumber > cpNum)
            {
                cpNum = cpd.checkpointNumber;
                respawnLocation = cpd.respawnLocation;
            }
        } else if (other.gameObject.layer == 8)
        {
            Die();
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            //Debug.Log("unparented");
            transform.parent = null;
            Vector2 velo = GetComponent<Rigidbody2D>().velocity;
            //Debug.Log(other.transform.GetComponent<Rigidbody2D>().velocity.y);
            //velo.y -= other.transform.GetComponent<Rigidbody2D>().velocity.y;
            float offset = other.transform.GetComponent<Platform_one_movement>().velocity.y;
            if (offset < 0)
            {
                velo.y += offset;
            } else
            {
                velo.y += offset * 0.5f;
            }
            GetComponent<Rigidbody2D>().velocity = velo;
            
            atHospital = false;
        }
    }

    public bool GetCollisions() {
        return atHospital;
    }
}