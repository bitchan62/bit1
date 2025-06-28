using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ͱ� ������ ť�� ������Ʈ
/// ť�� ���� �� �ڵ����� ���� ����
/// </summary>
public class MonsterCube : MonoBehaviour
{
    [Header("���� ���")]
    [Tooltip("������ ť�� ���� (��������� �ڵ����� ã��)")]
    public CubeMover cubeMover;

    [Tooltip("Ȱ��ȭ�� ���� ������ (��������� �ڵ����� ã��)")]
    public MonsterSpawner monsterSpawner;

    [Header("���� ����")]
    [Tooltip("���� �� ��� �ð� (��)")]
    public float delayBeforeSpawn = 0f;

    [Header("�����")]
    [Tooltip("����� �α� ���")]
    public bool showDebugLog = true;

    // ���� ����
    private bool hasSpawnTriggered = false;

    void Start()
    {
        // �ڵ����� ������Ʈ ã��
        if (cubeMover == null)
        {
            cubeMover = GetComponent<CubeMover>();
        }

        if (monsterSpawner == null)
        {
            monsterSpawner = GetComponent<MonsterSpawner>();
        }

        // ������Ʈ Ȯ��
        if (cubeMover == null)
        {
            Debug.LogError($"[{gameObject.name}] CubeMover ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        if (monsterSpawner == null)
        {
            Debug.LogError($"[{gameObject.name}] MonsterSpawner ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        if (showDebugLog)
        {
            Debug.Log($"[{gameObject.name}] MonsterCube �ʱ�ȭ �Ϸ�. ť�� ���� �� ���� ���� ����.");
        }
    }

    void Update()
    {
        // �̹� ���������� üũ���� ����
        if (hasSpawnTriggered)
        {
            return;
        }

        // ť�갡 �����ߴ��� üũ
        if (cubeMover != null && cubeMover.HasArrived)
        {
            if (showDebugLog)
            {
                Debug.Log($"[{gameObject.name}] ť�� ���� ����! ���� ������ �����մϴ�.");
            }

            TriggerSpawn();
            hasSpawnTriggered = true;
        }
    }

    // ���� Ʈ����
    private void TriggerSpawn()
    {
        if (monsterSpawner == null)
        {
            Debug.LogError($"[{gameObject.name}] MonsterSpawner�� ��� ������ �� �����ϴ�!");
            return;
        }

        if (delayBeforeSpawn > 0)
        {
            // �����̰� ������ �ڷ�ƾ���� ó��
            StartCoroutine(DelayedSpawn());
        }
        else
        {
            // ��� ����
            ActivateSpawner();
        }
    }

    // ������ �� ����
    private IEnumerator DelayedSpawn()
    {
        if (showDebugLog)
        {
            Debug.Log($"[{gameObject.name}] {delayBeforeSpawn}�� ��� �� ���� ����...");
        }

        yield return new WaitForSeconds(delayBeforeSpawn);

        ActivateSpawner();
    }

    // ������ Ȱ��ȭ
    private void ActivateSpawner()
    {
        if (monsterSpawner != null)
        {
            monsterSpawner.SpawnTriggerOn();

            if (showDebugLog)
            {
                Debug.Log($"[{gameObject.name}] ���� ������ Ȱ��ȭ �Ϸ�!");
            }
        }
    }

    // ���� ���� Ʈ���� (�׽�Ʈ��)
    public void ManualSpawnTrigger()
    {
        if (hasSpawnTriggered)
        {
            Debug.LogWarning($"[{gameObject.name}] �̹� ������ ����Ǿ����ϴ�.");
            return;
        }

        if (showDebugLog)
        {
            Debug.Log($"[{gameObject.name}] �������� ������ Ʈ�����մϴ�.");
        }

        TriggerSpawn();
        hasSpawnTriggered = true;
    }

    // ���� �ʱ�ȭ (������ ����)
    public void ResetMonsterCube()
    {
        hasSpawnTriggered = false;

        if (showDebugLog)
        {
            Debug.Log($"[{gameObject.name}] MonsterCube ���� �ʱ�ȭ �Ϸ�.");
        }
    }

    // ���� Ȯ�ο� ������Ƽ��
    public bool HasSpawnTriggered
    {
        get { return hasSpawnTriggered; }
    }

    public bool CanTriggerSpawn
    {
        get { return !hasSpawnTriggered && cubeMover != null && monsterSpawner != null; }
    }
}