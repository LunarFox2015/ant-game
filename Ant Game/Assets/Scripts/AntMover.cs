using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMover : MonoBehaviour
{
    Rigidbody2D ant;
    int moveDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        ant = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Walk()
    {
        var antPos = new Vector2(ant.transform.position.x, ant.transform.position.y);

        transform.position = new Vector2(antPos.x + (moveDirection * Time.deltaTime), antPos.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection *= -1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Walk();
        }
    }
}
