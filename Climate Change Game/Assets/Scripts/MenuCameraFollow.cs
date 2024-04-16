using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraFollow : MonoBehaviour
{
    public Vector3 targetpos;
    public float speed = 40;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(13, 8, 5);
        targetpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, speed * Time.deltaTime);
    }

    public void openExtras() 
    {
        targetpos = transform.position + new Vector3(26,0,0);
    }

    public void closeExtras() 
    {
        targetpos = transform.position + new Vector3(-26,0,0);
    }

    public void openLoads()
    {
        // for downwards moving load game click
    }
}
