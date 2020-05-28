using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Notifications
{
    public Sprite IconImage;
    public string NotifyText;
}

public class NotificationGenrator : MonoBehaviour
{
    public static NotificationGenrator m_instance;
    public Notifications[] AllNotify;
    public int CurrentNumber;
    public Image IconImage;
    public Text NotifyText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Dance") != 0)
        {
            m_instance = this;
            //InvokeRepeating("NotifyUser", 5, 20);
        }
        Invoke("Disapper", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NotifyUser()
    {
        IconImage.sprite = AllNotify[CurrentNumber].IconImage;
        NotifyText.text = AllNotify[CurrentNumber].NotifyText;
    }

    private void OnEnable()
    {
        Invoke("Disapper", 5);
    }

    void Disapper()
    {
        this.gameObject.SetActive(false);
    }

    public void OnNotificationClick()
    {
        this.gameObject.SetActive(false);
    }
}
