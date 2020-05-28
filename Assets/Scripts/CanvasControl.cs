using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Analytics;

public class CanvasControl : MonoBehaviour
{
    public GameObject[] m_Panels;
    bool Paused;
    public Text TimerText;
    public int m_CurrentPanel;
    public Text MessngerText;
    public Image HealthBar;
    public Image HealthBar1;
    public GameObject Stars;
    public GameObject PlentyPanel;
    public Text Plentytext;
    public Text PlentyTotalReward;
    public Text TotalReward;
    int plentyprice;
    public ControlConverter ScriptObj;
    public GameObject MissionPanel;
    public Text MissionText;
    GameObject CallerObj;
    int MissionNumber;
    public Text ChaseText;
    public GameObject UseBtn;
    public Image GunImage;
    public Sprite[] GunsImages;
    FPSPlayer PlayerFpsScript;
    void Start()
    {

    }

    void ReferenceAssign()
    {
        PlayerFpsScript = ScriptObj.PlayerObjects[0].GetComponent<FPSPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        TotalReward.text = PlayerPrefs.GetInt("TotalReward").ToString();
        ChaseText.text = LevelManager.m_instance.ChasingNumber.ToString();
        if (PlayerFpsScript != null)
        {
            if(PlayerFpsScript.PlayerWeaponsComponent.currentWeapon == 5)
            {
                GunImage.sprite = GunsImages[1];
            }
            else if (PlayerFpsScript.PlayerWeaponsComponent.currentWeapon == 6)
            {
                GunImage.sprite = GunsImages[2];
            }
            else
            {
                GunImage.sprite = GunsImages[0];
            }
        }
        else
        {
            GunImage.sprite = GunsImages[0];
            ReferenceAssign();
        }
    }

    public void OpenSpecficPanel(int number)
    {
        m_Panels[m_CurrentPanel].SetActive(false);
        m_CurrentPanel = number;
        m_Panels[m_CurrentPanel].SetActive(true);
    }

    public void PausedBtnFun()
    {
        Paused = !Paused;
        if (Paused)
        {
            OpenSpecficPanel(1);
            Time.timeScale = 0;
            AudioListener.volume = 0;
            string SendData = "Level_Paused_Level_Number" + LevelManager.m_currentLevel.ToString();
            LevelManager.m_instance.FPSCanvas.SetActive(false);
            PlayerPosition();
            if (AdsManager.Instance)
            {
                AdsManager.Instance.TapdaqBannerHide();
                FirebaseAnalytics.LogEvent(SendData);
            }
        }
        else
        {
            OpenSpecficPanel(0);
            Time.timeScale = 1;
            AudioListener.volume = 1;
            LevelManager.m_instance.FPSCanvas.SetActive(true);
            if (AdsManager.Instance)
            {
                AdsManager.Instance.TapdaqBannerShow();
            }
        }
    }

    public void ReloadBtnFun()
    {
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }

    public void NextBtnFun()
    {
        PlayerPrefs.SetInt("LevelSelected", PlayerPrefs.GetInt("LevelSelected") + 1);
        if (PlayerPrefs.GetInt("LevelSelected") == LevelManager.m_instance.m_level.Length)
        {
            AudioListener.volume = 1;
            bl_SceneLoaderUtils.GetLoader.LoadLevel("Main");
            Time.timeScale = 1;
        }
        else
        {
            AudioListener.volume = 1;
            bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
            Time.timeScale = 1;
        }
    }

    public void HomeBtnFun()
    {
        AudioListener.volume = 1;
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Main");
        Time.timeScale = 1;

    }

