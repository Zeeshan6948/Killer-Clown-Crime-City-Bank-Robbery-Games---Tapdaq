using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerAnimControl : MonoBehaviour
{
    public GameObject Shooting;
    public GameObject Driving;
    bool NormalAgain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NormalAgain)
        {
            StartDriving();
        }
    }

    public void StartDriving()
    {
        Driving.SetActive(true);
        Shooting.SetActive(false);
        NormalAgain = false;
    }

    public void StopDriving()
    {
        Driving.SetActive(false);
        Shooting.SetActive(false);
    }

    public void StartShooting()
    {
        if(IsInvoking("ReturnBack"))
            CancelInvoke("ReturnBack");
        Shooting.SetActive(true);
        Driving.SetActive(false);
        NormalAgain = false;
        Invoke("ReturnBack", 3);
    }
    void ReturnBack()
    {
        NormalAgain = true;
        CancelInvoke("ReturnBack");
    }
}
