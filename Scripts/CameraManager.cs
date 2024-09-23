using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Application = UnityEngine.Device.Application;

namespace ProcGen2D.Sample
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private bool _forceMobile;

        [SerializeField]
        private Transform _clouds;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            var pixelPerfect = _mainCamera.GetComponent<PixelPerfectCamera>();

            if (Application.isMobilePlatform || _forceMobile)
            {
                pixelPerfect.assetsPPU = 48;
                pixelPerfect.cropFrame = PixelPerfectCamera.CropFrame.Pillarbox;
                pixelPerfect.refResolutionX = 1280;
                pixelPerfect.refResolutionY = 768;
            }
            else
            {
                
                pixelPerfect.assetsPPU = 16;
                pixelPerfect.cropFrame = PixelPerfectCamera.CropFrame.StretchFill;
                pixelPerfect.refResolutionX = 1024;
                pixelPerfect.refResolutionY = 768;
            }
        }

        private void Update()
        {
            var cameraRect = _mainCamera.pixelRect;
            var cameraMin = _mainCamera.ScreenToWorldPoint(cameraRect.min);
            var cameraMax = _mainCamera.ScreenToWorldPoint(cameraRect.max);
            _clouds.transform.localScale = new Vector3((cameraMax.x - cameraMin.x), (cameraMax.y - cameraMin.y), 0);
        }
    }
}