using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Vector3 startPosition;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        resetBall();
    }

    void resetBall()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ResetBallPosition();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.CompareTag("GoalLine"))
        {
            ResetBallPosition();
        }    
    }

    private void ResetBallPosition()
    {
        transform.position = startPosition;
        rigidbody.velocity = Vector3.zero;
    }
}

