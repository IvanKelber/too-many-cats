using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public Material selectedMaterial;
    public TargetedPlayer targetedPlayer;
    private Renderer _renderer;
    private Material _defaultMaterial;

    void Awake() {
        if(targetedPlayer == null) {
            targetedPlayer = ScriptableObject.CreateInstance<TargetedPlayer>();
        }
        if(targetedPlayer.isNull()) {
            targetedPlayer.SetNewPlayer(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
    }

    public void select() {
        _renderer.material = selectedMaterial;
    }

    public void deselect() {
        _renderer.material = _defaultMaterial;
    }

    public CameraHelper getCamera() {
        foreach(Transform child in transform) {
            Camera cam = child.GetComponent<Camera>();
            if(cam != null) {
                return new CameraHelper(cam, child.gameObject);
            }
        }
        return null;
    }

    public void targetSelf() {
        deselect();
        targetedPlayer.SetNewPlayer(this);
    }
}
