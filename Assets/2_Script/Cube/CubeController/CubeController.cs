using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events; // �̺�Ʈ �ý��� ����� ���� �߰�

/// <summary>
/// ť�� Ȱ��ȭ�� Ʈ���Ÿ� �����ϴ� ������Ʈ
/// </summary>
public class CubeController : MonoBehaviour
{
    // ���� Ʈ���� -> �������� �˻� (������ �۵��� ������)
    // �ð� Ʈ���� -> ������ �˻� (�� ������Ʈ����)


    // -------------------- �ʱ�ȭ --------------------

    void Start()
    {
        // ���� ������ ����� �ӽ� ����
        float tempTime = 0;


        // ���� �� ��� ť�� Ȯ��
        foreach (var data in activationSettings)
        {
            // ----- ť�깫�� ������Ʈ �ֵ� -----
            if (data.targetCube != null)
            { CheckAndAddMoverComponent(data.targetCube); }


            // ----- ���� �ð� ������ �ο� -----
            if (0 < sharingDelayTime)
            {
                if (data.triggerType == TriggerType.TimeTrigger)
                {
                    tempTime += data.delayTime;
                    tempTime += sharingDelayTime;
                    data.delayTime = tempTime;
                }
            }
        } // foreach
    }


    // ť�꿡 CubeMover ������Ʈ�� �ִ��� Ȯ���ϰ� ������ �߰�
    // MoverAdder
    private void CheckAndAddMoverComponent(GameObject cube)
    {
        CubeMover mover = cube.GetComponent<CubeMover>();
        if (mover == null)
        {
            Debug.LogWarning($"ť�� '{cube.name}'�� CubeMover ������Ʈ�� �����ϴ�. �ڵ����� �߰��˴ϴ�.");
            cube.AddComponent<CubeMover>();
        }
    }



    // -------------------- ��Ʈ�ѷ� Ʈ���� --------------------

    // �� ��Ʈ�ѷ��� �Ϸ�� �� Ȱ��ȭ�� ���� ť�� ��Ʈ�ѷ�
    // CubeControllerManager�� ���� ������
    [HideInInspector] public CubeController nextController;

    // ���� ��Ʈ�ѷ��� Ȱ��ȭ�ϴ� Ʈ���� �̺�Ʈ
    [HideInInspector] public UnityEvent nextCubeControllerActivate;

    // Ȱ��ȭ Ȯ��
    private bool isActivated = false;


    // ���� ��Ʈ�ѷ� Ȱ��ȭ �޼���
    public void ActivateNextController()
    {
        if (nextController != null)
        {
            Debug.Log($"[{gameObject.name}] ���� ��Ʈ�ѷ� [{nextController.gameObject.name}]�� Ȱ��ȭ�մϴ�.");
            nextController.StartController();
        }
        else
        { Debug.Log($"[{gameObject.name}] ���� ��Ʈ�ѷ��� �������� �ʾҽ��ϴ�."); }
    }

    // ��Ʈ�ѷ� ���� �޼���
    public void StartController()
    {
        isActivated = true;
        Debug.Log($"[{gameObject.name}] ��Ʈ�ѷ� Ȱ��ȭ��");
    }




    // -------------------- ť�� Ʈ���� --------------------

    // Ʈ���� ���� Ÿ�� ����
    public enum TriggerType
    {
        TimeTrigger,  // �ð� Ʈ����: ���� �ð� ��� �� ������Ʈ Ȱ��ȭ
        AreaTrigger,  // ���� Ʈ����: Ư�� ������ �÷��̾ ������ Ȱ��ȭ
        Manual        // ���� Ʈ����: �ڵ忡�� ���� ȣ���Ͽ� Ȱ��ȭ
    }


    // ���� Ʈ���� ���� �� ȣ���
    public void OnAreaTrigger(GameObject triggerArea, GameObject other)
    {
        if (!isActivated) { return; }

        // �� Ȱ��ȭ �����͸� Ȯ��
        foreach (var data in activationSettings)
        {
            // �̹� Ȱ��ȭ�� ť��� ��ŵ
            if (data.hasActivated) continue;

            // ���� Ʈ���� �����̰� ������ �±װ� ��ġ�ϴ��� Ȯ��
            if (data.triggerType == TriggerType.AreaTrigger &&
                data.triggerArea == triggerArea &&
                other.CompareTag(data.targetTag))
            {
                ActivateCube(data);
            }
        }
    }


