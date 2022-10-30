using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -7);
    private Vector3 newOffset = new Vector3(-20, 0, 0);
    private Vector3 finalOffset;

    public float turnSpeed = 4.0f;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform.position);
        transform.Rotate(newOffset);
    }
}