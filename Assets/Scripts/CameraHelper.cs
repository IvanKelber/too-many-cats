using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper {
    public Camera camera;
    public GameObject child;

    public CameraHelper(Camera camera, GameObject child) {
        this.camera= camera;
        this.child = child;
    }
}
