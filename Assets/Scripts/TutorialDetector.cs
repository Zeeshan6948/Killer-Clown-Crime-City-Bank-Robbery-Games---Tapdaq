using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckPoints
{
    next, CarDoor, CarExit, Aim
}

public class TutorialDetector : MonoBehaviour
{
    public CheckPoints ThisObjectIs;
    bool disablework=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.transform.tag == "PlayerCar" && ThisObjectIs == ObjectTypes.Final && !disablework) 
        //{
        //    LevelManager.m_instance.CanvasObject.MessngerText.text = LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().Statements[1];
        //    LevelManager.m_instance.Messenger.SetActive(true);
        //    LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().GetOutOfCar();
        //    ThisObjectIs = ObjectTypes.Reached;
        //    disablework = true;
        //    Invoke("wait", 1);
        //}
        //if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Reached && !disablework)
        //{
        //    LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
        //    Destroy(this.gameObject);
        //}
        //if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.safty && !disablework)
        //{
        //    LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
        //    LevelManager.m_instance.GetComponent<ControlConverter>().Convert = true;
        //    LevelManager.m_instance.m_level[0].LevelObject.SetActive(false);
        //    PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + 10000);
        //    Destroy(this.gameObject);
        //}
        //if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.vault && !disablework)
        //{
        //    LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
        //    LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().VanisReadytoRun();
        //    VaultDoor.GetComponent<Animator>().enabled = true;
        //    Destroy(this.gameObject);
        //}
        //if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.CashObject)
        //{
        //    LevelManager.m_instance.IncreaseCash(50);
        //    Destroy(this.gameObject);
        //}
        //if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Mission1 && !disablework)
        //{
        //    LevelManager.m_instance.CanvasObject.ActivateMission("The mission is to Rob a Bank with your Team",0,this.gameObject);
        //    disablework = true;
        //    Invoke("wait", 6);
        //}
        if (other.transform.tag == "Player" && ThisObjectIs == CheckPoints.next)
        {
            LevelManager.m_instance.TutorialScript.StatementSystem();
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == CheckPoints.CarDoor)
        {
            LevelManager.m_instance.TutorialScript.StatementSystem();
            LevelManager.m_instance.TutorialScript.CarbtnProminent.SetActive(true);
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "PlayerCar" && ThisObjectIs == CheckPoints.CarExit)
        {
            LevelManager.m_instance.TutorialScript.StatementSystem();
            LevelManager.m_instance.TutorialScript.CarExirProminent.SetActive(true);
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == CheckPoints.Aim)
        {
            LevelManager.m_instance.TutorialScript.StatementSystem();
            LevelManager.m_instance.TutorialScript.AimProminent.SetActive(true);
            LevelManager.m_instance.TutorialScript.AfteraTime();
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
    }
    void wait()
    {
        disablework = false;
    }
}
