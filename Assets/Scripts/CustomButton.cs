using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    private CSceneManager _sceneManager;
    [SerializeField] private GameObject _nextScene;
    [SerializeField] private bool _startGame;
    [SerializeField] private bool _quitGame;
    private void Start()
    {
        _sceneManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CSceneManager>();
    }
    

    public void ChangeScene()
    {
        _sceneManager.SceneChange(transform.parent.gameObject, _nextScene, _startGame, _quitGame);
    }
}
