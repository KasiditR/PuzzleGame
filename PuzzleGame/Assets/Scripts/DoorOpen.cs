using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Transform obj;
    [SerializeField] private float open;
    [SerializeField] private float close;
    [SerializeField] private float duration;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Open");
        obj.DOLocalMoveY(open, duration, false);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Close");
        obj.DOLocalMoveY(close, duration, false);
    }
}
