using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class CSceneManager : MonoBehaviour
{
    [SerializeField] public int _playerScore = 0;
    [SerializeField] private float _timeAmount;

    [SerializeField] private GameObject _winPrefab;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _scoreScreen;

    [SerializeField] private Transform _axes;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _playerScoreText;
    
    private List<GameObject> _axeList;
    private List<Vector3> _axeListPos;
    private List<Quaternion> _axeListQuat;
    private float t = 0;
    private float _timeAmountOld = 0;
    float _canvasTimer = 0;
    private bool GameStart;

    private void Start()
    {
        _axeList = new List<GameObject>();
        _axeListPos = new List<Vector3>();
        _axeListQuat = new List<Quaternion>();
        for (int ii = 0; ii < _axes.childCount; ii++)
        {
            _axeList.Add(_axes.GetChild(ii).gameObject);
            _axeListPos.Add(_axes.GetChild(ii).transform.position);
            _axeListQuat.Add(_axes.GetChild(ii).transform.rotation);
        }
        Time.timeScale = 0;
        _timeAmountOld = _timeAmount;
    }

    public void SceneChange(GameObject currentCanvas, GameObject loadCanvas, bool start, bool quit)
    {
        GameStart = start;
        if (GameStart)
        {
            SetAxes();
        }
        if (quit)
        {
            Quit();
        }
        currentCanvas.SetActive(false);
        loadCanvas.SetActive(true);
    }
    private void SetAxes()
    {
        Time.timeScale = 1;
        for (int ii = 0; ii < _axeList.Count; ii++)
        {
            _axeList[ii].GetComponent<GrabbableObject>().enabled = true;
            _axeList[ii].transform.position = _axeListPos[ii];
            _axeList[ii].transform.rotation = _axeListQuat[ii];
        }
    }

    private void GameOver()
    {
        foreach (GameObject axe in _axeList)
        {
            axe.GetComponent<GrabbableObject>().enabled = false;
        }
        Time.timeScale = 0;
        GameStart = false;
        _playerScore = 0;
        _timeAmount = _timeAmountOld;
        _timerText.color = Color.white;
        _scoreScreen.SetActive(false);
        _gameOver.SetActive(true);

    }

    private void WinGame()
    {
        foreach (GameObject axe in _axeList)
        {
            axe.GetComponent<GrabbableObject>().enabled = false;
        }
        _playerScore = 0;
        GameStart = false;
        Time.timeScale = 0;
        _timeAmount = _timeAmountOld;
        _timerText.color = Color.white;
        Instantiate(_winPrefab);
        _scoreScreen.SetActive(false);
        _winScreen.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }



    private void Update()
    {
        if (GameStart)
        {
            if (_timeAmount <= 11)
            {
                if (t < 1) { t += Time.deltaTime / 1f; }
                _timerText.color = Color.Lerp(Color.white, Color.red, t);
            }
            if(_timeAmount >= 0f)
            {
                _timeAmount -= Time.deltaTime;
                _timerText.text = string.Format("TIME: {0}", (int)_timeAmount);
            }
            else
            {
                _timerText.text = string.Format("GAME OVER");
                GameOver();
            }
        }

    }

    public void AddScore(int _score)
    {
        
        _playerScore += _score;
        _playerScoreText.text = string.Format("SCORE: {0}", _playerScore);
        if(_playerScore >= 100)
        {
            WinGame();
        }
    }
}
// https://answers.unity.com/questions/1242481/how-to-check-if-a-component-exists-on-a-gameobject.html
// Code can be found here
public static class hasComponent
{
    public static bool HasComponent<T>(this GameObject flag) where T : Component
    {
        return flag.GetComponent<T>() != null;
    }
}
