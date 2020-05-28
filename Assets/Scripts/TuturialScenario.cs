using UnityEngine;
using UnityEngine.UI;

public class TuturialScenario : MonoBehaviour
{
    public ControlConverter CovertScript;
    public Transform PlayerPostion;
    public GameObject PlayerObject;
    public string[] Statement;
    public GameObject CamTuturial;
    public bool CamStart;
    public GameObject CarbtnProminent;
    public GameObject CarExirProminent;
    public GameObject AimProminent;
    public GameObject ShootProminent;
    int CurrentStatement;
    public Image Up, Down, Left, Right;
    bool Upb, Downb, Leftb, Rightb;
    int Killed;
    // Start is called before the first frame update
    void Start()
    {
        CurrentStatement = 0;
        StatementSystem();
        Invoke("StatementSystem", 13);
        Invoke("StartCameraTutorial", 20);
        PlayerObject.transform.position = PlayerPostion.position;
        PlayerObject.transform.rotation = PlayerPostion.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (CamStart)
        {
            if (ControlFreak2.CF2Input.GetAxis("SwipeLeft")<-10 && Leftb)
            {
                Up.gameObject.SetActive(true);
                Left.gameObject.SetActive(false);
                Leftb = false;
                Upb = true;
            }
            if (ControlFreak2.CF2Input.GetAxis("SwipeRight")>10 && Rightb)
            {
                Left.gameObject.SetActive(true);
                Right.gameObject.SetActive(false);
                Leftb = true;
                Rightb = false;
            }
            if (ControlFreak2.CF2Input.GetAxis("SwipeDown") < -10 && Downb)
            {
                Downb = false;
                CamStart = false;
                Down.gameObject.SetActive(false);
            }
            if (ControlFreak2.CF2Input.GetAxis("SwipeUp") > 10 && Upb)
            {
                Downb = true;
                Down.gameObject.SetActive(true);
                Up.gameObject.SetActive(false);
                Upb = false;
            }
        }
        if (CarbtnProminent.activeSelf)
        {
            if (!CovertScript.car)
            {
                CarbtnProminent.SetActive(false);
                StatementSystem();
            }
        }
        if (CarExirProminent.activeSelf)
        {
            if (CovertScript.car)
            {
                CarExirProminent.SetActive(false);
                StatementSystem();
            }
        }
    }

    public void StartCameraTutorial()
    {
        Right.gameObject.SetActive(true);
        Rightb = true;
        CamStart = true;
    }

    public void StatementSystem()
    {
        LevelManager.m_instance.CanvasObject.MessngerText.text = Statement[CurrentStatement];
        LevelManager.m_instance.Messenger.SetActive(true);
        CurrentStatement++;
        this.GetComponent<MarkerController>().TutorialMapMarkerTurnOff();
    }

    public void KillingCounter()
    {
        Killed++;
        AimProminent.SetActive(false);
        if (Killed == 2)
        {
            StatementSystem();
            Invoke("StartGame", 4);
        }
    }

    void StartGame()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        bl_SceneLoaderUtils.GetLoader.LoadLevel("Game");
    }

    public void AfteraTime()
    {
        Invoke("StatementSystem", 3);
    }
}
