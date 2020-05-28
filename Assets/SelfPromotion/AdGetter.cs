using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class AdGetter : MonoBehaviour
{
    Image MyImage;
    Button MyButton;
    public bool RandomAd;
    public int Number;
    string Url;
    bool once;
    bool once2;
    // Start is called before the first frame update
    void Start()
    {
        MyImage = this.GetComponent<Image>();
        MyButton = this.GetComponent<Button>();
        if (RandomAd)
        {
            int value = JsonController.m_instance.RandomPickAd();
            MyImage.sprite = JsonController.m_instance.spriteToUse[value];
            Url = JsonController.m_instance.linkToUse[value];
            MyButton.onClick.AddListener(ClickLink);
        }
        else
        {
            int value = JsonController.m_instance.SpecificPickAd(Number);
            MyImage.sprite = JsonController.m_instance.spriteToUse[value];
            Url = JsonController.m_instance.linkToUse[value];
            MyButton.onClick.AddListener(ClickLink);
        }
    }

    public void ClickLink()
    {
        Application.OpenURL(Url);
    }

    // Update is called once per frame
    void Update()
    {
        if(JsonController.m_instance.Loaded && !once)
        {
            MyImage.enabled = true;
            Start();
            once = true;
        }
        else if(!once2)
        {
            Invoke("CallAgain", 2);
            once2 = true;
        }
    }

    void CallAgain()
    {
        if (JsonController.m_instance.Loaded)
        {
            once2 = false;
            return;
        }
        else
        {
            JsonController.m_instance.LoadJsonFromFile();
        }
    }
}
