using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public class CameraEvent
    {
        public Vector3 point;
        public BaseItemScript baseItem;
    }

    public static CameraManager instance;

    public EventSystem EventSystem;
    [SerializeField] private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }
    private int layerMaskGround;
    private int layerMaskBaseItem;
    private bool isPanningStarted;
    private BaseItemScript selectedBaseItem;
    private BaseItemScript dragStartBaseItem;

    private static Vector3 PositiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    private int previousTouchCount;
    private Vector3 previoursPanPoint;
    private Vector3 panVelocity;
    private float oldZoom;

    private bool pinchStarted;
    private float oldPinchDist;
    private Vector3 touchPoint0;
    private Vector3 touchPoint1;
    private Vector3 tapGroundStartPosition;

    private readonly float maxZoomFactor = 50;
    private readonly float minZoomFactor = 3;
    private readonly float clampZoomOffset = 2.0f;

    public UnityEngine.Events.UnityAction<CameraEvent> OnTapItemAction { get; set; }
    public UnityEngine.Events.UnityAction<CameraEvent> OnTapGroundAction { get; set; }

    private void Awake()
    {
        instance = this;
        layerMaskGround = LayerMask.GetMask("GroundLayer");
        layerMaskBaseItem = LayerMask.GetMask("BaseItemLayer");
        oldZoom = mainCamera.orthographicSize;
        pinchStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsUsingUI())
        {
            return;
        }

        UpdateBaseItemTap();
        if (dragStartBaseItem == default)
        {
            UpdatePan();
            UpdateZoom();
        }
        UpdateGroundTap();
        UpdateBaseItemMove();
    }

    private void UpdateGroundTap()
    {
        if (isPanningStarted)
        {
            return;
        }

        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }


        Vector3 tapPosition = TryGetRaycastHitBaseGround(Input.mousePosition);
        if (tapPosition != PositiveInfinityVector)
        {
            CameraEvent evt = new CameraEvent()
            {
                point = tapPosition
            };
            OnTapGroundAction(evt);
        }
    }

    public bool IsUsingUI()
    {
        //if (this._isDraggingBaseItem)
        //{
        //    return false;
        //}

        if (isPanningStarted)
        {
            return false;
        }

        return EventSystem.IsPointerOverGameObject() || EventSystem.IsPointerOverGameObject(0);
    }

    private void UpdateBaseItemMove()
    {
        var targetBaseGroundPos = TryGetRaycastHitBaseGround(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            dragStartBaseItem = TryGetRaycastHit<BaseItemScript>(Input.mousePosition, layerMaskBaseItem);
            if (dragStartBaseItem != null)
            {
                dragStartBaseItem.OnItemDragStart(targetBaseGroundPos);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragStartBaseItem != null)
            {
                dragStartBaseItem.OnItemDrag(targetBaseGroundPos);
                Debug.DrawLine(mainCamera.transform.position, targetBaseGroundPos);

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragStartBaseItem = default;
        }
        
    }

    private void UpdateBaseItemTap()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }

        if (isPanningStarted)
        {
            return;
        }

        BaseItemScript obj = TryGetRaycastHit<BaseItemScript>(Input.mousePosition, layerMaskBaseItem);
        if (obj != default)
        {
            selectedBaseItem = obj;            
            OnTapItemAction?.Invoke( new CameraEvent() { baseItem = obj, point = Input.mousePosition });
        }
        else
        {
            selectedBaseItem = default;
        }
    }

    void UpdateZoom()
    {
        float newZoom = mainCamera.orthographicSize;
        //Editor
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAmount != 0)
        {
            newZoom = newZoom - scrollAmount;
        }

        if (Input.touchCount == 0 || Input.touchCount == 1)
        {
            pinchStarted = false;
        }

        if (Input.touchCount == 2)
        {
            touchPoint0 = TryGetRaycastHitBaseGround(Input.GetTouch(0).position);
            touchPoint1 = TryGetRaycastHitBaseGround(Input.GetTouch(1).position);
            float pinchDist = Vector3.Distance(touchPoint0, touchPoint1);

            if (!pinchStarted)
            {
                oldPinchDist = pinchDist;
                pinchStarted = true;
            }
            else
            {
                float delta = oldPinchDist - pinchDist;
                newZoom = mainCamera.orthographicSize + delta / 2;
            }
        }

        newZoom = Mathf.Clamp(newZoom, minZoomFactor, maxZoomFactor);

        if (newZoom < minZoomFactor + clampZoomOffset)
        {
            newZoom = Mathf.Lerp(newZoom, this.minZoomFactor + clampZoomOffset, Time.deltaTime * 2);

        }
        else if (newZoom > maxZoomFactor - clampZoomOffset)
        {
            newZoom = Mathf.Lerp(newZoom, this.maxZoomFactor - clampZoomOffset, Time.deltaTime * 2);
        }

        if (oldZoom != newZoom)
        {
            mainCamera.orthographicSize = newZoom;
            oldZoom = newZoom;
        }
    }
     
    private void UpdatePan()
    {
        int touchCount;
        bool isInEditor = false;
        bool touchCountChanged = false;
        bool canPan;

        Vector2 touchPosition;

        if (Input.touchCount == 0)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)))
            {
                touchCount = 1;
                isInEditor = true;
            }
            else
            {
                touchCount = 0;
            }
        }
        else
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchCount = 0;
            }
            else
            {
                touchCount = Input.touchCount;
            }
        }
        
        if (touchCount != previousTouchCount)
        {
            if (touchCount > 0)
            {
                touchCountChanged = true;
            }
        }

        if (isInEditor)
        {
            touchPosition = Input.mousePosition;
        }
        else
        {
            if (touchCount > 0)
            {
                if (touchCount == 1)
                {
                    touchPosition = Input.GetTouch(0).position;
                }
                else
                {
                    touchPosition = (Input.GetTouch(0).position + Input.GetTouch(1).position) / 2.0f;
                }
            }
            else
            {
                touchPosition = Vector2.zero;
            }
        }

        canPan = touchCount > 0;
        previousTouchCount = touchCount;

        if (canPan)
        {
            Vector3 hitPoint = TryGetRaycastHitBaseGround(touchPosition);
            if (touchCountChanged)
            {
                tapGroundStartPosition = hitPoint;
                previoursPanPoint = hitPoint;
            }

            if (!isPanningStarted && (tapGroundStartPosition - hitPoint).magnitude > 2f)
            {
                isPanningStarted = true;
                previoursPanPoint = hitPoint;
            }

            if (isPanningStarted)
            {
                OnScenePan(hitPoint);
            }
        }
        else
        {
            if (isPanningStarted)
            {
                isPanningStarted = false;
                OnScenePanEnded();
            }
            UpdatePanInertia();
        }
    }

    private void OnScenePanEnded()
    {
        Debug.Log($"stop _panVelocity{panVelocity}");
    }

    private void OnScenePan(Vector3 newPoint)
    {
        Vector3 delta = previoursPanPoint - newPoint;
        Debug.Log($"_previous {previoursPanPoint} evtpoint {newPoint} delta {delta}");
        mainCamera.transform.localPosition += delta;
        if(delta.magnitude > 0.1f)
            panVelocity = delta;
        ClampCameara();
    }

    private T TryGetRaycastHit<T>(Vector2 touch, int layerMask)
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(touch), out RaycastHit hit, 1000, layerMask))
        {
            return hit.collider.gameObject.GetComponent<T>();
        }
        return default;
    }
    private Vector3 TryGetRaycastHitBaseGround(Vector2 touch)
    {
        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(touch);

        if (Physics.Raycast(ray, out hit, 1000, layerMaskGround))
        {
            return hit.point;
        }
        else
        {
            return PositiveInfinityVector;
        }
    }

    private void ClampCameara()
    {
        float worldSizePerPixel = 2 * mainCamera.orthographicSize / (float)Screen.height;

        var leftTopClampScreenPos = mainCamera.WorldToScreenPoint(CameraBoundary.instance.CameraClampTopLeftPosition);
        if (leftTopClampScreenPos.x > 0)
        {
            float deltaFactor = leftTopClampScreenPos.x * worldSizePerPixel;
            var delta = new Vector3(deltaFactor, 0, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition += delta;
        }

        if (leftTopClampScreenPos.y < Screen.height)
        {
            float deltaFactor = (Screen.height - leftTopClampScreenPos.y) * worldSizePerPixel;
            var delta = new Vector3(0, deltaFactor, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition -= delta;
        }

        var rightBottomClampScreenPos = mainCamera.WorldToScreenPoint(CameraBoundary.instance.CameraClampBottomRightBotPosition);
        if (rightBottomClampScreenPos.x < Screen.width)
        {
            float deltaFactor = (Screen.width - rightBottomClampScreenPos.x) * worldSizePerPixel;
            var delta = new Vector3(deltaFactor, 0, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition -= delta;
        }

        if (rightBottomClampScreenPos.y > 0)
        {
            float deltaFactor = rightBottomClampScreenPos.y * worldSizePerPixel;
            var delta = new Vector3(0, deltaFactor, 0);
            delta = mainCamera.transform.TransformVector(delta);
            mainCamera.transform.localPosition += delta;
        }
    }

    public void UpdatePanInertia()
    {
        if (panVelocity.magnitude < 0.05f)
        {
            panVelocity = Vector3.zero;
        }
    
        if (panVelocity != Vector3.zero)
        {
            panVelocity = Vector3.Lerp(panVelocity, Vector3.zero, Time.deltaTime * 5);
            mainCamera.transform.localPosition += panVelocity;
            ClampCameara();
        }
    }

}
