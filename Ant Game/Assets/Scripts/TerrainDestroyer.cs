using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    public Tilemap terrain;

    public void DestroyTerrain(Vector3 tile)
    {
        Vector3Int tilePos = terrain.WorldToCell(tile);
        if(terrain.GetTile(tilePos) != null)
        {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
