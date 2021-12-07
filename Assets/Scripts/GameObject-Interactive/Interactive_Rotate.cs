using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interactive_Rotate : MonoBehaviour, IDragHandler, IEndDragHandler
{

    /// <summary>    ///  是否需要自转    /// </summary>
    public bool selfRotate = true;
    /// <summary>    /// 是否需要复位    /// </summary>
    public bool needReset = true;
    [Range(0f, 1f)]
    /// <summary>    ///  旋转速度   /// </summary>
    public float rotateSpeed = 0.2f;
    [Range(0.2f, 1f)]
    /// <summary>    ///  阻尼系数   /// </summary>
    public float dampSpeed = 0.5f;

    [Range(1f, 30f)]
    /// <summary>    ///  复位速度    /// </summary>
    public float resetSpeed =9f;
    /// <summary>    /// 鼠标拖拽速度    /// </summary>
    public float dragSpeed_Mouse = 5f;
    [Range(0f, 1f)]
    /// <summary>    /// 触摸拖拽速度    /// </summary>
    public float dragSpeed_Touch = 0.5f;

    /// <summary>    ///  拖拽状态    /// </summary>
    private bool isDragging = false;
    /// <summary>    /// 阻尼恢复值    /// </summary>
    private float damping;





    /// <summary>    /// 自身旋转方向    /// </summary>
    public SelfRotateDirection selfRotateDirection = SelfRotateDirection.Left;


    /// <summary>    /// 自身旋转方向枚举    /// </summary>
    public enum SelfRotateDirection
    {
        Left,
        Right
    }


    private void Update()
    {

        if (isDragging)
        {
            RotateInput();
        }
        else
        {
            if (damping > 0f)
            {
                damping -= dampSpeed * 100f * Time.deltaTime / cXY; //通过除以鼠标移动长度实现拖拽越长速度减缓越慢// 
                if (axisX < 0f)
                    this.transform.Rotate(Vector3.up * Time.deltaTime * -damping * dampSpeed * 100f);
                else if (axisX > 0f)
                    this.transform.Rotate(Vector3.up * Time.deltaTime * damping * dampSpeed * 100f);
            }
            else
            {
                damping = 0f;

                if (!Mathf.Approximately(this.transform.rotation.x, Quaternion.identity.x) && needReset)
                {
                    // X轴复位
                    if (Mathf.Abs(this.transform.rotation.x - Quaternion.identity.x) < 0.0001f)
                    {
                        this.transform.rotation = Quaternion.identity;
                    }
                    else
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, resetSpeed * Time.deltaTime / 2f);
                    }
                }
                else
                {
                    if (selfRotate && !isDragging && damping == 0f)
                    {
                        // 自身旋转

                        if (selfRotateDirection == SelfRotateDirection.Left)
                            this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * 100f);
                        else if (selfRotateDirection == SelfRotateDirection.Right)
                            this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * -100f);
                    }
                }
            }
        }
       
    

       


    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    axisX = 0f;
    //    axisY = 0f;
    //}

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;

#if UNITY_EDITOR
        damping = rotateSpeed * 100f;
#elif UNITY_ANDROID || UNITY_IOS
         damping = rotateSpeed *5f;
#endif

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }






    private float cXY;
    private float axisX = 0f;
    private float axisY = 0f;
    private float dragSpeed = 0f;
    /// <summary>    /// 旋转输入    /// </summary>
    private void RotateInput()
    {
#if UNITY_EDITOR
        dragSpeed = dragSpeed_Mouse;
        //获得鼠标增量// 
        axisX = -Input.GetAxis("Mouse X");
        axisY = Input.GetAxis("Mouse Y");
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount <= 0) return;
        dragSpeed = dragSpeed_Touch /3f;
        Touch touch = Input.GetTouch(0);
        axisX = -touch.deltaPosition.x;
        axisY = touch.deltaPosition.y;
#endif

        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY);
        if (cXY == 0f) { cXY = 1f; }
        this.transform.Rotate(new Vector3(axisY, axisX, 0) * dragSpeed, Space.World);
    }




}
