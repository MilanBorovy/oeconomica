using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private bool topview = false;
	
	void FixedUpdate () {
        if (topview)
        {
            //View from top
            transform.position = new Vector3(250, 450, 250);
            transform.eulerAngles = new Vector3(90, 0, 0);
            if (Input.GetKeyDown(KeyCode.Space)) { topview = false; }
        }
        else
        {
            //Normal view
            int ws = 0;
            int ad = 0;
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 40;
            int modifier = 1;
            float maxScroll = (transform.position.y - 100) / 150 * 270;
            if (Input.GetKey(KeyCode.W)) { ws = 1; } else if (Input.GetKey(KeyCode.S)) { ws = -1; }
            if (Input.GetKey(KeyCode.D)) { ad = 1; } else if (Input.GetKey(KeyCode.A)) { ad = -1; }
            if (Input.GetKey(KeyCode.LeftShift)) { modifier = 2; }
            if (Input.GetKeyDown(KeyCode.Space)) { topview = true; }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + 5 * ad * modifier, 100, 400), Mathf.Clamp(transform.position.y - scroll, 100, 250), Mathf.Clamp(transform.position.z + 5 * ws * modifier, -10, 400 - maxScroll));
            transform.eulerAngles = new Vector3(60, 0, 0);
        }
	}
}
