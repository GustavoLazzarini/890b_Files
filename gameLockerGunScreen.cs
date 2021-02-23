//Copyright © SwipeProductions

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Plugins.InputHandler;

namespace Game.Gameplay
{
    public class gameLockerGunScreen : MonoBehaviour
    {
        private bool OnScreen;
        private Transform cameraTransform;

        private bool lerpEnabled;

        private Vector3 targetPosition;
        private Vector3 targetRotation;
        private Vector3 targetSize;

        private Coroutine enableLerpCoroutine;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            GetInputs();

            if (lerpEnabled)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 5);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * 5);
                transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime * 6);
            }
        }

        private void Initialize()
        {
            cameraTransform = Camera.main.transform;

            targetPosition = transform.position;
            targetRotation = transform.rotation.eulerAngles;
            targetSize = Vector3.one;
        }

        public void GoToScreen()
        {
            OnScreen = true;

            MovementPlayer.canMove = false;

            transform.parent = cameraTransform;

            targetPosition = new Vector3(0, 0, 0.7f);
            targetRotation = new Vector3(0, 125, 0);

            if (enableLerpCoroutine != null)
                StopCoroutine(enableLerpCoroutine);

            enableLerpCoroutine = StartCoroutine(EnableLerpCoroutine());
        }

        public void GetInputs()
        {
            if (InputHandler.GetButtonDown(InputHandler.eButtonCode.Any))
            {
                if (OnScreen && !lerpEnabled)
                {
                    Disapear();
                }
            }
        }

        private void Disapear()
        {
            OnScreen = false;

            MovementPlayer.canMove = true;

            targetSize = Vector3.zero;

            if (enableLerpCoroutine != null)
                StopCoroutine(enableLerpCoroutine);

            enableLerpCoroutine = StartCoroutine(EnableLerpCoroutine());
        }

        private IEnumerator EnableLerpCoroutine()
        {
            lerpEnabled = true;
            yield return new WaitForSeconds(3);
            lerpEnabled = false;
        }
    }
}
