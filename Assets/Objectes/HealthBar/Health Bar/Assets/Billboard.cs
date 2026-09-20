using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform cam;
    private void Start()
    {
        Camera cam1 = FindObjectOfType<Camera>();
        cam = cam1.transform;
    }
    void LateUpdate()
    {
		transform.LookAt(transform.position + cam.forward);
    }
}
