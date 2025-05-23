using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        // "MainCamera" 태그를 가진 오브젝트에서 Camera 컴포넌트 가져오기
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪힌 오브젝트 정보를 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 광선에 부딪힌 오브젝트의 태그가 "Tile"이라면
                if (hit.transform.CompareTag("Tile"))
                {
                    // 타워를 생성하는 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
                // 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
                else if ( hit.transform.CompareTag("Tower") )
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
    }
}