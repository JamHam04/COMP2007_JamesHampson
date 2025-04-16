using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour
{
    public float carSpeed = 5f;
    public int direction = 1; // For both sides of road

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, direction) * carSpeed * Time.deltaTime;
    }

    public void SetDirection(int dir)
    {
        direction = dir;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has been hit!");
        }
    }
}
