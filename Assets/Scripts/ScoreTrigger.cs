using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private ScoreGiver _scorer;
    private void Start()
    {
        _scorer = GetComponentInParent<ScoreGiver>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Free Grab")
        {
            _scorer.AxeEntered();
        }
    }
}
