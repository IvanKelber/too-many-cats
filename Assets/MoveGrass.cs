using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrass : MonoBehaviour
{
    [Range(0,.05f)]
    public float tileNoise = 0f;

    private Renderer _renderer;
    private float initialTileX;
    private float initialTileY;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        initialTileX = _renderer.material.mainTextureScale.x;
        initialTileY = _renderer.material.mainTextureScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleX = initialTileX + Mathf.Sin(Time.time) * tileNoise;
        float scaleY = initialTileY + Mathf.Sin(Time.time) * tileNoise;
        _renderer.material.mainTextureScale = new Vector2(scaleX, scaleY);
    }
}
