using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssetsStatus : MonoBehaviour
{
    [Header("Player Clothes")]
    public Renderer PlayerBody;
    public Texture[] PlayerClothes;
    [Header("Bullets Status")]
    public PlayerWeapons Playerweapon;
    public int CurrentBullets;
    [Header("Guns Status")]
    public int[] GunsIndexes;
    [Header("Health Status")]
    public FPSPlayer PlayerHealth;
    public int ExtraHealth;
    // Start is called before the first frame update
    void Start()
    {
        PlayerBody.material.mainTexture = PlayerClothes[PlayerPrefs.GetInt("SelectedClothes")];
        Playerweapon.CurrentWeaponBehaviorComponent.ammo = PlayerPrefs.GetInt("BulletHave");
        settingbulletfirst();
        InvokeRepeating("updatebulletStatus", 3, 3);
        PlayerHealth.hitPoints = PlayerHealth.hitPoints + PlayerPrefs.GetInt("HealthHave");
        if (PlayerHealth.hitPoints > 100)
        {
            LevelManager.m_instance.CanvasObject.HealthBar1.fillAmount = 1;
            LevelManager.m_instance.CanvasObject.HealthBar.fillAmount = 1;
        }
        else if (PlayerHealth.hitPoints <= 100)
        {
            LevelManager.m_instance.CanvasObject.HealthBar1.fillAmount = 0;
            LevelManager.m_instance.CanvasObject.HealthBar.fillAmount = 1;
        }
        InvokeRepeating("updateHealthStatus", 3, 3);
        if (PlayerPrefs.GetInt("Gun0") == 1)
        {
            Playerweapon.weaponOrder[GunsIndexes[0]].GetComponent<WeaponBehavior>().haveWeapon = true;
            Playerweapon.weaponOrder[GunsIndexes[0]].GetComponent<WeaponBehavior>().cycleSelect = true;
        }
        if (PlayerPrefs.GetInt("Gun1") == 1)
        {
            Playerweapon.weaponOrder[GunsIndexes[1]].GetComponent<WeaponBehavior>().haveWeapon = true;
            Playerweapon.weaponOrder[GunsIndexes[1]].GetComponent<WeaponBehavior>().cycleSelect = true;
        }
    }

    void updatebulletStatus()
    {
        PlayerPrefs.SetInt("BulletHave", Playerweapon.CurrentWeaponBehaviorComponent.ammo);
        settingbulletfirst();
    }

    void updateHealthStatus()
    {
        ExtraHealth = PlayerPrefs.GetInt("HealthHave");
        if(PlayerHealth.hitPoints < ExtraHealth && PlayerHealth.hitPoints>1)
        {
            PlayerPrefs.SetInt("HealthHave", (int)PlayerHealth.hitPoints);
        }
    }

    void settingbulletfirst()
    {
        foreach (WeaponBehavior wb in Playerweapon.weaponBehaviors)
        {
                wb.ammo = PlayerPrefs.GetInt("BulletHave");
        }
    }
    // Update is called once per frame
    void Update()
    {
        CurrentBullets = PlayerPrefs.GetInt("BulletHave");
    }
}
