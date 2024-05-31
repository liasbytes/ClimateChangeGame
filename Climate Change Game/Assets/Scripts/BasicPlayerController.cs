using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class BasicPlayerController : MonoBehaviour
{

    private CharacterController2D controller;
    private Animator anim;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool attack = false;

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
    public FadeUI blackScreen;


    public Transform respawnLocation;
    public int cpNum;
    public Image deathScreen;
    private bool isDead;
    private bool isRespawned;
    private float deathTimer;

    InputAction moveAction, jumpAction, attackAction;
    public LayerMask m_EnemyLayer;

    bool atHospital = false;
    bool m_Started;
    public ParticleSystem attackEffect;

    void Awake() 
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");

        damageTimer = damageTimerStart;
        health = startHealth;
        cpNum = 0;
        isDead = false;
        isRespawned = false;
        deathTimer = 2;

        initialColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        finalColor = new Color(0.6f, 0.0f, 0.0f, 1.0f);
        
        m_Started = true;
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
        

            if (attackAction.WasPressedThisFrame()) {
                if (controller.GetGrounded()) {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
                    anim.SetTrigger("attack");
                    attack = true;
                }
            }
            else if (jumpAction.IsPressed())
            {
                float currentVelo = GetComponent<Rigidbody2D>().velocity.y;
                
                if (currentVelo <= 0.1f )
                {
                    if (controller.GetGrounded() && !attack)
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
            if (!attack) {
                controller.Move(horizontalMove*Time.fixedDeltaTime,jump);
            }
            anim.SetBool("isRunning", (horizontalMove != 0));
            anim.SetBool("isJumping", !controller.GetGrounded());
        }
        jump = false;
    }

    public void AttackEnd()
    {
        attack = false;
    }

    public void AttackEnemies() {
        Vector3 boxPosition = GetComponent<Transform>().position;
        ParticleSystemRenderer attackRenderer = attackEffect.GetComponent<ParticleSystemRenderer>();
        var vel = attackEffect.velocityOverLifetime;
        // determines attack offset
        if (controller.GetFacingRight()) {
            boxPosition += new Vector3(2f,0f,0f);
            attackRenderer.pivot = new Vector3(1f,0f,0f);
            vel.x = new ParticleSystem.MinMaxCurve(10f);
        } else {
            boxPosition += new Vector3(-2f,0f,0f);
            attackRenderer.pivot = new Vector3(-1f,0f,0f);
            vel.x = new ParticleSystem.MinMaxCurve(-10f);
        }
        attackEffect.Play();
        // determines attack range
        Collider2D[] enemies = Physics2D.OverlapBoxAll(boxPosition, new Vector3(5f,2f,0f),0f, m_EnemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject != gameObject)
            {
                enemies[i].GetComponent<EnemyController>().takeDamage(5f);
            }
        }  
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started) {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(GetComponent<Transform>().position + new Vector3(2f,1f,0f), new Vector3(5f,2f,0));
        }
    }

    void Die()
    {
        health = startHealth;
        isDead = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
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
        } else if (other.gameObject.tag == "CityLevelEnd")
        {
            Time.timeScale = 1f;
            StartCoroutine(fadeAndLoad());
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.transform.tag == "MovingPlatform") {
            transform.parent = null;
            Vector2 velo = GetComponent<Rigidbody2D>().velocity;
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

    IEnumerator fadeAndLoad()
    {
        blackScreen.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("WinScreen");
    }
}