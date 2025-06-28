using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetManager : MonoBehaviour
{
    public static TargetManager instance { get; private set; }

    public Transform target { get; private set; }

    [Header("Ÿ�� ����")]
    [SerializeField] private string targetTag = "Target";
    [SerializeField] private string targetLayerName = "Target";
    [SerializeField] private LayerMask targetLayerMask;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else
        {
            Destroy(gameObject);
            Debug.Log("TargetManager �ټ� ����");
            return;
        }
    }

    void Start()
    { Targeting(); }

    // Ÿ����
    public Transform Targeting()
    {
        // string���� �Է¹��� ���̾� �̸��� int�� ��ȯ
        int targetLayer = LayerMask.NameToLayer(targetLayerName);

        // ���� targetLayerMask�� �����Ǿ� ���� �ʴٸ�, targetLayerName�� ������� ����
        if (targetLayerMask.value == 0 && targetLayer >= 0)
        { targetLayerMask = 1 << targetLayer; }

        // �±׷� �ĺ����� ���� ã��
        GameObject[] candidates = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (var obj in candidates)
        {
            // LayerMask�� bit �������� üũ
            if (((1 << obj.layer) & targetLayerMask.value) != 0)
            {
                target = obj.transform;
                break;
            }
        }

        return target;
    }
}
