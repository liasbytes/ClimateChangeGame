using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
// Code based platform movement code (which is based upon https://medium.com/@pkillman2000/moving-platforms-in-unity-c7548fe1220c )
public class EnemyController : MonoBehaviour
{
    public float damageValue;
    public float maxHealth;
    public float health;
    public float damageTimerStart = 0.4f;
    private float damageTimer;
    private bool isDamaged = false;
    private Color initialColor;
    private Color finalColor;
    private SpriteRenderer enemySprite;


    public Transform healthBar;
    public bool moving;
    [SerializeField]
    private Vector3[] waypoints;
    public Vector3 closestWaypoint;
    [SerializeField]
    public Transform playerPosition;
    [SerializeField]
    private float movementSpeed;
    public float thresholdDistance;
    private int currentTargetIndex = 0;
    private float distance;

    private void Start()
    {
        damageTimer = damageTimerStart;
        enemySprite = GetComponent<SpriteRenderer>();
        initialColor = enemySprite.color;
        finalColor = initialColor;
        finalColor.a = 1.0f / 3;
        health = maxHealth;
        closestWaypoint = Vector3.positiveInfinity;
    }

    void Update()
    {
        if (moving)
        {
            /*if (Vector3.Distance(transform.position, playerPosition.position) <= thresholdDistance)
            {
                if (closestWaypoint == Vector3.positiveInfinity)
                {
                    for (int i = 0; i <waypoints.Length; i++)
                    {
                        if (Vector3.Distance(waypoints[i], playerPosition.position) <= Vector3.Distance(closestWaypoint, playerPosition.position))
                        {
                            closestWaypoint = waypoints[i];
                            currentTargetIndex = i;
                        }
                    }
                }
                distance = Vector3.Distance(transform.position, waypoints[currentTargetIndex]);

                if (distance < 0.1)
                {
                    closestWaypoint = Vector3.positiveInfinity;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentTargetIndex], movementSpeed * Time.deltaTime);
                }
            }
            else
            {*/
                closestWaypoint = Vector3.positiveInfinity;
                distance = Vector3.Distance(transform.position, waypoints[currentTargetIndex]);

                if (distance < 0.1)
                {
                    currentTargetIndex++;
                    currentTargetIndex = currentTargetIndex % waypoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentTargetIndex], movementSpeed * Time.deltaTime);
                }
            //}
        }

        if (isDamaged)
        {
            if (damageTimer > damageTimerStart * 5f / 6)
            {
                enemySprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 5f / 6) * 6));
            }
            else if (damageTimer > damageTimerStart * 4f / 6)
            {
                enemySprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 4f / 6) * 6));
            }
            else if (damageTimer > damageTimerStart * 3f / 6)
            {
                enemySprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 0.5f) * 6));
            }
            else if (damageTimer > damageTimerStart * 2f / 6)
            {
                enemySprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)((damageTimer - 2f / 6) * 6));
            }
            else if (damageTimer > damageTimerStart * 1f / 6)
            {
                enemySprite.color = Color.Lerp(initialColor, finalColor, 1f - (float)((damageTimer - 1f / 6) * 6));
            }
            else
            {
                enemySprite.color = Color.Lerp(finalColor, initialColor, 1f - (float)(damageTimer * 6));
            }

            damageTimer -= Time.deltaTime;
            if (damageTimer < 0)
            {
                damageTimer = damageTimerStart;
                isDamaged = false;
            }
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        isDamaged = true;
        healthBar.localScale = new Vector3(0.95f * health / maxHealth,0.6f,1);
        healthBar.position = new Vector3(-(1-(0.95f * health / maxHealth))/2f, 0, 0);
    }
}
