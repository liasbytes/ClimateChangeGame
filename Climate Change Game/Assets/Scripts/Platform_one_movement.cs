using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code based upon https://medium.com/@pkillman2000/moving-platforms-in-unity-c7548fe1220c

public class Platform_one_movement : MonoBehaviour
{
    [SerializeField]
    private Vector3[] waypoints;
    public float movementSpeed;
    private int currentTargetIndex = 0;
    private float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, waypoints[currentTargetIndex]);

        if (distance < 0.1) {
            currentTargetIndex++;
            currentTargetIndex = currentTargetIndex % waypoints.Length;
        } else {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentTargetIndex], movementSpeed * Time.deltaTime);
        }
    }
}
