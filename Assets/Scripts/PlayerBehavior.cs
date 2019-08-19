using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public Material selectedMaterial;
    private Renderer _renderer;
    private Material _defaultMaterial;
    private int index = 0;
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
        Debug.Log("get camera returns null.");
        return null;
    }

    public void setGameController(GameController gameController) {
        SpotPlayer spotPlayer = GetComponentInChildren<SpotPlayer>();
        if(spotPlayer) {
            spotPlayer.setGameController(gameController);
        } else {
            throw new System.NullReferenceException("SpotPlayer does not exist.  Is the main camera active?");
        }
    }

    public void turnOn() {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void turnOff() {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public int getIndex() {
        return index;
    }

    public void setIndex(int index) {
        this.index = index;
    }

}
