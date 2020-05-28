using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    public MapMarker[] many;
    public MapMarker[] SelectiveOn;
    public MapMarker[] InBank;
    public MapMarker[] Chase;
    public MapMarker[] Tutorial;
    public MapMarker[] Dance;
    bool again=true;
    // Start is called before the first frame update
    void Start()
    {
        many = GameObject.FindObjectsOfType<MapMarker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapMarkerTurnOff()
    {
        again = !again;
        if(again == true)
        {
            Reactive();
            return;
        }

        foreach(MapMarker abc in many)
        {
            if (abc != null)
            {
                foreach(MapMarker ob in SelectiveOn)
                {
                    if (abc != ob)
                    {
                        abc.isActive = false;
                    }
                }
            }
        }
        foreach (MapMarker ob in InBank)
        {
            if (ob != null)
            {
                ob.isActive = true;
            }
        }
    }

    public void Reactive()
    {
        foreach (MapMarker abc in many)
        {
            abc.isActive = true;
        }
        foreach (MapMarker ob in InBank)
        {
            if (ob != null)
            {
                ob.isActive = false;
            }
        }
    }

    public void ChasingStart()
    {
        foreach (MapMarker abc in many)
        {
            if (abc != null)
            {
                abc.isActive = false;
            }
        }
        foreach (MapMarker abc in Chase)
        {
            abc.isActive = true;
        }
    }

    public void TutorialMapMarkerTurnOff()
    {
        foreach (MapMarker abc in many)
        {
            if (abc != null)
            {
                abc.isActive = false;
            }
        }
        foreach (MapMarker ob in Tutorial)
        {
            if (ob != null)
            {
                ob.isActive = true;
            }
        }
    }

    public void DanceMapMarkerTurnOff()
    {
        foreach (MapMarker abc in many)
        {
            if (abc != null)
            {
                abc.isActive = false;
            }
        }
        foreach (MapMarker ob in Dance)
        {
            if (ob != null)
            {
                ob.isActive = true;
            }
        }
    }
}
