using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void SetWalk(bool set)
    {
        animator.SetBool("Walk", set);
    }
}
