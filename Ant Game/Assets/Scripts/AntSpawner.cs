using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public GameObject ant;
    public GameObject spawner;
    public float spawnTime;
    public int numberOfAnts;

    Vector2 spawnerPos;
    Vector2 newAntPos;
    Quaternion spawnerRot;

    // Start is called before the first frame update
    void Start()
    {
        spawnerPos = spawner.transform.position;
        newAntPos = new Vector2(spawnerPos.x, spawnerPos.y);
        spawnerRot = spawner.transform.rotation;

        Debug.Log(newAntPos + ", " + spawnerPos);

        if (numberOfAnts <= 0)
        {
            numberOfAnts = 1;
        };

        StartCoroutine(AntSpawn(numberOfAnts));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AntSpawn(int i)
    {
        var wait = new WaitForSeconds(spawnTime);
        while (i > 0)
        {
            Instantiate(ant, newAntPos, spawnerRot);
            i--;
            yield return wait;
        }
    }
}
