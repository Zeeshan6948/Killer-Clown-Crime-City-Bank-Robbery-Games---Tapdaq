using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlConverter : MonoBehaviour
{
    public bool Convert;
    public bool car;
    public GameObject[] CarObjects;
    public GameObject[] PlayerObjects;
    public GameObject PlayerClone;
    public MapCanvasController Minimap;
    public PlayerWeapons Myweapon;
    public int gunindex;
    // Start is called before the first frame update
    void Start()
    {
        car = false;
        RCC_SceneManager.Instance.activePlayerCamera = CarObjects[0].GetComponent<RCC_Camera>();
        Myweapon = GameObject.FindObjectOfType<PlayerWeapons>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        if (Convert)
        {
            if (!car)
            {
                Convert = false;
                ConvertoPerson();
                return;
            }
            if (car)
            {
                Convert = false;
                ConvertoCar();
                return;
            }
        }
    }
    void UpdateHealth()
    {
        LevelManager.m_instance.CanvasObject.HealthBar.fillAmount = PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints;
        if(PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints > 100)
        {
            LevelManager.m_instance.CanvasObject.HealthBar1.fillAmount = (float)(PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints - 100) / (float)PlayerPrefs.GetInt("HealthHave");
        }
        else if (PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints <= 100)
        {
            LevelManager.m_instance.CanvasObject.HealthBar1.fillAmount = 0;
            LevelManager.m_instance.CanvasObject.HealthBar.fillAmount = (float)(PlayerObjects[0].GetComponent<FPSPlayer>().hitPoints) / (float)(100);
        }
    }
    public void ConvertoCar()
    {
        car = false;
        foreach (GameObject ob in PlayerObjects)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in CarObjects)
        {
            ob.SetActive(true);
        }
        PlayerClone = GameObject.Find("Player Character Model(Clone)");
        if (PlayerClone != null)
        {
            PlayerClone.SetActive(false);
            PlayerClone.transform.parent = LevelManager.m_instance.currentCar.transform;
        }
        gunindex = Myweapon.currentWeapon;
        PlayerObjects[0].transform.parent = LevelManager.m_instance.currentCar.transform;
        CarObjects[0].GetComponent<RCC_Camera>().playerCar = LevelManager.m_instance.currentCar.GetComponent<RCC_CarControllerV3>();
        RCC.RegisterPlayerVehicle(LevelManager.m_instance.currentCar.GetComponent<RCC_CarControllerV3>(),true);
        Minimap.playerTransform = LevelManager.m_instance.currentCar.transform;
        if(!LevelManager.m_instance.currentCar.GetComponent<RCC_CarControllerV3>().engineRunning)
            LevelManager.m_instance.currentCar.GetComponent<RCC_CarControllerV3>().KillOrStartEngine();
        LevelManager.m_instance.currentCar.GetComponent<MapMarker>().isActive = false;
        Camera.main.farClipPlane = 1;
    }
    public void ConvertoPerson()
    {
        car = true;
        Camera.main.farClipPlane = 100;
        foreach (GameObject ob in CarObjects)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in PlayerObjects)
        {
            ob.SetActive(true);
        }
        PlayerObjects[0].transform.parent = null;
        if (PlayerClone != null)
        {
            PlayerClone.SetActive(true);
            PlayerClone.transform.parent = null;
        }
        Minimap.playerTransform = PlayerObjects[0].transform;
        StartCoroutine(Myweapon.SelectWeapon(0));
        Invoke("afterTime", 1);
        if (LevelManager.m_instance.currentCar != null)
        {
            LevelManager.m_instance.currentCar.GetComponent<MapMarker>().isActive = true;
            LevelManager.m_instance.currentCar.GetComponent<RCC_CarControllerV3>().KillEngine();
        }
    }

    void afterTime()
    {
        StartCoroutine(Myweapon.SelectWeapon(gunindex));
    }
}
