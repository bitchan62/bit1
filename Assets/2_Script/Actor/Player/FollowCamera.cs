using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] protected Transform target; // ���� Ÿ��(�÷��̾�)
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected Vector3 rotation;
    
    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(rotation);
    }
}