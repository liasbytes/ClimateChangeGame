using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public CharacterController2D controller;
    public float runSpeed = 40f;

    public float startHealth;
    public float health;
    public float damageTimerStart = 0.4f;
    
    private float damageTimer;
    private bool canTakeDamage = true;
    public RectTransform healthBar;
    private SpriteRenderer playerSprite;
    private Color initialColor;
    private Color finalColor;

    public Transform respawnLocation;
    public int cpNum;
    public Image deathScreen;
    private bool isDead;
    private bool isRespawned;
    private float deathTimer;

    float horizontalMove = 0f;
    bool jump = false;
    InputAction moveAction;
    InputAction jumpAction;

    bool atHospital = false;
    private void Start()
    {
        damageTimer = damageTimerStart;
        playerSprite = GetComponent<SpriteRenderer>();
        initialColor = playerSprite.color;
        finalColor = initialColor;
        finalColor.a = 1.0f /3;
        health = startHealth;
        cpNum = -1;
        isDead = false;
        isRespawned = false;
        deathTimer = 2;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
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
                if (damageTimer > damageTimerStart * 5f / 6)
                {
                    playerSprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 5f / 6) * 6));
                }
                else if (damageTimer > damageTimerStart * 4f / 6)
                {
                    playerSprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 4f / 6) * 6));
                }
                else if (damageTimer > damageTimerStart * 3f / 6)
                {
                    playerSprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 0.5f) * 6));
                }
                else if (damageTimer > damageTimerStart * 2f / 6)
                {
                    playerSprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 2f / 6) * 6));
                }
                else if (damageTimer > damageTimerStart * 1f / 6)
                {
                    playerSprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 1f / 6) * 6));
                }
                else
                {
                    playerSprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)(damageTimer * 6));
                }

                damageTimer -= Time.deltaTime;
                if (damageTimer < 0)
                {
                    damageTimer = damageTimerStart;
                    canTakeDamage = true;
                }
            }

            //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            horizontalMove = moveAction.ReadValue<Vector2>().x*runSpeed;

            //if (Input.GetButtonDown("Jump"))
            if (jumpAction.IsPressed())
            {
                jump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        }
        jump = false;

    }

    void Die()
    {
        health = startHealth;
        isDead = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
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
            transform.parent = null;
            atHospital = false;
        }
    }

    public bool GetCollisions() {
        return atHospital;
    }
}
