using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraFollow : MonoBehaviour
{
    public Vector3 targetpos;
    public float speed = 50;
    private Vector3 velocity = Vector3.zero;
    private Vector3 homepos = new Vector3(13, 8, -10);
    // Start is called before the first frame update
    void Start()
    {
        transform.position = homepos;
        targetpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, speed * Time.deltaTime);
    }

    public void openExtras() 
    {
        StartCoroutine(waiter(new Vector3(18,0,0)));
    }

    public void closeExtras() 
    {
        StartCoroutine(waiter(new Vector3(0,0,0)));
    }

    public void openLoads()
    {
        StartCoroutine(waiter(new Vector3(0,-13,0)));
        //targetpos = transform.position + new Vector3(0,-13,0);
        
    }

    public void closeLoads()
    {
        StartCoroutine(waiter(new Vector3(0,0,0)));
    }

    IEnumerator waiter(Vector3 displacement) {
        yield return new WaitForSeconds((float)0.1);
        targetpos = homepos + displacement;
    }
}
