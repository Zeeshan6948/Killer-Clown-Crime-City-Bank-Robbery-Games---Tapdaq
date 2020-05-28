using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationResponce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationEnded()
    {
        LevelManager.m_instance.DanceScript.AwardeReward(5000);
    }
}
