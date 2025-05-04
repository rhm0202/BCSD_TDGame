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
        OffPanel(); // ���� �� �г� ��Ȱ��ȭ
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel(); // ESC Ű�� ������ �г� ��Ȱ��ȭ
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // ����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();

        // Ÿ�� ���� Panel On
        gameObject.SetActive(true);

        // Ÿ�� ���� ����
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
