﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("after", 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void after()
    {
        SceneManager.LoadScene("Main");
    }
}
