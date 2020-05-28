//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2017 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

#pragma warning disable 0414

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Mobile UI Drag used for orbiting RCC Camera.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Drag Handler")]
public class RCC_UIDrag2 : MonoBehaviour, IDragHandler, IEndDragHandler{

	private bool isPressing = false;
    public RCC_Camera2 rccCam;
    void OnEnable()
    {
        if(rccCam==null)
        rccCam = GameObject.FindObjectOfType<RCC_Camera2>();

    }
	public void OnDrag(PointerEventData data){
        Debug.Log(rccCam.gameObject);
		isPressing = true;
      // RCC_SceneManager.Instance.activePlayerCamera.OnDrag (data);
        rccCam.OnDrag(data);
	}

	public void OnEndDrag(PointerEventData data){

		isPressing = false;

	}

}
