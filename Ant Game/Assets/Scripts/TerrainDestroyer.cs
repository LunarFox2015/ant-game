using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    public Tilemap terrain;

    public void DestroyTerrain(Vector3 tile)
    {
        Debug.Log("Ant is trying to destroy terrain");
        Vector3Int tilePos = terrain.WorldToCell(tile);
        if(terrain.GetTile(tilePos) != null)
        {
            Debug.Log("Terrain tile detected, deleting terrain");
            DestroyTile(tilePos);
        }
    }

    void DestroyTile(Vector3Int tile)
    {
        terrain.SetTile(tile, null);
    }

    // Start is called before the first frame update
    void Start()
    {
        terrain = FindObjectOfType<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
