using UnityEngine;
using System.Collections;

/// <summary>
/// ������ �Ʒ��� �������� ť�갡 �� �Ʒ��� �ִ� ť���� ���鿡 ������ ��� ǥ�ø� �����ִ� ��ũ��Ʈ
/// ť�갡 ����������� ��� ǥ�ð� �� ����������, ���� ������ �ε巴�� �����.
/// </summary>
public class WarningSystem : MonoBehaviour
{
    [Header("��� ǥ�� ȿ�� ����")]
    [Tooltip("��� ���� ��ȭ ���� �Ÿ� ���� (0.5 = ���� �Ÿ����� ����)")]
    [Range(0.2f, 0.8f)]
    public float colorChangeStartRatio = 0.5f;

    [Tooltip("��� ǥ�� ����� ���� �Ÿ� (���� �� �� �Ÿ����� ������� ����)")]
    public float fadeStartDistance = 0.5f;

    [Tooltip("��� ǥ�� ����� �ð� (��, ���� Ŭ���� õõ�� �����)")]
    public float fadeDuration = 0.3f;

    // ���� ���� (������ �ʿ� ����)
    private GameObject targetCube;          // �Ʒ��� �ִ� ť��
    private GameObject warningPlane;        // ��� ǥ�� ���
    private Vector3 initialPosition;        // ���� ��ġ
    private Vector3 targetPosition;         // ������ ��ġ
    private float totalDistance;            // �� �̵� �Ÿ�
    private float colorChangeStartDist;     // ���� ��ȭ ���� �Ÿ�
    private bool isFading = false;          // ������� ������ ����
    private Material planeMaterial;         // ��� ǥ�� ����

    // ���� ������ (�����Ϸ��� �ڵ� ���� �ʿ�)
    private readonly Color warningColor = Color.red;      // ��� ���� (������)
    private const float startAlpha = 0.3f;                // �ʱ� ���� (0.3 = 70% ����)
    private const float maxAlpha = 0.8f;                  // �ִ� ���� (0.8 = 20% ����)
    private const float emissionIntensity = 1f;           // �߱� ����
    private const float intensityCurve = 1f;              // ���� ��ȭ � (1 = ������ �ӵ�)

    
    //   void Awake()
    //   {
    //       // ���� �� �߿� ���� ���
    //       Debug.Log("��� �ý���: ť�� ������ ���� Default ���̾ ����մϴ�.");
    //   }

    void Start()
    {
        // �ʱ� ��ġ ����
        initialPosition = transform.position;

        // �Ʒ��� �ִ� ť�� üũ �� ��� ǥ��
        CheckForCubeBelow();
    }

    void OnEnable()
    {
        // Ȱ��ȭ�� ���� üũ
        if (transform.position == initialPosition) // �ʱ� ��ġ�� ���� üũ
        {
            CheckForCubeBelow();
        }
    }


    private void CheckForCubeBelow()
    {
        // �ڱ� �ڽ��� ����ĳ��Ʈ���� �����ϱ� ���� �ӽ÷� ���̾� ����
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // ��� �浹 ������Ʈ ���� (���� �Ÿ��� �����Ͽ� �Ÿ� ���� ���� ����)
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, Mathf.Infinity, 1);

        // �����: ������ ������Ʈ �� Ȯ��
        Debug.Log("����ĳ��Ʈ�� ������ ������Ʈ ��: " + hits.Length);

        // �浹 �Ÿ��� ���� ���� (����� �ͺ���)
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool foundValidCube = false;

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            // �����: ������ ������Ʈ ����
            Debug.Log("- ������ ������Ʈ: " + hitObject.name + " (�Ÿ�: " + hit.distance + " ����)");

            // �ڱ� �ڽ� ����
            if (hitObject == gameObject) continue;

            // �÷��̾� ���� (�±׷� Ȯ��)
            if (hitObject.CompareTag("Player"))
            {
                Debug.Log("�÷��̾� ������Ʈ ����: " + hitObject.name);
                continue;
            }

            // ���� �̵� ���� ť�� ���� (CubeMover�� �ְ� ���� �����̰� �ִ� ť��)
            CubeMover cubeMover = hitObject.GetComponent<CubeMover>();
            if (cubeMover != null && cubeMover.IsCurrentlyMoving)
            {
                Debug.Log("���� �̵� ���� ť�� ����: " + hitObject.name);
                continue;
            }