    // ��� ť�갡 Ȱ��ȭ�Ǿ����� Ȯ��
    private void CheckAllCubesActivated()
    {
        // ��� ť�갡 Ȱ��ȭ�Ǿ����� Ȯ��
        foreach (var data in activationSettings)
        {
            // �ϳ��� ��Ȱ��ȭ ���¶��
            // ����
            if (!data.hasActivated) { return; }
        }

        // <- activatedCubeCount�� activationSettings.count �� �񱳷� �ٲٱ�
        //   if (activatedCubeCount < activationSettings.Count) { return; }


        // ��� ť�갡 Ȱ��ȭ�Ǿ����� �̺�Ʈ �߻�
        if (activationSettings.Count > 0)
        {
            Debug.Log($"[{gameObject.name}] ��� ť�갡 Ȱ��ȭ�Ǿ����ϴ�. �̺�Ʈ�� �߻���ŵ�ϴ�.");

            // ���� ��Ʈ�ѷ� Ȱ��ȭ �̺�Ʈ �߻�
            nextCubeControllerActivate?.Invoke();
        }
    }



    // �� �����Ӹ��� �ð� Ʈ���� üũ
    void Update()
    {
        // Ȱ��ȭ üũ
        if (!isActivated) { return; }

        // Ȱ��ȭ ���¶��, ť�� Ȱ��ȭ ���� ó��
        foreach (var data in activationSettings)
        {
            // �̹� Ȱ��ȭ�� ť��� ��ŵ
            if (data.hasActivated) { continue; }

            // �ð� Ʈ���� ó��
            if (data.triggerType == TriggerType.TimeTrigger)
            {
                data.timer += Time.deltaTime;
                if (data.timer >= data.delayTime)
                { ActivateCube(data); }
            }
        }

        // ��� ť�� Ȱ��ȭ üũ
        CheckAllCubesActivated();
    }




    // -------------------- ���� ���� ���� --------------------

    [Tooltip("ť��� ť�� ���� ��� ����")]
    public float sharingDelayTime = 0f;



    // -------------------- Ȱ��ȭ --------------------

    // ť�� Ȱ��ȭ ������ �����ϴ� Ŭ����
    [System.Serializable]
    public class CubeData
    {
        [Header("������Ʈ ����")]
        [Tooltip("Ȱ��ȭ�� ť��")]
        public GameObject targetCube;

        [Header("Ʈ���� ����")]
        [Tooltip("Ȱ��ȭ Ʈ���� ����")]
        public TriggerType triggerType = TriggerType.TimeTrigger;

        [Tooltip("���� Ʈ������ ��� �±� (�⺻: Player)")]
        public string targetTag = "Player";

        [Tooltip("���� Ʈ������ ���, �浹 ������ ���� ������Ʈ")]
        public GameObject triggerArea;

        [Tooltip("�ð� Ʈ������ ���, ��ٸ� �ð�")]
        public float delayTime = 0f;

        // ����� �ð�
        [HideInInspector] public float timer = 0f;

        // Ȱ��ȭ ����
        [HideInInspector] public bool hasActivated = false;
    }

    [Header("ť�� Ȱ��ȭ ����")]
    public List<CubeData> activationSettings = new List<CubeData>();


    // ���� Ȱ��ȭ�� ť���� ����
    private int activatedCubeCount = 0;


    // ť�� ���� Ȯ��
    private int CheckActivatedCubeCount()
    {
        int count = 0;

        foreach (CubeData data in activationSettings)
        {
            if (data.hasActivated)
            { count++; }
        }

        return count;
    }


    // ť�� Ȱ��ȭ
    private void ActivateCube(CubeData data)
    {
        // Ȱ��ȭ���� ���� ť����
        if (data.targetCube != null && !data.hasActivated)
        {
            // ť�� Ȱ��ȭ
            data.targetCube.SetActive(true);
            data.hasActivated = true;
            activatedCubeCount++;

            Debug.Log($"[{gameObject.name}] ť�� [{data.targetCube.name}]�� Ȱ��ȭ�Ǿ����ϴ�." +
                $" ({activatedCubeCount}/{activationSettings.Count})");
        }
    }




    // -------------------- �׽�Ʈ/����� --------------------

    [Header("����� �ɼ�")]
    [Tooltip("�� �����Ϳ��� ���� Ʈ���Ÿ� �ð�ȭ")]
    public bool showTriggerAreas = true;

    // ����׿�: ������ ���� Ʈ���ſ� ť�긦 ������
    void OnDrawGizmos()
    {
        if (!showTriggerAreas || activationSettings == null) return;

        foreach (var data in activationSettings)
        {
            // Ʈ���� ���� ǥ��
            if (data.triggerType == TriggerType.AreaTrigger && data.triggerArea != null)
            {
                Collider triggerCollider = data.triggerArea.GetComponent<Collider>();
                if (triggerCollider != null)
                {
                    // ���� Ʈ���Ŵ� ������ �ڽ��� ǥ��
                    Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
                    Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
                }
            }
        }
    }
}