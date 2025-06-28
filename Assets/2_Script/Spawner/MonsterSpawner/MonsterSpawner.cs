using UnityEngine;
using System.Collections;
public class MonsterSpawner : Spawner
{
    [Header("���� ��ġ ����")]
    [Tooltip("ť�� ������ �߰��� ���� �Ÿ� (�⺻: 0.5����)")]
    public float heightOffset = 0.5f;

    // �ʱ�ȭ
    protected void Start()
    {
        targetCollider = GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogError("�ݶ��̴� �������� ���� : " + gameObject.name);
            return;
        }

        // �ڵ� ���� ���� - MonsterCube���� ȣ���� ������ ���
        Debug.Log($"[{gameObject.name}] MonsterSpawner �ʱ�ȭ �Ϸ�. �ܺ� ȣ�� ��� ��...");
    }


    // ===== ���� ��ġ =====
    // ���� ������Ʈ�� �ݶ��̴�
    protected Collider targetCollider;

    // ���� �߾� ��� (���� �ݶ��̴��� ����)
    public override void SetSpawnLocation()
    {
        // ���� �ݶ��̴����� ��� �����ؼ� ���� ���߾� ���
        Bounds combinedBounds = GetCombinedBoundsFromChildren();
        Vector3 topCenter = combinedBounds.center + Vector3.up * combinedBounds.extents.y;

        // �߰� ���� ������ ����
        spawnLocation = topCenter + Vector3.up * heightOffset;

        Debug.Log($"[{gameObject.name}] ���� �ݶ��̴� ��� ���� ��ġ ����: {spawnLocation}");
    }

    // ���� ������Ʈ���� ��� �ݶ��̴� ������ ���ļ� ���
    private Bounds GetCombinedBoundsFromChildren()
    {
        // ��� ���� �ݶ��̴� �������� (�ڱ� �ڽ� ����)
        Collider[] allColliders = GetComponentsInChildren<Collider>();

        if (allColliders.Length == 0)
        {
            // �ݶ��̴��� ������ Transform �������� �⺻ ũ�� ���
            Debug.LogWarning($"[{gameObject.name}] ���� �ݶ��̴��� ã�� �� �����ϴ�. Transform ũ�⸦ ����մϴ�.");
            return new Bounds(transform.position, transform.lossyScale);
        }

        // ù ��° �ݶ��̴��� �ʱ� ���� ����
        Bounds combinedBounds = allColliders[0].bounds;

        // ������ �ݶ��̴��� ���� ��� ��ġ��
        for (int i = 1; i < allColliders.Length; i++)
        {
            combinedBounds.Encapsulate(allColliders[i].bounds);
        }

        Debug.Log($"[{gameObject.name}] ���� �ݶ��̴� {allColliders.Length}���� ���� ��� �Ϸ�. ũ��: {combinedBounds.size}");

        return combinedBounds;
    }

    // ===== Ʈ���� / ���� / �Ϸ� =====
    // ���� �ֱ�
    [SerializeField] protected float spawnRate = 2f;
    // 1. ������ Ȱ��ȭ (MonsterCube���� ȣ��)
    // 2. ���� ��ġ ����
    // 3. ���� ����
    public override void SpawnTriggerOn()
    {
        Debug.Log($"[{gameObject.name}] MonsterSpawner Ȱ��ȭ��! ������ �����մϴ�.");
        base.SpawnTriggerOn();
        SetSpawnLocation(); // ���� ��ġ �缳�� (���� �ݶ��̴� ���)
        SpawnObject();
    }


    // ������ ������ų�� ����
    [SerializeField] bool isEndlessSpawn = false;

    // ����
    protected override void SpawnObject()
    {
        // ���� Ʈ���Ű� �����ִٸ�
        if (spawnTrigger)
        {
            // ������Ʈ ����
            base.SpawnObject();

            // ���� üũ
            CheckCompleted();

            // ������� �ʾҴٸ� : ���� ���� ����
            if (!isCompleted) { StartCoroutine(Timer.StartTimer(spawnRate, SpawnObject)); }
        }
    }

    // ���� Ȯ��
    public override void CheckCompleted()
    {
        // ��� �������� �����ߴٸ�
        if (targetPrefabs.Count <= PrefabIndex + 1)
        {
            Debug.Log($"[{gameObject.name}] ���� ���� �Ϸ�");
            base.CheckCompleted();

            // �ֱ��� �����ʶ��: ���� �߻�
            if (isEndlessSpawn) { ResetSpawner();}
        }
    }
}