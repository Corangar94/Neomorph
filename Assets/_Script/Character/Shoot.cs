using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Animator animator;

    private static readonly int ShootString = Animator.StringToHash("Shoot");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(ShootString);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Ball")
        {
            other.transform.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 600);
        }    
    }

 
}
