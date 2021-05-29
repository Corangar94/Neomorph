using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

//[ExecuteInEditMode]
public class GroundDetector : MonoBehaviour {
    [SerializeField]
    private LayerMask groundMask ;
    [SerializeField]
    private LayerMask wallMask ;
    Vector3 wallPoint;
    private SphereCollider collider;
    private GameObject body;
    [SerializeField]
    private float distance = 100f;
    private bool onGround;
   private void Start()
   {
       collider = GetComponent<SphereCollider>();
       body = PlayerController.Instance.gameObject;
   }
    
   
    void FixedUpdate()
    {
        Ray checkGround= new Ray(transform.position, -transform.up);
        Ray checkAbove= new Ray(transform.position, transform.up * distance);
          RaycastHit hit;
          if (NearWall() && FacingWall())
          {
              GrabWall();
              onGround = true;
          }
          else if (Physics.Raycast(checkAbove, out hit, distance, groundMask) )
          {
              CheckAbove(hit);
          }
          else if (Physics.Raycast(checkGround, out hit, distance, groundMask) )
          {
              CheckBelow(hit);
          }
          else
          {
              onGround = false;
          }
        
    }

    private void CheckBelow(RaycastHit hit)
    {
        Vector3 targetLocation = new Vector3(transform.position.x, hit.point.y, hit.point.z);
        targetLocation += new Vector3(0, transform.localScale.y, 0);
        transform.position = Vector3.Lerp(transform.position, targetLocation, 0.07F);
        onGround = true;
    }

    private void CheckAbove(RaycastHit hit)
    {
        Vector3 targetLocation = new Vector3(hit.point.z, hit.point.y, hit.point.z);
        if (targetLocation.y > transform.position.y + 0.1)
        {
            targetLocation += new Vector3(0, transform.localScale.y * 2, 0);
            transform.position = new Vector3(transform.position.x, targetLocation.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, targetLocation, 0.07F);
            onGround = true;   
        }
    }

    bool NearWall()
    {
        return Physics.CheckSphere(body.transform.position, 1f, wallMask);
    }
    bool FacingWall()
    {
        RaycastHit hit;
        var facingWall = Physics.Raycast(transform.position, -transform.forward, out hit, collider.radius + 1f, wallMask);
        wallPoint = hit.point;
        return facingWall;
    }

    void GrabWall()
    {
        transform.position = Vector3.Lerp(transform.position,  wallPoint, 50 * Time.deltaTime);

    }
}
