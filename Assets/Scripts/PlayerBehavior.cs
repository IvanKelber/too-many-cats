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
        
    }

    public void select() {
        _renderer.material = selectedMaterial;
    }

    public void deselect() {
        _renderer.material = _defaultMaterial;
    }
}
