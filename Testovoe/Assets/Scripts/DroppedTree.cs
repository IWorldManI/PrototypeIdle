using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedTree : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(-180, 180), transform.eulerAngles.z);
        float speed = 8;
        rb.isKinematic = false;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        rb.AddForce(force * speed);
    }
    public void StartMovingToplayer()
    {
        StartCoroutine(woodToplayer());
    }

    public IEnumerator woodToplayer()
    {
        Vector3 target = FindObjectOfType<PlayerCollect>().transform.position;
        float totalMovementTime = 5f;  //the amount of time you want the movement to take
        float currentMovementTime = 0f; //The amount of time that has passed
        while (Vector3.Distance(transform.localPosition, target) > 0.25)
        {
            currentMovementTime += Time.fixedDeltaTime;
            transform.localPosition = Vector3.Lerp(transform.position, target, currentMovementTime / totalMovementTime);
            yield return null;
        }
        Destroy(this.gameObject);
        Debug.Log("wood collected");
    }

}

