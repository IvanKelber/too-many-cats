using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObjects/TargetedPlayer")]
public class TargetedPlayer : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector3 position = Vector3.zero;
    public CameraHelper cameraHelper = null;
    public PlayerBehavior playerBehavior = null;

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

    public void SetPlayerBehavior(PlayerBehavior other) {
        playerBehavior = other;
    }

    public void SetNewPlayer(PlayerBehavior other) {
        SetPosition(other.transform.position);
        CameraHelper cameraHelper = other.getCamera();
        SetCamera(cameraHelper);
        SetPlayerBehavior(other);
    }

    public Vector3 GetPosition() {
        return position;
    }

    public void OnAfterDeserialize() {

    }

    public void OnBeforeSerialize() {
        // Nothing
    
    }

}
