using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public AudioSource walk;
    public AudioSource jump;
    public AudioSource attack;

    public void startWalk()
    {
        if (!walk.isPlaying)
        {
            walk.Play();
        }
    }

    public void endWalk()
    {
        if (walk.isPlaying)
        {
            walk.Stop();
        }
    }

    public void startJump()
    {
        jump.Play();
    }

    public void startAttack()
    {
        attack.Play();
    }
}
