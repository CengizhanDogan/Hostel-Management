using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CMF;

public class CameraCollision : MonoBehaviour
{

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision
    Vector3 camPosition;
    public float minDistance = 1;                //min camera distance
    public float maxDistance = 2;                //max camera distance

    public float DistanceUp = -2;                    //how high the camera is above the player
    public float rotateAround = 70f;
    [Header("Map coordinate script")]
    RaycastHit hit;
    public float smooth = 15.0f;                    //how smooth the camera moves into place
    Vector3 camMask;

    private GameObject TransparentedGameObject;
    [SerializeField]
    private Material TransparentMaterial;
   
    private Material StandartMaterial;

    private Renderer WallHitRenderer;
    private bool isRaycastHit = false;
    public Transform targetTransform;
    private Transform lastHit;
    void LateUpdate()
    {
        OccludeRay(targetTransform.eulerAngles);
    }
   
    void OccludeRay(Vector3 targetFollow)
    {
        #region prevent wall clipping

        //Offset of the targets transform (Since the pivot point is usually at the feet).


        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        Vector3 raycastPoint = transform.position + transform.forward * -15f;
        Debug.DrawLine(raycastPoint, targetTransform.position+ Vector3.up,Color.magenta);


        if (Physics.Linecast(raycastPoint, targetTransform.position + Vector3.up, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            //smooth = 10f;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            if (isRaycastHit)
            {
                if(lastHit != wallHit.transform)
                    ChangeMaterial();
            }
            if (!isRaycastHit)
            {
                if(wallHit.collider.TryGetComponent(out Renderer wallhitRenderer))
                {
                    WallHitRenderer = wallhitRenderer;
                    StandartMaterial = WallHitRenderer.material;
                    WallHitRenderer.material = TransparentMaterial;
                    isRaycastHit = true;
                    lastHit = wallHit.transform;
                }
               
            }
            //camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        else
        {
            if (isRaycastHit)
            {
                ChangeMaterial();
            }
        }
        #endregion
    }
    private void ChangeMaterial()
    {
        WallHitRenderer.material = StandartMaterial;
        isRaycastHit = false;
    }
}