using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    [SerializeField] private float height;

    private Vector3 rotationLimit;

    private void Start()
    {
        rotationLimit = transform.localEulerAngles;
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = rotationLimit;
        transform.position = GetPositionVec();
    }

    private Vector3 GetPositionVec()
    {
        return Player.transform.position + new Vector3(0, height, 0);
    }
}