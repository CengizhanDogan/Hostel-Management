using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorMovement : MonoBehaviour
{
    [SerializeField] private float resetTime;
    private float countDown;

    private bool canBePushed;

    private Rigidbody rb;

    private Vector3 euler;
    void Start()
    {
        countDown = resetTime;
        rb = GetComponent<Rigidbody>();
        euler = transform.localEulerAngles;

    }

    private void Update()
    {
        if (!rb.isKinematic && !canBePushed)
        {
            canBePushed = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!canBePushed) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Customer"))
        {
            countDown = resetTime;
            rb.isKinematic = false;
            DOTween.KillAll(transform);
            StopAllCoroutines();
            StartCoroutine(ResetDoor());
        }
    }

    private IEnumerator ResetDoor()
    {
        while (countDown > 0)
        {
            countDown--;
            yield return null;
        }
        rb.isKinematic = true;
        countDown = resetTime;
        transform.DOLocalRotate(euler, 0.5f).OnComplete(()=> rb.isKinematic = false);
    }
}
