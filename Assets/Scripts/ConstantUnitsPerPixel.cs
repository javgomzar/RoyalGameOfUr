using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantUnitsPerPixel : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        cam.orthographicSize = cam.pixelHeight / 2;
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = cam.pixelHeight / 2;
    }
}
