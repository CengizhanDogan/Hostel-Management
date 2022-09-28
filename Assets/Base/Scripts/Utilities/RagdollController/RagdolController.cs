using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class RagdolController : RagdollBase
{
    public bool isPlayer;
    private void Awake()
    {
        DisableRagdoll();
    }
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        GameManager.Instance.OnStageFail.AddListener(OnFailed);     
    }
    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        GameManager.Instance.OnStageFail.RemoveListener(OnFailed);
    }    
    
    public void FreeFall() 
    {
        ActivateRagdoll();
    }       
    public void RagdollWithForce(Vector3 direction, float force)
    {
        ActivateRagdoll();
        AddForceToRagdollObject(direction, force);
    }

    public void OnFailed() 
    {
        if (isPlayer)
        {
            RagdollWithForce(transform.forward, 8f);
        }
    }
}