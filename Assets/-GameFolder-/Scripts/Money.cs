using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour, IInteractable
{
    [SerializeField] private Collider coll;
    [SerializeField] private Collider trigger;
    private Rigidbody rb;

    [SerializeField] private bool isPlaced;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Interact(Interactor interactor)
    {
        if (interactor.TryGetComponent(out CustomerGetter manager))
        {
            DOTween.Kill("Jump");
            if (!manager.isPlayer) return;
            SetColliders(false);
            rb.isKinematic = true;
            StartCoroutine(Follow(manager.transform));
        }
    }

    private IEnumerator Follow(Transform manager)
    {
        var pos = manager.transform.position; pos.y += 1;
        while (Vector3.Distance(transform.position, pos) > 0.1f)
        {
            pos = manager.transform.position; pos.y += 1;
            transform.position = Vector3.Lerp(transform.position, pos, 40 * Time.deltaTime);
            yield return null;
        }
        var audioManager = AudioManager.Instance;
        audioManager.PlaySound(audioManager.MoneySound, 0.1f);
        EventManager.OnGemCollected.Invoke(transform.position, () => { });
        if (isPlaced) Destroy(gameObject);
        else PoolingSystem.Instance.DestroyAPS(gameObject);
    }
    public void SetColliders(bool set)
    {
        coll.enabled = set;
        trigger.enabled = set;
        rb.isKinematic = !set;
    }
}
