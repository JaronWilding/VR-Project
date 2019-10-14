using HTC.UnityPlugin.PoseTracker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    private Button onGrabButton;
    private Button onReleaseButton;

    private Renderer _ren;
    private Material _mat;
    private Vector3 _tran;
    [SerializeField] private GameObject _pref;
    [SerializeField] private Listeners listen;
    private PoseTracker poseTracker;
    private Draggable drag;
    // Start is called before the first frame update
    void Start()
    {
        _ren = GetComponent<Renderer>();
        _tran = transform.position;
        onGrabButton = listen.onGrabButton;
        onReleaseButton = listen.onReleaseButton;

        onGrabButton.onClick.AddListener(Grabbed);
        onReleaseButton.onClick.AddListener(Released);
        poseTracker = GetComponent<PoseTracker>();
        drag = GetComponent<Draggable>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.button);
        //Debug.Log(eventData.);
    }

    public void Grabbed()
    {
        _ren.material.color = Color.red;
        OnDrag(drag.currentGrabber.eventData);
        //poseTracker.target = 

    }



    public void Released()
    {
        _ren.material.color = Color.white;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "HitPoint")
        {   
            Instantiate(_pref, _tran, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
