using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PingisAgent : Agent {

    public Transform target;
    public resetBall resetball;
    public ThrowBallTest throwBall;
    public Transform rack;
    public Transform environment;

    public float rackSpeed;
    public float rotateSpeed;

    float lastDistance = 100000;
    bool movedTarget = false;
    float lastXpos;
    Vector3 initPos;

    private void Start()
    {
        rack.position = initPos = rack.position + new Vector3(0, 0.35f, 0);
        lastXpos = rack.position.x;
    }

    public override void AgentReset()
    {
        rack.position = initPos;
    }

    public override void CollectObservations()
    {
        // Calculate relative position
        Vector3 relativePosition = target.position - rack.position;

        // Relative position
        AddVectorObs(relativePosition);
        AddVectorObs(target.GetComponent<Rigidbody>().velocity);
        AddVectorObs(rack.eulerAngles.y);

        Monitor.Log("relaive pos x", relativePosition.x, null);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float distanceToTarget = Mathf.Abs(target.position.x - rack.position.x);

        Monitor.Log("distance", distanceToTarget, null);
        Monitor.Log("reward", this.GetCumulativeReward(), null);

        if (resetball.reset)
        {
            resetball.reset = false;
            AddReward(0.05f);
            //Done();
        }

        if (Mathf.Abs(rack.position.x - environment.position.x) > 1.5f) {
            AddReward(-0.1f);
            Done();
        }

        //AddReward(0.01f);

        if (throwBall.hasReset)
        {
            throwBall.hasReset = false;
            if (throwBall.miss) {
                throwBall.miss = false;
                AddReward(-0.1f);
                Done();
            } else {
                AddReward(0.05f);
            }
            lastDistance = 1000000;
            movedTarget = false;
        }

        if (Mathf.Abs(rack.eulerAngles.y) > 0.1) {
            AddReward(-0.001f);
        }/*

        if (movedTarget)
        {
            if (distanceToTarget < lastDistance) {
                AddReward(0.04f);
                lastDistance = distanceToTarget;
            } else {
                AddReward(-0.0025f);
            }
        }
        else
        {
            movedTarget = true;
            lastDistance = distanceToTarget;
        }*/

        Quaternion targetRot = Quaternion.Euler(0, 0, 0);

        if (Mathf.Abs(rack.position.x - lastXpos) > Mathf.Epsilon) // we are moving
        {
            if (rack.position.x > lastXpos)
            { // moving right
                targetRot = Quaternion.Euler(0, 30, 0);
            } else
            { // moving left
                targetRot = Quaternion.Euler(0, -30, 0);
            }
        }

        // The step size is equal to speed times frame time.
        float step = rotateSpeed * Time.fixedDeltaTime;
        Quaternion currentRot = rack.rotation;
        Vector3 euler = currentRot.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        currentRot.eulerAngles = euler;

        Quaternion lookRot = Quaternion.Slerp(currentRot, targetRot, step);

        euler = lookRot.eulerAngles;
        euler.x = rack.eulerAngles.x;
        euler.z = rack.eulerAngles.z;
        lookRot.eulerAngles = euler;
        transform.rotation = lookRot;

        //Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(vectorAction[0] * 0.5f + 0.5f, 0.5f, rack.position.z - Camera.main.transform.position.z));
        Vector3 pos = rack.position + new Vector3(vectorAction[0], 0, 0);
        lastXpos = rack.position.x;
        rack.position = Vector3.MoveTowards(rack.position, pos, rackSpeed * Time.fixedDeltaTime);
    }
}
