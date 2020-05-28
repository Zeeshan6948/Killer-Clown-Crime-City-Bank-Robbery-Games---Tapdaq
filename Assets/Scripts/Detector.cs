using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTypes
{
    Final, Reached, CashObject, Mission1, DoorCheck, alarm, vault, safty, Dance
}

public class Detector : MonoBehaviour
{
    public ObjectTypes ThisObjectIs;
    public AudioSource Alarm;
    bool disablework=false;
    public GameObject VaultDoor;
    public int PositionNo;
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
        if (other.transform.tag == "PlayerCar" && ThisObjectIs == ObjectTypes.Final && !disablework) 
        {
            LevelManager.m_instance.CanvasObject.MessngerText.text = LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().Statements[1];
            LevelManager.m_instance.Messenger.SetActive(true);
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().GetOutOfCar();
            ThisObjectIs = ObjectTypes.Reached;
            disablework = true;
            Invoke("wait", 1);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Reached && !disablework)
        {
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.safty && !disablework)
        {
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
            LevelManager.m_instance.GetComponent<ControlConverter>().Convert = true;
            LevelManager.m_instance.m_level[0].LevelObject.SetActive(false);
            PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + 10000);
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<MarkerController>().Reactive();
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.vault && !disablework)
        {
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().VanisReadytoRun();
            VaultDoor.GetComponent<Animator>().enabled = true;
            //LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().ChasingStarted();
            //LevelManager.m_instance.m_level[0].LevelObject.GetComponent<MarkerController>().ChasingStart();
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.CashObject)
        {
            LevelManager.m_instance.IncreaseCash(50);
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Mission1 && !disablework)
        {
            LevelManager.m_instance.CanvasObject.ActivateMission("In this mission you will perform robbry with gangsters", 0,this.gameObject);
            disablework = true;
            Invoke("wait", 6);
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.DoorCheck)
        {
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<MarkerController>().MapMarkerTurnOff();
            NotificationGenrator.m_instance.CurrentNumber = 2;
            NotificationGenrator.m_instance.NotifyUser();
        }
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.alarm)
        {
            Alarm.Stop();
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().NextInstructions();
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Dance)
        {
            LevelManager.m_instance.DanceScript.TheNumber = -1;
            LevelManager.m_instance.DanceScript.RightSpot = false;
            LevelManager.m_instance.DanceScript.DanceBtn.interactable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && ThisObjectIs == ObjectTypes.Dance)
        {
            LevelManager.m_instance.DanceScript.TheNumber = PositionNo;
            LevelManager.m_instance.DanceScript.RightSpot = true;
            LevelManager.m_instance.DanceScript.DanceBtn.interactable = true;
            LevelManager.m_instance.DanceScript.DanceStart();
        }
    }
    void wait()
    {
        disablework = false;
    }
}
