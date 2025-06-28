using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� Ʈ���ŷ� ����� ������Ʈ�� ���̴� ������Ʈ
/// </summary>
public class TriggerArea : MonoBehaviour
{
    [Tooltip("�� Ʈ���Ÿ� ������ CubeController")]
    public CubeController controller;

    // Ʈ���� ������ �������� �� ȣ���
    private void OnTriggerEnter(Collider other)
    {
        if (controller != null)
        {
            controller.OnAreaTrigger(gameObject, other.gameObject);
        }
    }
}