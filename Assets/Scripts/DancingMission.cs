using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Analytics;
using UnityEngine.EventSystems;

public class DancingMission : MonoBehaviour
{
    public GameObject a_NormalGUI;
    public GameObject a_Normal2GUI;
    public GameObject a_DanceGUI;
    public GameObject a_FightingGUI;
    public Transform[] a_DanceLocations;
    public string a_MissionDescirption;
    public GameObject CameraDance;
    public Transform[] CameraPosition;
    public GameObject[] DanceCrowdObject;
    public Button DanceBtn;
    public int TotalCrowdNow;
    public int MaximumCrowd;
    public bool AllowReward = false;
    public bool RightSpot;
    public int TheNumber;
    public Animator PlayerAnim;
    public RuntimeAnimatorController PlayerController;
    public RuntimeAnimatorController DanceController;
    public Slider AnimationSlider;
    public AudioSource DancePlayer;
    public AudioClip[] Sounds;
    public GameObject DanceReward;
    public Text DanceRewardText;
    int CurrentNumber = -1;
    int currentSlider;
    GameObject CurrentPositonMarker;
    public GameObject InformationIcon;
    public GameObject AssetsBuyPanel;
    double levelrecord;
    [Header("New 2 Jokers")]
    public Transform jokerPos1;
    public Transform jokerPos2;
    public Animator NewJoker1;
    public Animator NewJoker2;
    public Texture[] PlayerClothes;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.m_instance.CanvasObject.MessngerText.text = a_MissionDescirption;
        LevelManager.m_instance.Messenger.SetActive(true);
        LevelManager.m_instance.Arrow.SetActive(true);
        LevelManager.m_instance.Arrow.GetComponent<RotateAround>().Target = a_DanceLocations[0];
        Invoke("Afterasec", 0.1f);
        a_FightingGUI.SetActive(false);
        a_Normal2GUI.SetActive(false);
        if (AdsManager.Instance)
        {
            FirebaseAnalytics.LogEvent("DanceOpened");
        }
        if (PlayerPrefs.GetInt("Music4") == 1)
        {
            a_DanceGUI.transform.GetChild(6 + 4 + 1).GetChild(0).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Music5") == 1)
        {
            a_DanceGUI.transform.GetChild(6 + 5 + 1).GetChild(0).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Animation4") == 1)
        {
            a_DanceGUI.transform.GetChild(4 + 1).GetChild(0).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Animation5") == 1)
        {
            a_DanceGUI.transform.GetChild(5 + 1).GetChild(0).gameObject.SetActive(false);
        }
    }
    void Afterasec()
    {
        this.GetComponent<MarkerController>().DanceMapMarkerTurnOff();
        LevelManager.m_instance.CanvasObject.ResetPlayer();
        PlayerPrefs.SetInt("HealthHave", 100);
    }
    // Update is called once per frame
    void Update()
    {
        if (AllowReward)
        {
            AnimationSlider.value = PlayerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        }
    }


    void DancePositionSetter(int Value)
    {
        CurrentNumber = Value;
        AllowReward = true;
        LevelManager.m_instance.Arrow.SetActive(false);
        CurrentPositonMarker = a_DanceLocations[Value].gameObject;
        CurrentPositonMarker.SetActive(false);
    }

    public void DanceEnded()
    {
        CurrentNumber = -1;
        AllowReward = false;
        a_NormalGUI.SetActive(true);
        a_DanceGUI.SetActive(false);
        CameraDance.SetActive(false);
        PlayerAnim.GetComponent<PlayerCharacter>().enabled = true;
        PlayerAnim.runtimeAnimatorController = PlayerController;
        //PlayerAnim.applyRootMotion = false;
        NearByGenerate.Instance.DeleteAllCrowd();
        a_FightingGUI.SetActive(true);
        a_Normal2GUI.SetActive(true);
        if (CurrentPositonMarker != null)
        {
            CurrentPositonMarker.SetActive(true);
            CurrentPositonMarker = null;
        }
    }

    public void DanceStart()
    {
        a_NormalGUI.SetActive(false);
        a_DanceGUI.SetActive(true);
        CameraDance.SetActive(true);
        PlayerAnim.GetComponent<PlayerCharacter>().enabled = false;
        NearByGenerate.Instance.CreateACrowd();
        PlayerAnim.runtimeAnimatorController = DanceController;
        //PlayerAnim.applyRootMotion = true;
        if (RightSpot)
        {
            DancePositionSetter(TheNumber);
        }
        PlayerAnim.transform.position = new Vector3(-182.122f,1.002076f,-91.027f);
        PlayerAnim.transform.localEulerAngles = new Vector3(0, 119.111f, 0);
        CreateAssign2Jokers();
        StartAnimation(0);
        currentSlider = 0;
        StartMusic(0);
        if (AdsManager.Instance)
        {
            levelrecord = CurrentNumber;
            FirebaseAnalytics.LogEvent("Dance", "Positon", levelrecord);
        }
    }
    bool Unlocked;
    public void StartAnimation(int Number)
    {
        if((Number == 4 || Number == 5) && !Unlocked)
        {
            EventSystem.current.SetSelectedGameObject(null);
            CheckifIOpened(Number);
            return;
        }
        if(AnimationSlider.value > 0.3f)
        {
            AwardeReward((int)(AnimationSlider.value*5000));
        }
        Unlocked = false;
        AnimationSlider.value = 0;
        currentSlider = Number;
        PlayerAnim.SetInteger("DanceNo", Number);
        PlayerAnim.SetTrigger("Dance");
        NewJoker1.SetInteger("DanceNo", Number);
        NewJoker1.SetTrigger("Dance");
        NewJoker2.SetInteger("DanceNo", Number);
        NewJoker2.SetTrigger("Dance");
        a_DanceGUI.transform.GetChild(Number+1).GetComponent<Button>().Select();
        //if (AdsManager.Instance)
        //{
        //    levelrecord = Number;
        //    FirebaseAnalytics.LogEvent("Dance", "Animation", levelrecord);
        //}
    }
    bool Unlocked2;
    public void StartMusic(int Number)
    {
        if ((Number == 4 || Number == 5) && !Unlocked2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            CheckifIOpened2(Number);
            return;
        }
        Unlocked2 = false;
        a_DanceGUI.transform.GetChild(Number + 6 + 1).GetComponent<Button>().Select();
        DancePlayer.clip = Sounds[Number];
        DancePlayer.Play();
    }

    public void AwardeReward(int Amount)
    {
        DanceReward.SetActive(true);
        DanceRewardText.text = Amount.ToString();
        Invoke("AfterAnimation", 1.2f);
    }

    void AfterAnimation()
    {
        DanceReward.SetActive(false);
        PlayerPrefs.SetInt("TotalReward", PlayerPrefs.GetInt("TotalReward") + int.Parse(DanceRewardText.text));
        if(PlayerPrefs.GetInt("TotalReward") >= 35000)
        {
            InformationFresher();
        }
    }

    public void InformationFresher()
    {
        PlayerPrefs.SetInt("Dance", 1);
        InformationIcon.GetComponent<GrowAndShrink>().enabled = true;
        if (AdsManager.Instance)
        {
            levelrecord = 35000;
            FirebaseAnalytics.LogEvent("Dance", "CompleteReward", levelrecord);
        }
    }

    public void AssetsBuyingPromotion()
    {
        AssetsBuyPanel.SetActive(true);
    }

    void CreateAssign2Jokers()
    {
        GameObject ob = Instantiate(PlayerAnim.gameObject, jokerPos1.position, jokerPos1.rotation);
        NewJoker1 = ob.GetComponent<Animator>();
        GameObject ob2 = Instantiate(PlayerAnim.gameObject, jokerPos2.position, jokerPos2.rotation);
        NewJoker2 = ob2.GetComponent<Animator>();
        if (PlayerPrefs.GetInt("SelectedClothes") == 0)
        {
            ob.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[1];
            ob2.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[2];
        }
        if (PlayerPrefs.GetInt("SelectedClothes") == 1)
        {
            ob.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[0];
            ob2.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[2];
        }
        if (PlayerPrefs.GetInt("SelectedClothes") == 2)
        {
            ob.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[0];
            ob2.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = PlayerClothes[1];
        }
    }

    int TempraryNumber;
    void CheckifIOpened(int Value)
    {
        if(PlayerPrefs.GetInt("Animation"+ Value) == 0)
        {
            if(AdsManager.Instance)
            {
                TempraryNumber = Value;
                AdsManager.Instance.functioncalling(this.gameObject, "RewardforWatch");
                AdsManager.Instance.TappedReward();
            }
        }
        else
        {
            Unlocked = true;
            StartAnimation(Value);
        }
    }

    public void RewardforWatch()
    {
        PlayerPrefs.SetInt("Animation" + TempraryNumber,1);
        a_DanceGUI.transform.GetChild(TempraryNumber + 1).GetChild(0).gameObject.SetActive(false);
    }

    int TempraryNumber2;
    void CheckifIOpened2(int Value)
    {
        if (PlayerPrefs.GetInt("Music" + Value) == 0)
        {
            if (AdsManager.Instance)
            {
                TempraryNumber2 = Value;
                AdsManager.Instance.functioncalling(this.gameObject, "RewardforWatch2");
                AdsManager.Instance.TappedReward();
            }
        }
        else
        {
            Unlocked2 = true;
            StartMusic(Value);
        }
    }

    public void RewardforWatch2()
    {
        PlayerPrefs.SetInt("Music" + TempraryNumber2, 1);
        a_DanceGUI.transform.GetChild(6 + TempraryNumber2 + 1).GetChild(0).gameObject.SetActive(false);
    }
}
