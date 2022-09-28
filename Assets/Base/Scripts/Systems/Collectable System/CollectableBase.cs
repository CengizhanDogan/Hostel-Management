using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBase : MonoBehaviour, IInteractable
{
	public GameObject CollectEffectPrefab;
	[SerializeField] private bool uiCollectEffect;
	public virtual void Interact(Interactor collector)
	{
		if (CollectEffectPrefab != null)
		{
			ParticleSystem collectEffect = Instantiate(CollectEffectPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>();
			var main = collectEffect.main;
			main.stopAction = ParticleSystemStopAction.Destroy;

		}
		if (uiCollectEffect)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.Coin, PlayerPrefs.GetInt(PlayerPrefKeys.Coin) + 1);
			EventManager.OnGemCollected.Invoke(transform.position, () => { });
		}
	}
}
