using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarFactor
{
    public class LimitCamera : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject Player;

        [Header("Settings")]
        [SerializeField] private float heightAbovePlayer;
        [SerializeField] private bool isRotationLocked;
        private Vector3 startRotation;


        private void Start()
        {
            startRotation = transform.eulerAngles;
        }

        private void Update()
        {
            transform.eulerAngles = GetRotation(isRotationLocked);
        }

        private void LateUpdate()
        {
            transform.position = GetPositionVec();
        }

        public void NorthLock()
        {
            isRotationLocked = true;
        }

        public void FreeRotation()
        {
            isRotationLocked = false;
        }

        private Vector3 GetPositionVec()
        {
            return Player.transform.position + new Vector3(0, heightAbovePlayer, 0);
        }

        private Vector3 GetRotation(bool lockToStart)
        {
            if (lockToStart)
            {
                return startRotation;
            }
            else
            {
                return new Vector3(90, 0, -Player.transform.eulerAngles.y);
            }
        }
    }
}