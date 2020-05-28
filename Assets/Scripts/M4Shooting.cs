using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4Shooting : MonoBehaviour
{
    public float FireRate;
    float time = 0;
    public AudioSource ShootAudio;
    public MeshRenderer effect;
    public LayerMask bulletMask;
    public Transform mainCamTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    RaycastHit tpRayHit;
    Vector3 cameraForwardPoint;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= FireRate)
        {
            if (Physics.Raycast(mainCamTransform.position, mainCamTransform.forward, out tpRayHit, 200, bulletMask))
            {
                cameraForwardPoint = tpRayHit.point;
                ShootAudio.Play();
                effect.enabled = true;
                time = 0;
            }
            GameObject cloneParent = AzuObjectPool.instance.SpawnPooledObj(4, tpRayHit.point + (tpRayHit.normal * 0.025f), Quaternion.identity) as GameObject;
            cloneParent.transform.localScale = new Vector3(2, 2, 2);
            GameObject cloneDecal = cloneParent.transform.GetChild(0).gameObject;
            FadeOutDecals FadeOutDecalsComponent = cloneDecal.transform.GetComponent<FadeOutDecals>();
            Transform tempObjTransform = FadeOutDecalsComponent.parentObjTransform;
            Transform cloneTransform = FadeOutDecalsComponent.myTransform;
            FadeOutDecalsComponent.InitializeDecal();
            cloneTransform.parent = null;
            cloneDecal.transform.rotation = Quaternion.FromToRotation(Vector3.up, tpRayHit.normal);
            //save initial scaling of bullet mark prefab object
            Vector3 scale = cloneTransform.localScale;
            //set parent of empty game object to hit object's transform
            tempObjTransform.parent = tpRayHit.transform;
            if (tpRayHit.collider.gameObject.layer == 15)
                tpRayHit.collider.gameObject.GetComponent<AIDamage>().ApplyDamage();
            //set scale of empty game object to (1,1,1) to prepare it for applying the inverse scale of the object that was hit
            tempObjTransform.localScale = Vector3.one;
            //sync empty game object's rotation quaternion with hit object's quaternion for correct scaling of euler angles (use the same orientation of axes)
            Quaternion tempQuat = tpRayHit.transform.rotation;
            tempObjTransform.rotation = tempQuat;
            Vector3 tempScale1 = Vector3.one;
        }
        effect.enabled = false;
    }
}
