using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;

    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider UI가 붙어야 할 target 설정
        targetTransform = target;

        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 target이 사라지면 Slider UI도 삭제
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트의 위치가 갱신된 이후에 Slider UI도 같이 위치를 맞추기 위해 LateUpdate()에서 실행
        // 오브젝트의 월드 좌표를 기준으로 화면 좌표(Screen Point) 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        // 화면에서 screenPosition + distance 만큼 떨어진 위치를 Slider UI의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }
}