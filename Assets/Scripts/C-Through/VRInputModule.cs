using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRInputModule : BaseInputModule
{
    public Camera cam;

    protected PointerEventData p_Data;
    private GameObject p_CurrentObject;

    protected override void Awake()
    {
        base.Awake();

        p_Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // Reset the data
        p_Data.Reset();
        p_Data.position = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);

        // Raycast

        eventSystem.RaycastAll(p_Data, m_RaycastResultCache);
        p_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        p_CurrentObject = p_Data.pointerCurrentRaycast.gameObject;

        // Clear
        m_RaycastResultCache.Clear();

        // Hover
        HandlePointerExitAndEnter(p_Data, p_CurrentObject);

       
    }

    public PointerEventData GetData()
    {
        return p_Data;
    }

    protected void ProcessPress(PointerEventData data)
    {
        p_Data.pointerPressRaycast = p_Data.pointerCurrentRaycast;

        p_Data.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(p_Data.pointerPressRaycast.gameObject);

        ExecuteEvents.Execute(p_Data.pointerPress, p_Data, ExecuteEvents.pointerDownHandler);
    }
    protected void ProcessRelease(PointerEventData data)
    {
        GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(p_Data.pointerCurrentRaycast.gameObject);

        if (p_Data.pointerPress == pointerRelease)
            ExecuteEvents.Execute(p_Data.pointerPress, p_Data, ExecuteEvents.pointerClickHandler);

        ExecuteEvents.Execute(p_Data.pointerPress, p_Data, ExecuteEvents.pointerUpHandler);

        p_Data.pointerPress = null;

        p_Data.pointerCurrentRaycast.Clear();
    }
}
