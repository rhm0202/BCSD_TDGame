using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;

    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider UI�� �پ�� �� target ����
        targetTransform = target;

        // RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� target�� ������� Slider UI�� ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� ���� ��ġ�� ���߱� ���� LateUpdate()���� ����
        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�� ��ǥ(Screen Point) ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        // ȭ�鿡�� screenPosition + distance ��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }
}