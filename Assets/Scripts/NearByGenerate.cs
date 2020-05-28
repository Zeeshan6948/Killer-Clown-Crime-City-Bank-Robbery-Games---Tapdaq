using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NearByGenerate : MonoBehaviour
{
    public float range = 10.0f;
    public GameObject ObjectPrefab;
    public GameObject[] PeoplePrefabe;
    public static NearByGenerate Instance;
    public int MaximumNumbertoAppear;
    public int MaximumNumberCrowd;
    int currentNumber;
    public int BreakTime;
    bool called;
    public List<GameObject> PoliceObj;
    public List<GameObject> Crowd;
    public Camera DanceCamera;
    Vector3 screenPoint;
    bool onScreen;
    void Start()
    {
        currentNumber = 0;
        Instance = this;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    bool RandomPoint2(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                //screenPoint = DanceCamera.WorldToViewportPoint(hit.position);
                //onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                if (Vector3.Distance(transform.position, hit.position) > 7 )//&& onScreen)
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void Update()
    {
        
        
    }

    public void CreateAObject()
    {
        range = 30;
        for (int i = 0; i <= currentNumber; i++)
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                GameObject ob =  Instantiate(ObjectPrefab, point, Quaternion.identity);
                ob.GetComponent<AI>().factionNum = 2;
                ob.GetComponent<AI>().huntPlayer = true;
                ob.GetComponent<AI>().shootRange = 7;
                PoliceObj.Add(ob);
            }
        }
        Invoke("waitingforTimer", BreakTime);
    }

    void waitingforTimer()
    {
        currentNumber++;
        if (currentNumber >= MaximumNumbertoAppear)
        {
            currentNumber = MaximumNumbertoAppear;
        }
        LevelManager.m_instance.CanvasObject.ActivateStars(currentNumber);
        CreateAObject();
    }

    public void ResetAll()
    {
        currentNumber = 0;
    }

    public void FirstCalled()
    {
        if (!called)
        {
            Invoke("CreateAObject", 5);
            called = true;
            LevelManager.m_instance.CanvasObject.ActivateStars(currentNumber);
        }
    }

    public void DeleteAllPolice()
    {
        foreach(GameObject abc in PoliceObj)
        {
            Destroy(abc);
        }
        PoliceObj.Clear();
        currentNumber = 0;
        called = false;
        CancelInvoke("waitingforTimer");
        CancelInvoke("CreateAObject");
    }


    public void CreateACrowd()
    {
        range = 10;
        if (Crowd.Count >= MaximumNumberCrowd)
        {
            return;
        }
        for (int i = 0; i <= 5; i++)
        {
            Vector3 point;
            if (RandomPoint2(transform.position, range, out point))
            {
                GameObject ob = Instantiate(PeoplePrefabe[Random.Range(0,4)], point, Quaternion.identity);
                ob.GetComponent<AI>().factionNum = 1;
                Crowd.Add(ob);
            }
        }
        Invoke("CreateACrowd", 8);
    }

    public void DeleteAllCrowd()
    {
        foreach (GameObject abc in Crowd)
        {
            Destroy(abc);
        }
        Crowd.Clear();
        CancelInvoke("CreateACrowd");
    }
}