            // ������ ť�� ã��
            targetCube = hitObject;
            foundValidCube = true;

            // �����: ���õ� ť�� Ȯ��
            Debug.Log("���õ� ť��: " + targetCube.name + " (�Ÿ�: " + hit.distance + " ����)");

            // ť�� ó�� ����
            Renderer targetRenderer = targetCube.GetComponent<Renderer>();
            Renderer thisRenderer = GetComponent<Renderer>();

            if (targetRenderer != null && thisRenderer != null)
            {
                float targetTopY = targetRenderer.bounds.center.y + targetRenderer.bounds.extents.y;
                float thisHalfHeight = thisRenderer.bounds.extents.y;

                // ť�갡 ������ ��ġ ���
                targetPosition = new Vector3(
                    transform.position.x,
                    targetTopY + thisHalfHeight,
                    transform.position.z
                );

                // ���� ��ġ���� ���������� �� �Ÿ� ���
                totalDistance = Vector3.Distance(initialPosition, targetPosition);

                // ���� ��ȭ ���� �Ÿ� ���
                colorChangeStartDist = totalDistance * colorChangeStartRatio;

                // ������ ��� ǥ�� ����
                CreateWarningPlane(hit, targetRenderer);
            }

            break; // ù ��° ������ ť�� ã�����Ƿ� �ߴ�
        }

        // ���� ���̾�� ����
        gameObject.layer = originalLayer;

        if (!foundValidCube)
        {
            Debug.LogWarning("�Ʒ��� ������ ť�긦 ã�� �� �����ϴ�.");
        }
    }

    // ��� ǥ�� ���� 
    private void CreateWarningPlane(RaycastHit hit, Renderer targetRenderer)
    {
        if (targetCube == null) return;

        // ������ ������ ��� ǥ�ð� ������ ����
        if (warningPlane != null)
        {
            Destroy(warningPlane);
        }

        // ť�� ���� ��ġ ���
        float targetTopY = targetRenderer.bounds.center.y + targetRenderer.bounds.extents.y;
        Vector3 planePosition = new Vector3(
            hit.point.x,
            targetTopY + 0.005f, // ���̸� ���� ��¦ �ø� (5mm)
            hit.point.z
        );
        // �����: ��� ǥ�� ��ġ Ȯ��
        Debug.Log("��� ǥ�� ���� ��ġ: " + planePosition + ", ������ ť��: " + targetCube.name);

        // ��� ����
        warningPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        warningPlane.name = "Warning_" + targetCube.name;

        // ��ġ �� ȸ�� ����
        warningPlane.transform.position = planePosition;
        warningPlane.transform.rotation = Quaternion.Euler(90, 0, 0); // �ٴڿ� �����ϰ� ����

        // ũ�� ���� (ť�� ũ�⿡ �°�)
        float planeSize = targetRenderer.bounds.extents.x * 2;
        warningPlane.transform.localScale = new Vector3(planeSize, planeSize, 1f);

        // �浹ü ���� (�ʿ� ����)
        Collider planeCollider = warningPlane.GetComponent<Collider>();
        if (planeCollider != null)
        {
            Destroy(planeCollider);
        }

        // ��� ǥ�ø� Ignore Raycast ���̾�� �����Ͽ� �ٸ� ����ĳ��Ʈ�� �������� �ʵ��� ��
        warningPlane.layer = LayerMask.NameToLayer("Ignore Raycast");

        // ��Ƽ���� ����
        Renderer planeRenderer = warningPlane.GetComponent<Renderer>();
        if (planeRenderer != null)
        {
            planeMaterial = new Material(Shader.Find("Standard"));

            // ������ ����
            planeMaterial.SetFloat("_Mode", 3); // ���� ���
            planeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            planeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            planeMaterial.SetInt("_ZWrite", 0);
            planeMaterial.DisableKeyword("_ALPHATEST_ON");
            planeMaterial.EnableKeyword("_ALPHABLEND_ON");
            planeMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            planeMaterial.renderQueue = 3000;

            // �ʱ� ���� ���� 
            Color color = warningColor;
            color.a = startAlpha; // ���� ���� ����
            planeMaterial.color = color;

            // �߱� ȿ�� (ó������ ���ϰ�)
            planeMaterial.EnableKeyword("_EMISSION");
            planeMaterial.SetColor("_EmissionColor", warningColor * startAlpha * emissionIntensity);

            // ��Ƽ���� ����
            planeRenderer.material = planeMaterial;
        }
    }

    void Update()
    {
        // ��� ǥ�ð� ���ų� ť�갡 �������� �ʾ����� �ƹ��͵� ���� ����
        if (warningPlane == null || targetCube == null || planeMaterial == null) return;

        // ���� ���������� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // ������ ���� (�ʱ� ��ġ���� 0.05 �̻� ����������)
        bool isMoving = Vector3.Distance(transform.position, initialPosition) > 0.05f;

        if (isMoving && !isFading)
        {
            // ���������� ���� �Ÿ��� ���̵� ���� �Ÿ����� ������ ������� ����
            if (distanceToTarget <= fadeStartDistance)
            {
                StartCoroutine(FadeOutWarning());
            }
            else
            {
                // ���� ���� ��� (�Ÿ��� �پ����� �� ��������)
                UpdateWarningIntensity(distanceToTarget);
            }
        }
    }

    // �Ÿ��� ���� ��� ǥ�� ���� ������Ʈ
    private void UpdateWarningIntensity(float currentDistance)
    {
        // �Ÿ� ���� ��� (1 = �� �Ÿ�, 0 = ����� �Ÿ�)
        float distanceRatio = Mathf.Clamp01(currentDistance / totalDistance);

        // ���� ��ȭ ������ ������ ������ ���
        if (distanceRatio > colorChangeStartRatio)
        {
            // ���� ���� ��ȭ ������ �������� ���� - �ʱ� ���� ����
            Color color = warningColor;
            color.a = startAlpha;  // �ʱ� ���� ����
            planeMaterial.color = color;
            planeMaterial.SetColor("_EmissionColor", warningColor * startAlpha * emissionIntensity);
            return;
        }

        // ��ȭ ���� ���� ��� (0 = ��ȭ ����, 1 = �ִ� ����)
        float changeProgress = 1f - (distanceRatio / colorChangeStartRatio);

        // ���� � ���� (�� �ڿ������� ��ȭ�� ����)
        float curvedProgress = Mathf.Pow(changeProgress, intensityCurve);

        // ���� ��� (startAlpha���� maxAlpha�� ��ȭ)
        float alpha = Mathf.Lerp(startAlpha, maxAlpha, curvedProgress);

        // ���� �� �߱� ȿ�� ������Ʈ
        Color newColor = warningColor;
        newColor.a = alpha;  // �� ���� ����
        planeMaterial.color = newColor;

        // �߱� ������ �Բ� ����
        float emissionStrength = alpha * emissionIntensity;
        planeMaterial.SetColor("_EmissionColor", warningColor * emissionStrength);
    }

    // ��� ǥ�� ������ ������� �ϱ�
    private IEnumerator FadeOutWarning()
    {
        if (warningPlane == null || planeMaterial == null) yield break;

        isFading = true;

        // ���� ���� ��������
        Color startColor = planeMaterial.color;
        Color emissionColor = planeMaterial.GetColor("_EmissionColor");

        // ������ ������� ó��
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // ���� ���� (���� ��������)
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startColor.a, 0f, t);
            planeMaterial.color = newColor;

            // �߱� ������ �Բ� ����
            Color newEmission = Color.Lerp(emissionColor, Color.black, t);
            planeMaterial.SetColor("_EmissionColor", newEmission);

            yield return null;
        }

        // ������ ���������� ����
        RemoveWarning();
    }

    // ��� ǥ�� ����
    private void RemoveWarning()
    {
        if (warningPlane != null)
        {
            Destroy(warningPlane);
            warningPlane = null;
        }

        isFading = false;
    }

    void OnDisable()
    {
        // ��Ȱ��ȭ�� �� ��� ����
        RemoveWarning();
    }

    void OnDestroy()
    {
        // ��ũ��Ʈ�� �ı��� �� ��� ���� (�� ��ȯ�̳� ���� ���� ��)
        // ���� �ı��Ǵ°� ������ ����ġ ���� ��Ȳ(�� ��ȯ, ���� ����, ����� �� ������Ʈ ���� ��) �޸� ���� ������
        RemoveWarning();
    }
}