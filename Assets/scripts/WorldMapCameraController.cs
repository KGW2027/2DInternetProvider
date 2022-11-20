using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCameraController : MonoBehaviour
{
    public GameObject mapCamera;
    public GameObject map;
    public float camMoveSpeed = 1.0f;
    public float camZoomSpeed = 3.0f;

    private Camera _mainCamera;
    private Camera _camCamera;
    
    private Vector2 _onClicked;
    private Vector3 _onClickedCamVector;
    private bool _moveCameraMode;
    private float[] _camBorder;
    private float[] _camScale;

    private const float MaxFocal = 15.0f;
    private const float MinFocal = 3.0f;

    void Start()
    {
        _mainCamera = Camera.main;
        Vector3 mapImageCenter = map.transform.position;
        mapImageCenter.z = -10;
        mapCamera.transform.position = mapImageCenter;

        _camCamera = mapCamera.GetComponent<Camera>();
        UpdateCamScale();
        
        Bounds mapBounds = map.GetComponent<SpriteRenderer>().bounds;
        float centerX = mapBounds.center.x;
        float centerY = mapBounds.center.y;
        float extentX = mapBounds.extents.x;
        float extentY = mapBounds.extents.y;
        _camBorder = new[] {centerX + extentX, centerY + extentY, centerX - extentX, centerY - extentY};
    }
    
    void Update()
    {
        if (!_moveCameraMode && Input.GetMouseButtonDown(1))
        {
            _onClickedCamVector = mapCamera.transform.position;
            _onClicked = GetMousePosition();
            
            _moveCameraMode = true;
        }
        else if (_moveCameraMode)
        {
            if (Input.GetMouseButtonUp(1))
            {
                _moveCameraMode = false;
                
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 buffer = mapCamera.transform.position;
                mapCamera.transform.position = _onClickedCamVector + (Vector3) ((_onClicked - GetMousePosition()) * camMoveSpeed);
                if (!BorderTest(mapCamera.transform.position))
                {
                    mapCamera.transform.position = buffer;
                }
            }
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel") * camZoomSpeed * -1;
        if (wheel != 0)
        {
            if ((_camCamera.orthographicSize <= MinFocal && wheel < 0.0f) ||
                (_camCamera.orthographicSize >= MaxFocal && wheel > 0.0f)) return;
            _camCamera.orthographicSize += wheel;
            UpdateCamScale();
        }
    }

    private Vector2 GetMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool BorderTest(Vector2 tv)
    {
        return _camBorder[2] + _camScale[0] < tv.x && tv.x <= _camBorder[0] - _camScale[0] &&
               _camBorder[3] + _camScale[1] < tv.y && tv.y <= _camBorder[1] - _camScale[1];
    }

    private void UpdateCamScale()
    {
        float orthoSize = _camCamera.orthographicSize;
        float unitSize = _camCamera.pixelWidth / (float)_camCamera.pixelHeight;
        _camScale = new[] {unitSize * orthoSize, orthoSize};
    }
}