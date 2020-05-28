using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

[System.Serializable]
public class Level
{
    public Transform PlayerPos;
    public int LevelTime;
    public int LevelReward;
    public GameObject LevelObject;
}

[System.Serializable]
public class Plenty
{
    public string PlentyText;
    public int Plentycash;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager m_instance;
    public TuturialScenario TutorialScript;
    public DancingMission DanceScript;
    public Level[] m_level;
    public Plenty[] m_plenty;
    public ControlConverter m_Convertor;
    public GameObject currentCar;
    public GameObject PlayerObject;
    public static int m_currentLevel;
    public GameObject Messenger;
    public CanvasControl CanvasObject;
    public GameObject FPSCanvas;
    int waitnumber;
    public int Timer;
    double levelrecord;
    public int KilledBy;
    public Transform PoliceStation;
    public Transform Hospital;
    public GameObject CashObject;
    public GameObject DanceBtn;
    public GameObject Arrow;
    public Transform MissionMarker;
    public int ChasingNumber;
    void Start()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        m_currentLevel = PlayerPrefs.GetInt("LevelSelected");
        AudioListener.volume = 1;
        levelrecord = (float)m_currentLevel;
        ActivationofObjects();
        //if (PlayerPrefs.GetInt("tutorial") == 0)
        //{
        //    TutorialScript.gameObject.SetActive(true);
        //}
        if (PlayerPrefs.GetInt("Dance") == 0)
        {
            DanceScript.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Dance") == 1)
        {
            Arrow.SetActive(true);
            Arrow.GetComponent<RotateAround>().Target = MissionMarker;
        }
        if (AdsManager.Instance)
        {
            AdsManager.Instance.TapdaqBannerShow();
            FirebaseAnalytics.LogEvent("Level", "LevelOpened", levelrecord);
        }
    }
    public void ActivationofObjects()
    {
        //PlayerObject.transform.position = m_level[m_currentLevel].PlayerPos.position;
        //PlayerObject.transform.rotation = m_level[m_currentLevel].PlayerPos.rotation;
        PlayerObject.SetActive(true);
        if(PlayerPrefs.GetInt("PositonSaved") == 1)
        {
            Vector3 pos = new Vector3(PlayerPrefs.GetFloat("PositonSavedx"), PlayerPrefs.GetFloat("PositonSavedy"), PlayerPrefs.GetFloat("PositonSavedz"));
            m_Convertor.PlayerObjects[0].transform.position = pos;
        }
        if (PlayerPrefs.GetInt("PlayerdiedinMission") == 1)
        {
            KilledBy = 0;
            this.GetComponent<ControlConverter>().PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints = 0;
            ReactivePlayer();
        }
        if (PlayerPrefs.GetInt("PlayerdiedinMission") == 2)
        {
            KilledBy = 1;
            this.GetComponent<ControlConverter>().PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints = 0;
            ReactivePlayer();
        }
        //m_level[m_currentLevel].LevelObject.SetActive(true);
        //Timer = m_level[m_currentLevel].LevelTime;
        //InvokeRepeating("TickTick", 1, 1);
    }
    public void StartTimer()
    {
        CanvasObject.TimerText.transform.parent.gameObject.SetActive(true);
        Timer = m_level[m_currentLevel].LevelTime;
        InvokeRepeating("TickTick", 1, 1);
    }
    void TickTick()
    {
        Timer--;
        CanvasObject.TimerText.text = (Timer / 60).ToString() + ":" + (Timer % 60).ToString();
        if (Timer == 0)
        {
            LevelFailed();
            CancelInvoke("TickTick");
        }
    }

    public void LevelFailed()
    {
        if (waitnumber != 1)
        {
            Invoke("GeneralWait", 0);
            FPSCanvas.SetActive(false);
            waitnumber = 1;
        }
        else
        {
            Invoke("GeneralWait", 0);
            waitnumber = 2;
            FPSCanvas.SetActive(false);
            AudioListener.volume = 0;
        }
    }

    public void LevelCompleted()
    {
        print("completed");
        if (PlayerPrefs.GetInt("LevelCompleted") <= PlayerPrefs.GetInt("LevelSelected"))
            PlayerPrefs.SetInt("LevelCompleted", PlayerPrefs.GetInt("LevelSelected") + 1);
        PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + m_level[m_currentLevel].LevelReward);
        Invoke("GeneralWait", 2);
        waitnumber = 0;
        FPSCanvas.SetActive(false);
        AudioListener.volume = 0;
    }

    public void GeneralWait()
    {
        switch (waitnumber)
        {
            case 0:
                CanvasObject.OpenSpecficPanel(2);
                if (AdsManager.Instance)
                {
                    AdsManager.Instance.TapdaqBannerHide();
                    AdsManager.Instance.MediationAd();
                    FirebaseAnalytics.LogEvent("Level", "LevelCompleted", levelrecord);
                }
                break;
            case 1:
                CanvasObject.OpenSpecficPanel(5);
                break;
            case 2:
                CanvasObject.OpenSpecficPanel(3);
                if (AdsManager.Instance)
                {
                    AdsManager.Instance.TapdaqBannerHide();
                    AdsManager.Instance.MediationAd();
                    FirebaseAnalytics.LogEvent("Level", "LevelFailed", levelrecord);
                }
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void callingSepecialFunctions(int number)
    {
        switch (number)
        {
            case 0:
                m_level[0].LevelObject.GetComponent<Level1>().StartJourney();
                break;
        }
    }

    public void PlayerDied()
    {
        if(m_level[0].LevelObject.GetComponent<Level1>().enabled)
        {
            PlayerPrefs.SetInt("PlayerdiedinMission", KilledBy + 1);
            CanvasObject.ReloadBtnFun();
        }
            Invoke("ReactivePlayer", 4);
    }

    public void ReactivePlayer()
    {
        if (KilledBy == 0){
            NearByGenerate.Instance.DeleteAllPolice();
            this.GetComponent<ControlConverter>().PlayerObjects[0].transform.position = PoliceStation.position;
            CanvasObject.ActivatePlentyPanel(m_plenty[0].PlentyText, m_plenty[0].Plentycash);
        }
        if (KilledBy == 1)
        {
            NearByGenerate.Instance.DeleteAllPolice();
            this.GetComponent<ControlConverter>().PlayerObjects[0].transform.position = Hospital.position;
            CanvasObject.ActivatePlentyPanel(m_plenty[1].PlentyText, m_plenty[1].Plentycash);
        }
    }

    public void IncreaseCash(int number)
    {
        PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + number);
    }
}
