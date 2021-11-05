using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMover : MonoBehaviour
{
    Rigidbody2D ant;
    // Start is called before the first frame update
    void Start()
    {
        ant = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var antPos = new Vector2(ant.transform.position.x, ant.transform.position.y);

        transform.position = new Vector2(antPos.x + (1 * Time.deltaTime), antPos.y);
    }
}
