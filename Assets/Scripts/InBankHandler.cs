using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBankHandler : MonoBehaviour
{
    public static InBankHandler instance;
    public Transform place1;
    public Transform place2;
    public CharacterController[] PeopleInBank;
    public GameObject Person1;
    public GameObject Person2;
    public Transform EndPoint;
    public Vector3[] Postions;
    bool called;
    int i=0;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        instance = this;
        Postions = new Vector3[PeopleInBank.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunAll()
    {
        if (called)
            return;
        called = true;
        i = 0;
        foreach (CharacterController ob in PeopleInBank)
        {
            Postions[i] = ob.transform.position;
            ob.StartRun();
            ob.Remove = false;
            i++;
        }
        InvokeRepeating("StartCreatePeople", 3, 5);
    }

    public void StartCreatePeople()
    {
        GameObject ob = Instantiate(Person1, place1.position, place1.rotation);
        ob.GetComponent<CharacterController>().Path[0] = EndPoint;
        ob.GetComponent<CharacterController>().StartRun();
        ob.GetComponent<CharacterController>().Remove = true;
        GameObject ob2 = Instantiate(Person2, place2.position, place2.rotation);
        ob2.GetComponent<CharacterController>().Path[0] = EndPoint;
        ob2.GetComponent<CharacterController>().StartRun();
        ob2.GetComponent<CharacterController>().Remove = true;
    }

    public void StopAll()
    {
        if (!called)
            return;
        i = 0;
        called = false;
        foreach (CharacterController ob in PeopleInBank)
        {
            ob.transform.position = Postions[i];
            ob.Move = false;
            ob.CurrentAnim = AnimList.StandIdle;
            ob.Changeit = true;
            ob.Remove = false;
            i++;
        }
        CancelInvoke("StartCreatePeople");
    }
}
