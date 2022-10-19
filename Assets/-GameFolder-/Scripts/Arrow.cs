using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;

    private Transform follow;
    private Vector3 offset;
    void Start()
    {
        var arrowY = arrow.localPosition.y;
        arrow.DOLocalMoveY(arrowY + 1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetFollow(Transform follow, Vector3 offset)
    {
        this.follow = follow;
        this.offset = offset;
    }
    private void Update()
    {
        if (follow)
        {
            transform.position = follow.position + offset;
        }
    }
}
