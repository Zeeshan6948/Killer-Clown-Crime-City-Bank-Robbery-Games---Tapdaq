using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform Target;
    public bool Lookat;
    public bool Rotatearound;
    public int Speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lookat)
            transform.LookAt(Target);
        if (Rotatearound)
            transform.RotateAround(Target.position, Vector3.up, Speed * Time.deltaTime);
    }
}
