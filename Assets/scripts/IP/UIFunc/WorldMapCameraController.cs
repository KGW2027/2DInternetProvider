using IP.Objective;
using UnityEngine;

namespace IP.UIFunc
{
    public class WorldMapCameraController : MonoBehaviour
    {
        [Header("월드맵")]
        public GameObject mapCamera;
        public GameObject map;
        [Header("카메라 속성")]
        public float camMoveSpeed = 1.0f;
        public float camZoomSpeed = 3.0f;

        private Camera _mainCamera;
        private Camera _camCamera;
    
        private Vector2 _onClicked;
        private Vector3 _onClickedCamVector;
        private bool _moveCameraMode;
        private float[] _camBorder;
        private float[] _camScale;
        private WorldMapInteraction _wmi;

        private const float MaxFocal = 50.0f;
        private const float MinFocal = 3.0f;

        void Start()
        {
        
            _mainCamera = Camera.main;

            Bounds mapBounds = map.GetComponent<SpriteRenderer>().bounds;
            var centerX = mapBounds.center.x;
            var centerY = mapBounds.center.y;
            var extentX = mapBounds.extents.x;
            var extentY = mapBounds.extents.y;
            _camBorder = new[] {centerX + extentX, centerY + extentY, centerX - extentX, centerY - extentY};
            
            _camCamera = mapCamera.GetComponent<Camera>();
            UpdateCamScale();
            _wmi = FindObjectOfType<WorldMapInteraction>().GetComponent<WorldMapInteraction>();
            _wmi.ChangeVisibleMode(true);
        }
    
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _wmi.ClickMap(_camCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            
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
                float orthoSize = _camCamera.orthographicSize;
                if ((orthoSize <= MinFocal && wheel < 0.0f) ||
                    (orthoSize >= MaxFocal && wheel > 0.0f)) return;
            
                // 화면 크기가 Min 일때 0.4, Max일때 8
                float ratio = (orthoSize - MinFocal) / MaxFocal;
                camMoveSpeed = Mathf.Lerp(0.4f, 8.0f, ratio);
                _wmi.ChangeVisibleMode(orthoSize <= 23);
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

        /**
         * 카메라 포커스를 조정할 때 사용하는 함수
         */
        public void SetCameraFocus(Vector2 vec)
        {
            mapCamera.transform.position = new Vector3(vec.x, vec.y, -10);
        }
    }
}