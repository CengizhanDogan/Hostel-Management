using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour, IInteractable
{
    [SerializeField] private Collider coll;
    [SerializeField] private Collider trigger;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Interact(Interactor interactor)
    {
        var manager = interactor.GetComponent<CustomerGetter>();

        if (!manager.isPlayer) return;
        SetColliders(false);
        rb.isKinematic = true;
        StartCoroutine(FollowManager(manager.transform));
    }

    private IEnumerator FollowManager(Transform manager)
    {
        var pos = manager.transform.position; pos.y += 1;
        while (Vector3.Distance(transform.position, pos) > 0.1f)
        {
            pos = manager.transform.position; pos.y += 1;
            transform.position = Vector3.Lerp(transform.position, pos, 40 * Time.deltaTime);
            yield return null;
        }
        EventManager.OnGemCollected.Invoke(transform.position, () => { });
        PoolingSystem.Instance.DestroyAPS(gameObject);
    }
    public void SetColliders(bool set)
    {
        coll.enabled = set;
        trigger.enabled = set;
        rb.isKinematic = !set;
    }
}
