using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.AI.Navigation;
using UnityEngine;


[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshManager : MonoBehaviour
{
    // �̱���
    public static NavMeshManager instance = null;

    // �׺���̼�
    NavMeshSurface surface = null;

    private void Awake()
    { 
        // �̱��� �ʱ�ȭ
        if (instance == null)
        { instance = this; }
        else { Destroy(this.gameObject); return; }

        // NavMeshSurface ����
        surface = GetComponent<NavMeshSurface>();
        if(surface == null) { Debug.Log("NavMeshSurface�� �������� ���� : " + gameObject.name); }
        surface.collectObjects = CollectObjects.Children;

        Rebuild();
    }

    // ���� ����
    public void Rebuild()
    {
        surface.BuildNavMesh();
        StartCoroutine(Timer.StartTimer(0.1f, Rebuild)); // <- �׽�Ʈ : 0.1f���� ���� ����
    }
}