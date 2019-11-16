using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreGiver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _scoreAmount;

    private Camera _cam;
    private CSceneManager _sceneManager;

    private void Start()
    {
        _sceneManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CSceneManager>();
        _cam = Camera.main;
        _text.text = string.Format("Points: {0}", _scoreAmount);
        
    }

    private void Update()
    {
        _canvas.transform.LookAt(_cam.transform);

        Vector3 dir1 = _canvas.transform.position - _cam.transform.position;
        Vector3 dir2 = _cam.transform.position - ( _cam.transform.position + _cam.transform.forward * 200f );

        float finDot = Vector3.Dot(dir1.normalized, dir2.normalized);
        Color colour = new Color(_text.color.r, _text.color.g, _text.color.b, -finDot);

        _text.color = colour;
    }

    public void AxeEntered()
    { 
        _sceneManager.AddScore(_scoreAmount);
    }
    
}
