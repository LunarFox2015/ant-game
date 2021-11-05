using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public GameObject ant;
    public GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
        var spawnerPos = spawner.transform.position;
        var newAntPos = new Vector3(spawnerPos.x, spawnerPos.y);
        var spawnerRot = spawner.transform.rotation;

        GameObject newAnt = Instantiate(ant, newAntPos, spawnerRot);
        newAnt.transform.position = spawnerPos;

        Debug.Log(newAntPos + ", " + spawnerPos);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