    public void AssetsBtnFun()
    {
        PlayerPosition();
        AudioListener.volume = 1;
        PlayerPrefs.SetInt("AssetsBtn", 1);
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Main");
        Time.timeScale = 1;
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("Dance", "Assets", (double)(35000));
        }
    }

    public void WatchAdBtn()
    {
        if (AdsManager.Instance)
        {
            AdsManager.Instance.functioncalling(this.gameObject, "ReviveReturn");
            AdsManager.Instance.TappedReward();
        }
    }

    public void ReviveReturn()
    {
        //PlayerPrefs.SetInt("DelObjects", LevelManager.m_Instance.targeteliminated);
        //bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        GameObject.FindObjectOfType<FPSPlayer>().hitPoints = 100;
        LevelManager.m_instance.FPSCanvas.SetActive(true);
        OpenSpecficPanel(0);
        GameObject.FindObjectOfType<SmoothMouseLook>().enabled = true;
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }

    public void PriceReviveReturn()
    {
        if (PlayerPrefs.GetInt("TotalReward") > 1000)
        {
            PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 1000);
            //PlayerPrefs.SetInt("DelObjects", LevelManager.m_Instance.targeteliminated);
            //bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
            GameObject.FindObjectOfType<FPSPlayer>().hitPoints = 100;
            LevelManager.m_instance.FPSCanvas.SetActive(true);
            OpenSpecficPanel(0);
            GameObject.FindObjectOfType<SmoothMouseLook>().enabled = true;
            Time.timeScale = 1;
            AudioListener.volume = 1;
        }
        else
        {
            LevelManager.m_instance.LevelFailed();
        }
    }

    public void ActivateStars(int number)
    {
        for(int i= 0; i <= number; i++)
        {
            Stars.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }

    public void ActivatePlentyPanel(string TextPrint,int plentyamount)
    {
        PlentyTotalReward.text = PlayerPrefs.GetInt("TotalReward").ToString();
        for (int i = 0; i <= 5; i++)
        {
            Stars.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
        }
        PlentyPanel.SetActive(true);
        plentyprice = plentyamount;
        Plentytext.text = TextPrint;
    }

    public void PlentyPay()
    {
        if (PlayerPrefs.GetInt("TotalReward") >= plentyprice)
        {
            PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - plentyprice);
            PlayerPrefs.SetInt("PlayerdiedinMission",0);
            ResetPlayer();
        }
    }
    public void PlentyWatchAd()
    {
        if (AdsManager.Instance)
        {
            AdsManager.Instance.functioncalling(this.gameObject, "ResetPlayer");
            AdsManager.Instance.TappedReward();
        }
    }

    public void ResetPlayer()
    {
        PlentyPanel.SetActive(false);
        ScriptObj.PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints = 100;
        GameObject.FindObjectOfType<SmoothMouseLook>().enabled = true;
    }
    public void ActivateMission(string missionvalue, int number, GameObject Caller)
    {
        MissionPanel.SetActive(true);
        MissionText.text = missionvalue;
        MissionNumber = number;
        CallerObj = Caller;
    }

    public void MissionAccept()
    {
        LevelManager.m_instance.m_level[MissionNumber].LevelObject.SetActive(true);
        LevelManager.m_instance.StartTimer();
        Destroy(CallerObj);
        MissionPanel.SetActive(false);
        NotificationGenrator.m_instance.CurrentNumber = 1;
        NotificationGenrator.m_instance.NotifyUser();
    }

    public void MissionReject()
    {
        MissionPanel.SetActive(false);
    }

    public void DanceModeActivationBtn()
    {
        PlayerPrefs.SetInt("PositonSaved", 0);
        PlayerPrefs.SetInt("Dance", 0);
        PlayerPrefs.SetInt("PlayerdiedinMission", 0);
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        Time.timeScale = 1;
        AudioListener.volume = 1;
    }

    public void PlayerPosition()
    {
        Transform Record = LevelManager.m_instance.GetComponent<ControlConverter>().PlayerObjects[0].transform;
        PlayerPrefs.SetInt("PositonSaved", 1);
        PlayerPrefs.SetFloat("PositonSavedx", Record.position.x);
        PlayerPrefs.SetFloat("PositonSavedy", Record.position.y);
        PlayerPrefs.SetFloat("PositonSavedz", Record.position.z);
    }

    public void UseBtnForCar()
    {
        UseBtn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,0,50);
    }

    public void UseBtnBackOrigin()
    {
        UseBtn.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(587.5f, -83.5f, 50.0f);
    }
}