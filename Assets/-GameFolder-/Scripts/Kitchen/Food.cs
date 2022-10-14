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
        transform.position = followTransform.position;
    }
}
