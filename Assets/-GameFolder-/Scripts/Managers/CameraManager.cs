using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCam;
    [SerializeField] private List<Transform> targets = new List<Transform>();

    private bool doOnce;
    private int count;

    private void OnEnable()
    {
        EventManager.OnTutorialEvent.AddListener(AddTarget);
    }
    private void OnDisable()
    {
        EventManager.OnTutorialEvent.RemoveListener(AddTarget);
    }
    private void AddTarget(Transform target)
    {
        targets.Add(target);
        count = targets.Count;
        if (!doOnce)
        {
            StartCoroutine(SetCams());
            JoystickEnabler.Instance.SetJoystick(false);
            PlayerStopper.Instance.SetWall();
            doOnce = true;
        }
    }

    private IEnumerator SetCams()
    {
        var time = 2.5f;
        List<CinemachineVirtualCamera> cams = new List<CinemachineVirtualCamera>();

        for (int i = 0; i < count; i++)
        {
            if (!targets[i]) continue;

            var cam = PoolingSystem.Instance.InstantiateAPS("Cam")
                .GetComponent<CinemachineVirtualCamera>();

            cams.Add(cam);
            cams[i].Follow = targets[i];
            cams[i].LookAt = targets[i];

            if (i > 0) { cams[i - 1].gameObject.SetActive(false); }

            mainCam.SetActive(false);
            cams[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(time);
        }

        targets.Clear();
        JoystickEnabler.Instance.SetJoystick(true);
        PlayerStopper.Instance.SetWall();
        mainCam.gameObject.SetActive(true);
        for (int i = 0; i < cams.Count; i++)
        {
            PoolingSystem.Instance.DestroyAPS(cams[0].gameObject);
        }

        doOnce = false;
    }
}