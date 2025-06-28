using UnityEngine;

/// <summary>
/// ť�� �ر��� ���� ������ Ʈ���� ������Ʈ
/// �÷��̾ ������ ������Ʈ�� Ʈ���� ������ ������ ����� ť�긦 �ر���Ŵ
/// </summary>
[RequireComponent(typeof(Collider))]
public class CubeCollapseTrigger : MonoBehaviour
{
    [Header("�ر� ����")]
    [Tooltip("�ر���ų ť�� (CubeCollapser ������Ʈ�� �־�� ��)")]
    public CubeCollapser targetCube;

    [Tooltip("�ر��� Ʈ������ ������Ʈ�� �±�")]
    public string triggerTag = "Player";

    [Tooltip("�� ���� �۵��ϴ��� ���� (true: �� �� �۵� �� ��Ȱ��ȭ)")]
    public bool oneTimeUse = true;

    [Header("����� ����")]
    [Tooltip("Ʈ���� ������ ������ �ð�ȭ")]
    public bool showTriggerArea = true;

    // ���� ����
    private bool hasTriggered = false;
    private Collider triggerCollider;

    void Awake()
    {
        // �ݶ��̴� ���� Ȯ��
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            // Ʈ���ŷ� ����
            triggerCollider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] CubeCollapseTrigger�� Collider�� �����ϴ�!");
        }

        // Ÿ�� ť�� ����
        if (targetCube == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Ÿ�� ť�갡 �������� �ʾҽ��ϴ�.");
        }
    }

    void Start()
    {
        // ���� �� ���� �α�
        Debug.Log($"[{gameObject.name}] ť�� �ر� Ʈ���� �غ� �Ϸ�. " +
                 $"Ÿ��: {(targetCube != null ? targetCube.name : "����")}, " +
                 $"Ʈ���� �±�: {triggerTag}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // �̹� Ʈ���ŵǾ��� ��ȸ���̸� ����
        if (hasTriggered && oneTimeUse)
        {
            return;
        }

        // �±� Ȯ��
        if (!other.CompareTag(triggerTag))
        {
            return;
        }

        // Ÿ�� ť�� Ȯ��
        if (targetCube == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Ÿ�� ť�갡 �������� �ʾ� �ر��� ������ �� �����ϴ�.");
            return;
        }

        // �ر� Ʈ����
        Debug.Log($"[{gameObject.name}] {other.name}��(��) Ʈ���� ������ �����߽��ϴ�. " +
                 $"ť�� '{targetCube.name}' �ر��� �����մϴ�.");

        targetCube.TriggerCollapse();
        hasTriggered = true;

        // ��ȸ���� ��� ��Ȱ��ȭ
        if (oneTimeUse)
        {
            Debug.Log($"[{gameObject.name}] ��ȸ�� Ʈ���Ű� �۵��߽��ϴ�. Ʈ���Ÿ� ��Ȱ��ȭ�մϴ�.");
            triggerCollider.enabled = false;
        }
    }

    // Ʈ���� �ʱ�ȭ (������ ����)
    public void ResetTrigger()
    {
        hasTriggered = false;
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }
        Debug.Log($"[{gameObject.name}] Ʈ���Ű� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // Ÿ�� ť�� ����
    public void SetTargetCube(CubeCollapser newTarget)
    {
        targetCube = newTarget;
        Debug.Log($"[{gameObject.name}] Ÿ�� ť�갡 '{(newTarget != null ? newTarget.name : "����")}'�� ����Ǿ����ϴ�.");
    }

    // �������� �ر� Ʈ���� (�ٸ� ��ũ��Ʈ���� ȣ�� ����)
    public void ManualTrigger()
    {
        if (targetCube != null && (!hasTriggered || !oneTimeUse))
        {
            Debug.Log($"[{gameObject.name}] ���� Ʈ���ŷ� ť�� '{targetCube.name}' �ر��� �����մϴ�.");
            targetCube.TriggerCollapse();
            hasTriggered = true;

            if (oneTimeUse && triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
        }
    }

    // ����׿�: ������ Ʈ���� ���� �ð�ȭ
    void OnDrawGizmos()
    {
        if (!showTriggerArea) return;

        Collider col = GetComponent<Collider>();
        if (col == null) return;

        // Ʈ���� ���¿� ���� ���� ����
        if (hasTriggered && oneTimeUse)
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f); // ȸ�� (��Ȱ��ȭ��)
        }
        else
        {
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f); // ������ (Ȱ��ȭ��)
        }

        // �ݶ��̴� Ÿ�Կ� ���� �ٸ��� �׸���
        if (col is BoxCollider)
        {
            BoxCollider boxCol = col as BoxCollider;
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(boxCol.center, boxCol.size);
            Gizmos.matrix = oldMatrix;
        }
        else if (col is SphereCollider)
        {
            SphereCollider sphereCol = col as SphereCollider;
            Gizmos.DrawSphere(transform.position + sphereCol.center,
                             sphereCol.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
        }
        else
        {
            // ��Ÿ �ݶ��̴��� �ܼ� ���̾�����������
            Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }

        // Ÿ�� ť�갡 ������ ���ἱ �׸���
        if (targetCube != null)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.5f); // ����� ���ἱ
            Gizmos.DrawLine(transform.position, targetCube.transform.position);

            // Ÿ�� ť�� ��ġ�� ���� ǥ��
            Gizmos.color = new Color(1f, 0f, 0f, 0.8f); // ������
            Gizmos.DrawWireCube(targetCube.transform.position, Vector3.one * 0.2f);
        }
    }
}