using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code based platform movement code (which is based upon https://medium.com/@pkillman2000/moving-platforms-in-unity-c7548fe1220c )
public class EnemyController : MonoBehaviour
{
    public float damageValue;
    public bool moving;
    [SerializeField]
    private Vector3[] waypoints;
    [SerializeField]
    private float movementSpeed;
    private int currentTargetIndex = 0;
    private float distance;

    void Update()
    {
        if (moving)
        {
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
        }
    }
}
