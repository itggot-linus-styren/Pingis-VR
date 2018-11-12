using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetBall : MonoBehaviour {

    public Transform rack;

    public bool reset;

    private void Start()
    {
        reset = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        reset = true;
        collision.rigidbody.angularVelocity = Vector3.zero;
        collision.rigidbody.AddForce(-rack.forward * 430);
    }
}
