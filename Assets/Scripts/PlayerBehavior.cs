using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    public Material selectedMaterial;
    private Renderer _renderer;
    private Material _defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) {
            turnOff();
        }
    }

    public void select() {
        _renderer.material = selectedMaterial;
    }

    public void deselect() {
        _renderer.material = _defaultMaterial;
    }

    public Camera getCamera() {
        Camera _cam = gameObject.GetComponentInChildren<Camera>();
        return _cam;
    }

    public void setGameController(GameController gameController) {
        GetComponentInChildren<SpotPlayer>().setGameController(gameController);
    }

    public void turnOn() {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void turnOff() {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
