using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController2D controller;
    public float runSpeed = 40f;

    public float health = 100f;
    public float damageTimerStart = 0.4f;
    private float damageTimer;
    private bool canTakeDamage = true;
    public RectTransform healthBar;
    private SpriteRenderer playerSprite;
    private Color initialColor;
    private Color finalColor;

    float horizontalMove = 0f;
    bool jump = false;

    bool atHospital = false;
    private void Start()
    {
        damageTimer = damageTimerStart;
        playerSprite = GetComponent<SpriteRenderer>();
        initialColor = playerSprite.color;
        finalColor = initialColor;
        finalColor.a = 1.0f /3;
    }

    void Update()
    {
        if (canTakeDamage)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Enemy"));
            if (hits.Length > 0)
            {
                canTakeDamage = false;
                health -= hits[0].gameObject.GetComponent<EnemyController>().damageValue;
                healthBar.localScale = new Vector3(Math.Max(health / 100,0),1,1);
                if (health <= 0)
                {
                    Die();
                }
            }
        }
        else
        {
            if (damageTimer > damageTimerStart*5f/6)
            {                
                playerSprite.color = Color.Lerp(initialColor, finalColor, 1f-(float)((damageTimer - 5f/6) * 6));
            } else if (damageTimer > damageTimerStart*4f/6)
            {
                playerSprite.color = Color.Lerp(finalColor, initialColor, 1f-(float)((damageTimer-4f/6)*6));
            } else if (damageTimer > damageTimerStart * 3f / 6)
            {
                playerSprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 0.5f) * 6));
            } else if (damageTimer > damageTimerStart*2f/6)
            {
                playerSprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 2f/6) * 6));
            } else if (damageTimer > damageTimerStart * 1f / 6)
            {
                playerSprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 1f/6) * 6));
            } else
            {
                playerSprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)(damageTimer * 6));
            }

            damageTimer -= Time.deltaTime;
            if(damageTimer < 0)
            {
                damageTimer = damageTimerStart;
                canTakeDamage = true;
            }
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove*Time.fixedDeltaTime,jump);
        jump = false;

    }

    void Die()
    {

    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            transform.parent = other.transform;
            atHospital = true;
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
