using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneManager : MonoBehaviour
{
    private float _score = 0f;
    public void AddScore()
    {
        _score++;
        Debug.Log(_score);
    }
}
