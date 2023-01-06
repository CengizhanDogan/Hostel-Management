using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Transform followTransform;
    private bool follow;
    public void SetFollowTransform(Transform follow)
    {
        followTransform = follow;
        this.follow = follow;
    }
    void Update()
    {
        if (!follow) return;
        Vector3 followPos = followTransform.position;
        followPos.y = transform.position.y;
        transform.position = followPos;
    }
}
