using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;   


[System.Serializable]
public class LinkingData
{
    public string Link;
    public string PicLink;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class JsonController : MonoBehaviour
{
    public string JSONLink;
    public int TotalValues;
    public Sprite[] spriteToUse;
    public string[] linkToUse;
    public static JsonController m_instance;

    public bool Loaded;
    LinkingData[] Values;
    Rect rec;
    Texture myTexture;
    // Start is called before the first frame update
    void Start()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        StartCoroutine(GetTheJson(JSONLink));
    }


    public string LoadJsonFromFile()
    {
        string json;
        using (StreamReader r = new StreamReader("Assets\\file.json"))
        {
            json = r.ReadToEnd();
        }
        return json;
    }

    IEnumerator GetTheJson(string link)
    {
        UnityWebRequest www = UnityWebRequest.Get(link);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            Values = JsonHelper.FromJson<LinkingData>(jsonString);
            TotalValues = Values.Length;
            spriteToUse = new Sprite[TotalValues];
            linkToUse = new string[TotalValues];
            StartCoroutine(SetSprites());
        }
    }

    IEnumerator SetSprites()
    {
        for(int i=0; i<TotalValues; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(Values[i].PicLink);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                rec = new Rect(0, 0, myTexture.width, myTexture.height);
                spriteToUse[i] = Sprite.Create((Texture2D)myTexture, rec, new Vector2(0.5f, 0.5f), 100);
            }
            linkToUse[i] = Values[i].Link;
            Loaded = true;
        }
    }


    public int RandomPickAd()
    {
        int RandomValue = Random.Range(0, TotalValues);
        return RandomValue;
    }

    public int SpecificPickAd(int Number)
    {
        if(Values[Number] != null)
        {
            return Number;
        }
        else
        {
            if(Values[TotalValues-1] != null)
            {
                return TotalValues - 1;
            }
            else
            {
                return -1;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
