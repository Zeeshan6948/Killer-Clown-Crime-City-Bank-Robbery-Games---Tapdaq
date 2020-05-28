using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    void PickUpItem()
    {
        if (this.transform.parent.GetComponent<RCC_CarControllerV3>())
        {
            LevelManager.m_instance.currentCar = this.transform.parent.gameObject;
            LevelManager.m_instance.m_Convertor.Convert = true;
            if(this.transform.parent.GetComponent<JokerAnimControl>())
                this.transform.parent.GetComponent<JokerAnimControl>().StartDriving();
            if (LevelManager.m_currentLevel == 0)
            {
                LevelManager.m_instance.callingSepecialFunctions(0);
            }
        }
        if (this.transform.parent.GetComponent<RCC_CarControllerV3>().gameObject.name == "Van")
        {
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<Level1>().ChasingStarted();
            LevelManager.m_instance.m_level[0].LevelObject.GetComponent<MarkerController>().ChasingStart();
        }
    }
}
