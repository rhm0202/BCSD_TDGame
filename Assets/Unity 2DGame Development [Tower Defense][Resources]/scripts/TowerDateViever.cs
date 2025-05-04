using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;

    [SerializeField]
    private TextMeshProUGUI textDamage;

    [SerializeField]
    private TextMeshProUGUI textRate;

    [SerializeField]
    private TextMeshProUGUI textRange;

    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TowerAttackRange towerAttackRange;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel(); // 시작 시 패널 비활성화
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel(); // ESC 키를 누르면 패널 비활성화
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();

        // 타워 정보 Panel On
        gameObject.SetActive(true);

        // 타워 정보 갱신
        UpdateTowerData();

        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text   = "Rate : " + currentTower.Rate;
        textRange.text  = "Range : " + currentTower.Range;
        textLevel.text  = "Level : " + currentTower.Level;
    }
}
