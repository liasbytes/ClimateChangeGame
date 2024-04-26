using JetBrains.Annotations;
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
    public Vector3 closestPlayerWaypoint;
    public Vector3 closestWaypoint;
    private int closestWaypointIndex;
    [SerializeField]
    public Transform playerPosition;
    [SerializeField]
    private float movementSpeed;
    public float thresholdDistance;
    public int currentTargetIndex = 0;
    private int moveDirection = 1;
    public float distance;

    private void Start()
    {
        damageTimer = damageTimerStart;
        enemySprite = GetComponent<SpriteRenderer>();
        initialColor = enemySprite.color;
        finalColor = initialColor;
        finalColor.a = 1.0f / 3;
        health = maxHealth;
        closestPlayerWaypoint = Vector3.positiveInfinity;
        closestWaypoint = Vector3.positiveInfinity;
    }

    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, playerPosition.position) <= thresholdDistance)
            {
                if (float.IsPositiveInfinity(Vector3.Distance(transform.position, closestPlayerWaypoint)))
                {
                    for (int i = 0; i < waypoints.Length; i++)
                    {
                        if (Vector3.Distance(waypoints[i], playerPosition.position) < Vector3.Distance(closestPlayerWaypoint, playerPosition.position))
                        {
                            closestPlayerWaypoint = waypoints[i];
                            closestWaypointIndex = i;
                        }
                        /*if (Vector3.Distance(waypoints[i],transform.position) > 0.1f)
                        {
                            
                            float angleCurrent = Vector3.Angle(playerPosition.position - transform.position, closestWaypoint - transform.position);
                            float angleNew = Vector3.Angle(playerPosition.position - transform.position, waypoints[i] - transform.position);
                            float weightedDistanceCurrent =  Vector3.Distance(closestWaypoint, transform.position)/ Mathf.Cos(angleCurrent);
                            float weightedDistanceNew =  Vector3.Distance(waypoints[i], transform.position)/ Mathf.Cos(angleNew);
                            if (weightedDistanceNew >= 0 && weightedDistanceNew < weightedDistanceCurrent)
                            {
                                closestWaypoint = waypoints[i];
                                currentTargetIndex = i;
                            }
                        }*/
                    }
                    int newTargetIndex = closestWaypointIndex;
                    if (closestWaypointIndex != currentTargetIndex)
                    {
                        for (int i = 0; i < waypoints.Length; i++)
                        {
                            if (i != currentTargetIndex)
                            {
                                if (Mathf.Sign(currentTargetIndex - i) == Mathf.Sign(closestWaypointIndex - i))
                                {
                                    if (Vector3.Distance(waypoints[i], transform.position) < Vector3.Distance(closestWaypoint, transform.position))
                                    {
                                        closestWaypoint = waypoints[i];
                                        newTargetIndex = i;
                                    }
                                }
                            }
                        }
                    }
                    currentTargetIndex = newTargetIndex;
                }
                distance = Vector3.Distance(transform.position, closestWaypoint);

                if (distance < 0.1)
                {
                    closestPlayerWaypoint = Vector3.positiveInfinity;
                    closestWaypoint = Vector3.positiveInfinity;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentTargetIndex], movementSpeed * Time.deltaTime);
                }
            }
            else
            {
                closestPlayerWaypoint = Vector3.positiveInfinity;
                closestWaypoint = Vector3.positiveInfinity;
                distance = Vector3.Distance(transform.position, waypoints[currentTargetIndex]);

                if (distance < 0.1)
                {
                    if (moveDirection == 1 && currentTargetIndex + 1 == waypoints.Length)
                    {
                        moveDirection = -1;
                    } else if (moveDirection == -1 && currentTargetIndex == 0)
                    {
                        moveDirection = 1;
                    }
                    currentTargetIndex += moveDirection;
                    currentTargetIndex = currentTargetIndex % waypoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentTargetIndex], movementSpeed * Time.deltaTime);
                }
            }
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
