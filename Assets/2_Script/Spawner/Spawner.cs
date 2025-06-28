using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//====================
// �پ��� ���� ���ǰ� ����� ������ �� �ִ� �߻� Ŭ����
// �ڽ� Ŭ�������� SpawnTrigger()�� �������̵��Ͽ� ���� ���� ����
//====================
abstract public class Spawner : MonoBehaviour
{
    // ������ ������Ʈ (������ �����յ�� ��ġ ����)
    [SerializeField] protected List<GameObject> targetPrefabs = new List<GameObject>(); // ���������� ������ ������ ���

    // ������ �������� �ε��� (�� ��° ����������)
    private int _prefabIndex = 0;
    public int PrefabIndex
    {
        get { return _prefabIndex; }
        protected set
        {
            // null �˻� && �ε��� �˻�
            if (targetPrefabs != null && 0 <= value && value < targetPrefabs.Count)
            { _prefabIndex = value; }
        }
    }


    // ===== ���� ���� ���� ���� =====

    // ���� ���� ���� ���� (true�� ����� == ����)
    protected bool spawnTrigger = false;

    // ���� ���� ���� / Ʈ���� �ѱ�
    public virtual void SpawnTriggerOn()
    { spawnTrigger = true; }

    // ���� Ʈ���� ����
    public virtual void SpawnTriggerOFF()
    { spawnTrigger = false; }



    // ===== �Ϸ� / ��Ȱ�� =====

    // ������ �Ϸ� ���� Ȯ��
    protected bool isCompleted = false;

    // ������ �Ϸ� ����
    public virtual void CheckCompleted()
    { isCompleted = true; }

    // ������ �ʱ�ȭ (��Ȱ��ȭ)
    public virtual void ResetSpawner()
    {
        PrefabIndex = 0;
        isCompleted = false;
    }



    // ===== ���� / ��ġ ���� =====

    // ������Ʈ�� ������ ��ġ
    protected Vector3 spawnLocation;

    // ������Ʈ ����
    protected virtual void SpawnObject()
    {
        if (targetPrefabs.Count < 0) { Debug.Log("������ ������ �ε��� �������"); return; }
        // ���� �ε����� ������, ������ ��ġ, �⺻ ȸ�������� ����
        Instantiate(targetPrefabs[PrefabIndex], spawnLocation, Quaternion.identity);

        // ���� ������ �ε��� ����
        PrefabIndex += 1;
    }

    // ���� ��ġ ����
    public virtual void SetSpawnLocation()
    { spawnLocation = transform.position; }
}