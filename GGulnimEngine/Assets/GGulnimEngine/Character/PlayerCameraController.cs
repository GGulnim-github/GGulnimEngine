using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCameraController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Image Image;

    public Vector2 Look;
    public float Zoom;

    private Vector3 _firstPoint;
    private Vector3 _lastPoint;

    private float _xAixRate;
    private float _yAixRate;
    private float _zoomRate;

    private bool _zoomIng = false;
    private List<int> _touchList = new();

    private void Start()
    {
#if UNITY_EDITOR
        Image.raycastTarget = true;
#else
        Image.raycastTarget = false;
#endif

        _xAixRate = 360f / Screen.width;
        _yAixRate = 2f / Screen.height;
        _zoomRate = 20f / Screen.height;
    }
    private void Update()
    {
        if (Input.touchCount == 0)
        {
            _touchList.Clear();
            return;
        }

        for (int i = Input.touchCount-1; i >= 0; i--)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if (!EventSystem.current.currentSelectedGameObject)
                {
                    if (!_touchList.Contains(i))
                    {
                        _touchList.Add(i);
                    }
                }
            }
            else if (Input.GetTouch(i).phase == TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Canceled)
            {
                if (_touchList.Contains(i))
                {
                    _touchList.Remove(i);
                }
                for (int j = 0; j < _touchList.Count; j++)
                {
                    if (_touchList[j] > i)
                    {
                        _touchList[j] -= 1;
                    }
                }
            }
        }

        if (_touchList.Count == 0)
        {
            _zoomIng = false;
        }
        else if (_touchList.Count == 1)
        {
            Touch touchZero = Input.GetTouch(_touchList[0]);
            if (touchZero.phase == TouchPhase.Began)
            {
                _firstPoint = touchZero.position;
            }
            if (touchZero.phase == TouchPhase.Moved)
            {
                if (_zoomIng)
                {
                    _firstPoint = touchZero.position;
                    _zoomIng = false;
                }
                _lastPoint = touchZero.position;

                Look = _lastPoint - _firstPoint;
                Look = new Vector2(Look.x * _xAixRate, Look.y * _yAixRate);

                _firstPoint = _lastPoint;
            }
        }
        else if (_touchList.Count >= 2)
        {
            // Zoom In Out
            _zoomIng = true;
            Touch touchZero = Input.GetTouch(_touchList[0]);
            Touch touchOne = Input.GetTouch(_touchList[1]);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMangitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMangnitude = (touchZero.position - touchOne.position).magnitude;

            float distance = currentMangnitude - prevMangitude;
            Zoom = distance * _zoomRate;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
#if UNITY_EDITOR
        PointerEventData pointerEventData = eventData;
        _firstPoint = pointerEventData.position;

#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
#if UNITY_EDITOR
        PointerEventData pointerEventData = eventData;

        _lastPoint = pointerEventData.position;

        Look = _lastPoint - _firstPoint;
        Look = new Vector2(Look.x * _xAixRate, Look.y * _yAixRate);

        _firstPoint = _lastPoint;
#endif
    }
}
