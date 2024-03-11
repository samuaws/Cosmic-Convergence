using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsControler : MonoBehaviour
{
    public Vector3 originalPosition;
    public Vector3 supernovaPosition;
    public Vector3 blackholePosition;
    public Vector3 lazerPosition;
    public void SupernovaHands()
    {
        transform.localPosition = supernovaPosition;
    }
    public void BlackholeHands()
    {
        transform.localPosition = blackholePosition;
    }
    public void LazeerHands()
    {
        transform.localPosition = lazerPosition;
    }
    public void ResetPosition()
    {
        transform.localPosition = originalPosition;
    }
}
