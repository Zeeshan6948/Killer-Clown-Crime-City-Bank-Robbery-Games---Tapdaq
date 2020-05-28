    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Analytics;
public class Level1 : MonoBehaviour
{
    public GameObject Van;
    public GameObject[] jokerMen;
    public string[] Statements;
    public GameObject InVan;
    public Transform VanPos;
    public Transform VanRunPos;
    public GameObject VanDor;
    public GameObject Door;
    public GameObject[] Chasing;
    public Transform[] ArrowPoint;
    public RotateAround CarArrow;
    int InstNumber=1;
    // Start is called before the first frame update
    void Start()
    {
        //LevelManager.m_instance.Arrow.SetActive(false);
        LevelManager.m_instance.CanvasObject.MessngerText.text = Statements[0];
        LevelManager.m_instance.Arrow.GetComponent<RotateAround>().Target = ArrowPoint[0];
        LevelManager.m_instance.Messenger.SetActive(true);
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("RobberyMissionSelection");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartJourney()
    {
        foreach(GameObject ob in jokerMen)
        {
            if (ob != null)
            {
                ob.SetActive(false);
                ob.transform.parent = Van.transform;
            }
        }
        Van.GetComponent<RCC_CarControllerV3>().canControl = true;
        InVan.SetActive(true);
        InBankHandler.instance.StopAll();
    }

    public void GetOutOfCar()
    {
        Van.transform.position = VanPos.position;
        Van.transform.rotation = VanPos.rotation;
        Van.GetComponent<RCC_CarControllerV3>().brakeInput = 1;
        foreach (GameObject ob in jokerMen)
        {
            ob.transform.parent = Van.transform.parent;
            ob.GetComponent<AI>().standWatch = true;
            ob.GetComponent<AI>().walkOnPatrol = false;
            ob.SetActive(true);
        }
        Invoke("Wait",2f);
        Van.GetComponent<RCC_CarControllerV3>().KillEngine();
        InVan.SetActive(false);
    }

    void Wait()
    {
        foreach (GameObject ob in jokerMen)
        {
            ob.GetComponent<AI>().followPlayer = true;
        }
    }

    public void NextInstructions()
    {
        InstNumber++;
        print(InstNumber);
        if(ArrowPoint[InstNumber] != null)
            ArrowPoint[InstNumber].gameObject.SetActive(true);
        LevelManager.m_instance.Arrow.GetComponent<RotateAround>().Target = ArrowPoint[InstNumber];
        CarArrow.Target = ArrowPoint[InstNumber];
        print("chala");
        LevelManager.m_instance.CanvasObject.MessngerText.text = Statements[InstNumber];
        LevelManager.m_instance.Messenger.SetActive(true);
    }

    public void VanisReadytoRun()
    {
        InVan.SetActive(false);
        Van.transform.position = VanRunPos.position;
        Van.transform.rotation = VanRunPos.rotation;
        Van.GetComponent<Rigidbody>().isKinematic = false;
        Van.GetComponent<RCC_CarControllerV3>().externalController = false;
        //Van.GetComponent<RCC_CarControllerV3>().handbrakeInput = 1;
        Destroy(Van.GetComponent<RCC_AICarController>());
        Van.GetComponent<RCC_CarControllerV3>().SetCanControl(true);
        Van.GetComponent<RCC_CarControllerV3>().KillEngine();
        VanDor.SetActive(true);
        Door.SetActive(true);
    }

    public void ChasingStarted()
    {
        foreach (GameObject ob in Chasing)
        {
            ob.SetActive(true);
            if (ob.GetComponent<RCC_AICarController>())
            {
                ob.GetComponent<RCC_AICarController>().enabled = true;
            }
        }
    }
}
