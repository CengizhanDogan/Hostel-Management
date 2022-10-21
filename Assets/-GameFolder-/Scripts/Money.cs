using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isPlaced;

    private Collider coll; 

    private void Start()
    {
        coll = GetComponent<Collider>();
        if (isPlaced)
        {
            coll.enabled = true;
        }
    }
    public IEnumerator Follow(Transform manager)
    {
        var pos = manager.transform.position; pos.y += 1;
        var followSpeed = 10f;
        while (Vector3.Distance(transform.position, pos) > 0.1f)
        {
            pos = manager.transform.position; pos.y += 1;
            transform.position = Vector3.Lerp(transform.position, pos, followSpeed * Time.deltaTime);
            followSpeed += 0.5f;
            yield return null;
        }
        var audioManager = AudioManager.Instance;
        audioManager.PlaySound(audioManager.MoneySound, 0.1f);
        EventManager.OnGemCollected.Invoke(pos, () => { });

        if (isPlaced) Destroy(gameObject);
        else PoolingSystem.Instance.DestroyAPS(gameObject);
    }

    public void Interact(Interactor interactor)
    {
        if (isPlaced)
        {
            StartCoroutine(Follow(interactor.transform));
        }
    }
}
