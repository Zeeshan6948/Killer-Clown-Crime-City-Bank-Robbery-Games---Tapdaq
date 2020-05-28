using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AnimList
{
    StandIdle, StandHandUp, StandHandBack, Walk, Sitting, Run
}

public class CharacterController : MonoBehaviour
{
    public Animator CharacterAnim;
    public AnimList CurrentAnim;
    public bool Changeit = false;
    public NavMeshAgent MyAgent;
    public bool loopPath;
    public Transform[] Path;
    public int Health;
    Vector3 CashPlace;
    public bool Move;
    int number;
    public bool Remove;
    bool once;
    void Start()
    {
        CharacterAnim = this.GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Changeit)
        {
            Changeit = false;
            ChangeAnimation();
        }
        if (MyAgent!=null && !MyAgent.pathPending && Move)
        {
            if (MyAgent.remainingDistance <= MyAgent.stoppingDistance)
            {
                if (!MyAgent.hasPath || MyAgent.velocity.sqrMagnitude == 0f)
                {
                    SetNextPathForAgent();
                }
            }
        }
    }

    public void ChangeAnimation()
    {
        int number = (int)CurrentAnim;
        switch (number)
        {
            case 0:
                CharacterAnim.SetTrigger("Idle");
                break;
            case 1:
                print("StandHandUp");
                break;
            case 2:
                print("StandHandBack");
                break;
            case 3:
                print("Walk");
                break;
            case 4:
                print("Sitting");
                break;
            case 5:
                CharacterAnim.SetTrigger("Run");
                break;
        }
    }

    public void StartRun()
    {
        CurrentAnim = AnimList.Run;
        Changeit = true;
        Move = true;
        MyAgent.SetDestination(Path[number].position);
    }

    void SetNextPathForAgent()
    {
        number++;
        if (number >= Path.Length && loopPath)
        {
            number = 0;
        }
        if (number >= Path.Length && !loopPath)
        {
            MyAgent.isStopped = true;
            if (Remove)
                Destroy(this.gameObject);
            else
                this.gameObject.SetActive(false);
            return;
        }
        MyAgent.SetDestination(Path[number].position);
    }

    public void ApplyDamage()
    {
        if (!once)
        {
            InBankHandler.instance.RunAll();
            once = true;
        }
        MyAgent.SetDestination(Path[0].position);
        CurrentAnim = AnimList.Run;
        Move = true;
        ChangeAnimation();
        if (Health <= 0.0f)
        {
            return;
        }
        Health -= 27;
        if (Health < 100.0f)
        {
            MyAgent.speed = 10;
        }
        if (Health <= 0.0f)
        {
            CharacterAnim.SetTrigger("Dead");
            MyAgent.speed = 10;
            NearByGenerate.Instance.FirstCalled();
            CashPlace = this.transform.position + new Vector3(0, 1, 0);
            Instantiate(LevelManager.m_instance.CashObject, CashPlace, Quaternion.identity);
            Invoke("SendBodyToRemove", 5);
        }
    }
    void SendBodyToRemove()
    {
        Move = false;
        if (Remove)
            Destroy(this.gameObject);
        else
            this.gameObject.SetActive(false);
    }
}
