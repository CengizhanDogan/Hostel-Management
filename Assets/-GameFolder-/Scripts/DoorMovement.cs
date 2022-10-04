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
    void Start()
    {
        countDown = resetTime;
        rb = GetComponent<Rigidbody>();
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
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Customer"))
        {
            Debug.Log("Collide");
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
            Debug.Log(countDown);
            countDown--;
            yield return null;
        }
        rb.isKinematic = true;
        countDown = resetTime;
        transform.DOLocalRotate(Vector3.up * 180, 0.5f).OnComplete(()=> rb.isKinematic = false);
    }
}
