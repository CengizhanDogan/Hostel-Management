using CMF;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimatorController : MonoBehaviour
{
	private Rigidbody rb;
	public Rigidbody Rb => rb == null ? rb = GetComponentInParent<Rigidbody>() : rb;

	private AdvancedWalkerController walker;
	public AdvancedWalkerController Walker => walker == null ? walker = GetComponentInParent<AdvancedWalkerController>() : walker;
	private PlayerInputController playerMover;
	public PlayerInputController PlayerMover => playerMover == null ? playerMover = GetComponentInParent<PlayerInputController>() : playerMover;
	private Animator animator;
	public Animator Animator => animator == null ? animator = GetComponentInChildren<Animator>() : animator;

	[ReadOnly] public float walkSpeed;
	void Update()
	{
		SetWalkAnimations();
	}

	private void SetWalkAnimations()
	{
		walkSpeed = new Vector3(Rb.velocity.x, 0, rb.velocity.z).magnitude / Walker.movementSpeed;
		Animator.SetFloat("WalkSpeed", walkSpeed);
	}

	public void PlayDeathAnimation()
	{
		Animator.SetTrigger("Die");
	}
}
