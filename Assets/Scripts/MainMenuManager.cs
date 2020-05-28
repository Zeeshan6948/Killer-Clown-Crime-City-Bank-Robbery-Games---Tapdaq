    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Analytics;
using Firebase.Messaging;

public enum GameModes
{
    Dance, Action
}


public class MainMenuManager : MonoBehaviour
{
    GameModes Mymode;
    public GameObject[] m_Panels;
    public int m_CurrentPanel;
    public List<int> PanelFlow;
    public Text CoinsText;
    public Text StarText;
    public Button MainMusicBtn;
    public Button MainSoundBtn;
    public Button SettingMusicBtn;
    public Button SettingSoundBtn;
    public AudioSource MusicSource;
    public AudioSource SoundSource;
    public AudioSource BtnClickSound;
    public GameObject BarProfBtn;
    public GameObject BarBackBtn;
    [Header("Character Details")]
    public GameObject Char1;
    public GameObject Char2;
    [Header("Bullet Details")]
    public Text BulletCount;
    [Header("Gun Details")]
    public GameObject[] Guns;
    public int CurrentGun;
    [Header("Health Details")]
    public Text HealthText;
    [Header("Loading Details")]
    public Text LoadingHealthText;
    public Text LoadingBulletText;
    public GameObject[] LoadingGuns;
    public Image LoadingScreen;
    public GameObject LoadingData;
    public Sprite[] LoadingSprites;

