using UnityEngine;
using System.Collections;

public class PedestrianController : MonoBehaviour {

	protected Animator animator;
    public float hitPoints;
    private float speed = 0;
	public float direction = 0;
	private Locomotion locomotion = null;
	private Rigidbody body;
    bool stop;
    bool keepturn;
    Vector3 current;
    bool gothit;
    Vector3 CashPlace;
    // Use this for initialization
    void Start () 
	{
		body = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		locomotion = new Locomotion(animator);

		TSTrafficAI ai = GetComponent<TSTrafficAI>();
		ai.OnUpdateAI = OnAIUpdate;
		ai.UpdateCarSpeed = UpdateSpeed;

	}


	void UpdateSpeed(out float carSpeed)
	{
		carSpeed = body.velocity.z;
	}
	
	void OnAIUpdate(float steering, float brake, float throttle, bool isUpSideDown ){
		speed = Mathf.Clamp01(throttle - brake);
		direction = steering;
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ground")
        {
            stop = true;
            animator.applyRootMotion = false;
            locomotion.Do(0, 45 * direction);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ground")
        {
            animator.applyRootMotion = true;
            locomotion.Do(speed, 45 * direction);
            stop = false;
        }
    }
    void Update () 
	{
	
		if (animator && !stop)
		{
			if (body.constraints != RigidbodyConstraints.FreezeRotation)
                body.constraints = RigidbodyConstraints.FreezeRotation;
            if(!gothit)
			    locomotion.Do(speed , 45* direction);
            if(gothit)
                locomotion.Do(30, 45 * direction);
            if (keepturn)
                this.transform.localEulerAngles = current;
        }
        else
        {
            locomotion.Do(0, 45 * direction);
            Invoke("RotatetoTrun", 2);
        }
	}

    void RotatetoTrun()
    {
        CancelInvoke("RotatetoTrun");
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            this.transform.localEulerAngles += new Vector3(0, 45, 0);
            current = this.transform.localEulerAngles;
        }
        else
        {
            this.transform.localEulerAngles -= new Vector3(0, 45, 0);
            current = this.transform.localEulerAngles;
        }
        keepturn=true;
        Invoke("ResetTurn", 2);
    }
    private void ResetTurn()
    {
        keepturn = false;
    }

    public void ApplyDamage()
    {
        if (hitPoints <= 0.0f)
        {
            return;
        }
        hitPoints -= 27;
        if (hitPoints < 100.0f)
        {
            gothit = true;
            speed = 40;
        }
        if (hitPoints <= 0.0f)
        {
            animator.SetTrigger("Dead");
            NearByGenerate.Instance.FirstCalled();
            CashPlace = this.transform.position + new Vector3(0, 1, 0);
            Instantiate(LevelManager.m_instance.CashObject, CashPlace, Quaternion.identity);
            Invoke("SendBodyToRemove", 10);
        }
    }
    void SendBodyToRemove()
    {
        gothit = false;
        keepturn = false;
        stop = false;
        this.transform.position = new Vector3(50000, 50000, 50000);
    }
}
