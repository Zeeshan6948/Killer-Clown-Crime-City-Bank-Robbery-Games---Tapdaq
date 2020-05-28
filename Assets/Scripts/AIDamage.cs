using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamage : MonoBehaviour
{
    public float hitPoints;
    public GameObject Fire;
    Rigidbody hitRigidbody;
    Renderer cubeRenderer;
    Color abc;
    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = this.GetComponent<Renderer>();
        abc = cubeRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.relativeVelocity.magnitude >= 2)
            {
                if (collision.collider.transform.GetComponent<Rigidbody>())
                {
                    hitRigidbody = collision.collider.transform.GetComponent<Rigidbody>();
                    hitRigidbody.AddExplosionForce(15 * hitRigidbody.mass, this.transform.position, 10, 3.0f, ForceMode.Impulse);
                }
            }
        }
    }
    public void ApplyDamage()
    {
        if (hitPoints <= 0.0f)
        {
            return;
        }
        hitPoints -= 27;
        if(hitPoints <= 25)
        {
            Fire.SetActive(true);
            cubeRenderer.material.SetColor("_Color", Color.black);
            this.transform.root.GetComponent<TSSimpleCar>().enabled = false;
            Invoke("InvokeAfter", 10);
        }
    }
    private void OnDisable()
    {
        Fire.SetActive(false);
        hitPoints = 100;
    }

    void InvokeAfter()
    {
        CancelInvoke("InvokeAfter");
        cubeRenderer.material.SetColor("_Color", abc);
        this.transform.root.GetComponent<TSSimpleCar>().enabled = true;
    }
}
