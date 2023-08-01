using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetMouse(Camera cam) {
        return new Vector3(Input.mousePosition.x - cam.pixelWidth/2, Input.mousePosition.y - cam.pixelHeight/2,0);
    }
}