    void Start()
    {
        AudioListener.volume = 1;
        m_CurrentPanel = 1;
        OpenSpecficPanel(1);
        BarValueUpdate();
        LoadingScreen.sprite = LoadingSprites[0];
        if (PlayerPrefs.GetInt("Char1Buyed") == 1)
        {
            Char1.transform.GetChild(0).gameObject.SetActive(true);
            Char1.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Char2Buyed") == 1)
        {
            Char2.transform.GetChild(0).gameObject.SetActive(true);
            Char2.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("AssetsBtn") == 1)
        {
            OpenSpecficPanel(3);
        }
        if (AdsManager.Instance)
        {
            AdsManager.Instance.TapdaqBannerShow();
            FirebaseAnalytics.LogEvent("GameOpened");
            FirebaseMessaging.TokenReceived += OnTokenReceived;
            FirebaseMessaging.MessageReceived += OnMessageReceived;
        }
            
        BarValueUpdate();
    }
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
    void Update()
    {

    }

    public void BackBtnFun()
    {
        int temp = PanelFlow.IndexOf(m_CurrentPanel);
        OpenSpecficPanel(PanelFlow[temp - 1]);
        BarValueUpdate();
        BtnClickSound.Play();
        if (m_CurrentPanel == 1)
        {
            PanelFlow.Clear();
            PanelFlow.Add(1);
        }
    }

    void OpenSpecficPanel(int number)
    {
        m_Panels[m_CurrentPanel].SetActive(false);
        m_CurrentPanel = number;
        m_Panels[m_CurrentPanel].SetActive(true);
        PanelFlow.Add(number);
        if (m_CurrentPanel == 1)
        {
            BarProfBtn.SetActive(true);
            BarBackBtn.SetActive(false);
        }
        else
        {
            BarProfBtn.SetActive(false);
            BarBackBtn.SetActive(true);
        }
        if (AdsManager.Instance && m_CurrentPanel!=2 &&  m_CurrentPanel != 1)
            AdsManager.Instance.MediationAd();
    }

    public void MainMenuSinglePlayer(int CharNumber)
    {
        BtnClickSound.Play();
        BarValueUpdate();
        if (CharNumber == 0)
        {
            OpenSpecficPanel(2);
            PlayerPrefs.SetInt("SelectedClothes", 0);
        }
        if (CharNumber == 1)
        {
            if (PlayerPrefs.GetInt("Char1Buyed") == 0)
            {
                if (PlayerPrefs.GetInt("TotalReward") >= 15000)
                {
                    OpenSpecficPanel(2);
                    PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 15000);
                    PlayerPrefs.SetInt("Char1Buyed", 1);
                    PlayerPrefs.SetInt("SelectedClothes", 1);
                    Char1.transform.GetChild(0).gameObject.SetActive(true);
                    Char1.transform.GetChild(1).gameObject.SetActive(false);
                    BarValueUpdate();
                }
            }
            else
            {
                OpenSpecficPanel(2);
                PlayerPrefs.SetInt("SelectedClothes", 1);
            }
        }
        if (CharNumber == 2)
        {
            if (PlayerPrefs.GetInt("Char2Buyed") == 0)
            {
                if (PlayerPrefs.GetInt("TotalReward") >= 30000)
                {
                    OpenSpecficPanel(2);
                    PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 30000);
                    PlayerPrefs.SetInt("Char2Buyed", 1);
                    PlayerPrefs.SetInt("SelectedClothes", 2);
                    Char2.transform.GetChild(0).gameObject.SetActive(true);
                    Char2.transform.GetChild(1).gameObject.SetActive(false);
                    BarValueUpdate();
                }
            }
            else
            {
                OpenSpecficPanel(2);
                PlayerPrefs.SetInt("SelectedClothes", 2);
            }
        }
    }
    public void MainMenuWeapon()
    {
        OpenSpecficPanel(2);
        BtnClickSound.Play();
    }

    public void MainMenuShare()
    {
        BtnClickSound.Play();
    }

    public void MainMenuMusic()
    {
        if (MusicSource.volume == 0)
        {
            MainMusicBtn.GetComponent<Image>().sprite = MainMusicBtn.spriteState.pressedSprite;
            SettingMusicBtn.GetComponent<Image>().sprite = SettingMusicBtn.spriteState.pressedSprite;
            MusicSource.volume = 1;
        }
        else
        {
            MainMusicBtn.GetComponent<Image>().sprite = MainMusicBtn.spriteState.disabledSprite;
            SettingMusicBtn.GetComponent<Image>().sprite = SettingMusicBtn.spriteState.disabledSprite;
            MusicSource.volume = 0;
        }
        BtnClickSound.Play();
    }

    public void MainMenuSound()
    {
        if (SoundSource.volume == 0)
        {
            MainSoundBtn.GetComponent<Image>().sprite = MainSoundBtn.spriteState.pressedSprite;
            SettingSoundBtn.GetComponent<Image>().sprite = SettingSoundBtn.spriteState.pressedSprite;
            SoundSource.volume = 1;
        }
        else
        {
            MainSoundBtn.GetComponent<Image>().sprite = MainSoundBtn.spriteState.disabledSprite;
            SettingSoundBtn.GetComponent<Image>().sprite = SettingSoundBtn.spriteState.disabledSprite;
            SoundSource.volume = 0;
        }
        BtnClickSound.Play();
    }
    public void MainMenuRateUS()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        BtnClickSound.Play();
    }

    public void BarProfileBtn()
    {
        BtnClickSound.Play();
    }

    public void BarMoreGamesBtn()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Tap+To+Action");
        BtnClickSound.Play();
    }

    public void BarFreeGift()
    {
        BtnClickSound.Play();
        if (AdsManager.Instance)
        {
            AdsManager.Instance.functioncalling(this.gameObject, "RewardofFree");
            AdsManager.Instance.TappedReward();
        }
    }

    public void BarFreeCoins()
    {
        BtnClickSound.Play();
        if (AdsManager.Instance)
        {
            AdsManager.Instance.functioncalling(this.gameObject, "RewardofFree");
            AdsManager.Instance.TappedReward();
        }
    }
    public void RewardofFree()
    {
        Debug.Log("Reward Got");
        PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + 500);
        BarValueUpdate();
    }
    void BarValueUpdate()
    {
        CoinsText.text = PlayerPrefs.GetInt("TotalReward").ToString();
        StarText.text = PlayerPrefs.GetInt("TotalStars").ToString();
    }

    public void BarSetting()
    {
        OpenSpecficPanel(4);
        BtnClickSound.Play();
    }

    public void BarStore()
    {
        BtnClickSound.Play();

    }

    float WeaponPrice;
    GameObject MyCar;
    public void CarSelection(GameObject CarButton)
    {
        MyCar = CarButton;
        m_WeaponBtnClicked = CarButton;
        BtnClickSound.Play();
    }
    GameObject m_WeaponBtnClicked;
    public void CarSelectPurchase()
    {
        if (PlayerPrefs.GetInt("AssetsBtn") == 1)
        {
            OpenSpecficPanel(6);
            PlayerPrefs.SetInt("AssetsBtn", 0);
            return;
        }
        BtnClickSound.Play();
        LoadingBulletText.text = PlayerPrefs.GetInt("BulletHave").ToString();
        LoadingHealthText.text = PlayerPrefs.GetInt("HealthHave").ToString() + "% HEALTH";
        if (PlayerPrefs.GetInt("Gun0") == 1)
        {
            LoadingGuns[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("Gun1") == 1)
        {
            LoadingGuns[1].SetActive(true);
        }
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        
    }
    public void LevelSelected(int number)
    {
        OpenSpecficPanel(2);
        //CarSelection(FirstWeaponButon);
        PlayerPrefs.SetInt("LevelSelected", number);
        BtnClickSound.Play();
    }
    public void PrivacyPolicyBtn()
    {
        BtnClickSound.Play();
        Application.OpenURL("https://taptoaction.wixsite.com/privacypolicy");
    }
    public void RateUSBtn()
    {
        BtnClickSound.Play();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void ReviewbtnFun()
    {
        BtnClickSound.Play();
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void BulletBtnFun()
    {
        BtnClickSound.Play();
        if (PlayerPrefs.GetInt("TotalReward") >= 5000)
        {
            PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 5000);
            PlayerPrefs.SetInt("BulletHave", PlayerPrefs.GetInt("BulletHave") + 50);
            BulletCount.text = PlayerPrefs.GetInt("BulletHave").ToString();
            BarValueUpdate();
        }
    }

    public void GunNextBtnFun()
    {
        BtnClickSound.Play();
        CurrentGun++;
        if (CurrentGun == 2)
        {
            CurrentGun = 0;
        }
        gunstatus(CurrentGun);
    }

    public void GunPreBtnFun()
    {
        BtnClickSound.Play();
        CurrentGun--;
        if (CurrentGun == -1)
        {
            CurrentGun = 1;
        }
        gunstatus(CurrentGun);
    }

    void gunstatus(int value)
    {
        foreach (GameObject ob in Guns)
        {
            ob.SetActive(false);
        }
        Guns[value].SetActive(true);
        if (PlayerPrefs.GetInt("Gun" + value) == 0)
        {
            Guns[value].transform.GetChild(0).gameObject.SetActive(true);
            Guns[value].transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Guns[value].transform.GetChild(0).gameObject.SetActive(false);
            Guns[value].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void GunBtnFun()
    {
        BtnClickSound.Play();
        if (CurrentGun == 0)
        {
            if (PlayerPrefs.GetInt("TotalReward") >= 15000)
            {
                PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 15000);
                PlayerPrefs.SetInt("Gun" + CurrentGun, 1);
                gunstatus(0);
            }
        }
        if (CurrentGun == 1)
        {
            if (PlayerPrefs.GetInt("TotalReward") >= 25000)
            {
                PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 25000);
                PlayerPrefs.SetInt("Gun" + CurrentGun, 1);
                gunstatus(1);
            }
        }
        BarValueUpdate();
    }

    public void HealthBtnFun()
    {
        BtnClickSound.Play();
        if (PlayerPrefs.GetInt("TotalReward") >= 5000)
        {
            PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") - 5000);
            PlayerPrefs.SetInt("HealthHave", PlayerPrefs.GetInt("HealthHave") + 50);
            HealthText.text = PlayerPrefs.GetInt("HealthHave").ToString() + "% HEALTH";
            BarValueUpdate();
        }
    }

    public void ModeDanceBtn()
    {
        BarValueUpdate();
        PlayerPrefs.SetInt("Dance", 0);
        OpenSpecficPanel(5);
        LoadingData.SetActive(false);
        LoadingScreen.sprite = LoadingSprites[1];
        Mymode = GameModes.Dance;
        BtnClickSound.Play();
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("DanceModeSelection");
        }
    }

    public void ModeActionBtn()
    {
        BarValueUpdate();
        PlayerPrefs.SetInt("Dance", 1);
        LoadingData.SetActive(true);
        LoadingScreen.sprite = LoadingSprites[0];
        OpenSpecficPanel(3);
        Mymode = GameModes.Action;
        BtnClickSound.Play();
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("ActionModeSelection");
        }
    }

    public void LoadingOptionsDance()
    {
        LoadingData.SetActive(false);
        LoadingScreen.sprite = LoadingSprites[1];
        Mymode = GameModes.Dance;
        PlayerPrefs.SetInt("Dance", 0);
        PlayerPrefs.SetInt("PositonSaved",0);
        BtnClickSound.Play();
        LoadingBulletText.text = PlayerPrefs.GetInt("BulletHave").ToString();
        LoadingHealthText.text = PlayerPrefs.GetInt("HealthHave").ToString() + "% HEALTH";
        if (PlayerPrefs.GetInt("Gun0") == 1)
        {
            LoadingGuns[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("Gun1") == 1)
        {
            LoadingGuns[1].SetActive(true);
        }
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("LoadingDanceModeSelection");
        }
    }

    public void LoadingOptionsAction()
    {
        LoadingData.SetActive(true);
        LoadingScreen.sprite = LoadingSprites[0];
        Mymode = GameModes.Action;
        PlayerPrefs.SetInt("Dance", 1);
        BtnClickSound.Play();
        LoadingBulletText.text = PlayerPrefs.GetInt("BulletHave").ToString();
        LoadingHealthText.text = PlayerPrefs.GetInt("HealthHave").ToString() + "% HEALTH";
        if (PlayerPrefs.GetInt("Gun0") == 1)
        {
            LoadingGuns[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("Gun1") == 1)
        {
            LoadingGuns[1].SetActive(true);
        }
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("LoadingActionModeSelection");
        }
    }
}
