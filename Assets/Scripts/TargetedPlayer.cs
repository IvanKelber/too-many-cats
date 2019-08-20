using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObjects/TargetedPlayer")]
public class TargetedPlayer : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector3 position = Vector3.zero;
    public CameraHelper cameraHelper = null;
    
    // [NonSerialized]
    public PlayerBehavior InitialPlayerBehavior = null;
    private bool isEmpty = true;

    public void SetPosition(Vector3 other) {
        position = other;
    }

    public void SetCamera(CameraHelper other) {
        Debug.Log("Setting camera: " + other.camera + " " +  other.child);
        if(cameraHelper != null) {
            cameraHelper.child.SetActive(false);
        }
        other.child.SetActive(true);
        cameraHelper = other;
    }

    public void SetNewPlayer(PlayerBehavior other) {
        if(other == null) {
            isEmpty = true;
            return;
        }
        SetPosition(other.transform.position);
        CameraHelper cameraHelper = other.getCamera();
        SetCamera(cameraHelper);
        isEmpty = false;
    }

    public Vector3 GetPosition() {
        return position;
    }

    public void OnAfterDeserialize() {
        InitialPlayerBehavior = null;
        isEmpty = true;
    }

    public void OnBeforeSerialize() {
        // Nothing
    
    }

    public void Awake() {
        SetNewPlayer(InitialPlayerBehavior);
    }

    public bool isNull() {
        return isEmpty;
    }

}
