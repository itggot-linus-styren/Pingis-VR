using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRackTest : MonoBehaviour {

    public Animator anim;
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, pos.z - Camera.main.transform.position.z));
        pos.x = mousePos.x;
        pos.y = mousePos.y;
        transform.position = pos;

        if (Input.GetMouseButtonDown(0)) {
            anim.SetTrigger("swingRack");
        }
	}
}
