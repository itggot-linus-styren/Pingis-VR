using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBallTest : MonoBehaviour {

    public Rigidbody rb;

    public float throwStrength;
    public Transform dirObject;

    public bool miss;
    public bool hasReset;

    private Vector3 initPos;

    private float timeout;
    private float timeSinceReset;
    private bool doReset;

	// Use this for initialization
	void Start () {
        doReset = true;
        initPos = transform.position;
    }
	
	// Update is called once per frame

	void FixedUpdate () {
        if (doReset) {
            transform.position = initPos + new Vector3(Random.Range(-1.2f, 1.2f), -1f, 0f);
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(-dirObject.forward * throwStrength, ForceMode.Force);
            doReset = false;
            timeout = Time.time;
            timeSinceReset = Time.time;
        }
        if (rb.velocity.magnitude > 0.5) {
            timeout = Time.time;
        }

        if (Time.time - timeout > 1f || Time.time - timeSinceReset > 5f) {
            doReset = true;
            hasReset = true;
            miss = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("goal")) {
            miss = true;
        }
        hasReset = true;
        doReset = true;
    }
}
