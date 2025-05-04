using System.Collections;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;          // �߻�ü ������
    [SerializeField]
    private Transform spawnPoint;                 // �߻�ü ���� ��ġ
    [SerializeField]
    private float attackRate = 0.5f;              // ���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f;             // ���� ����
    [SerializeField]
    private int attackDamage = 1;               // ���ݷ�
    private int level = 0;                                       // Ÿ�� ����
    private WeaponState weaponState = WeaponState.SearchTarget;  // Ÿ�� ������ ����
    private Transform attackTarget = null;                       // ���� ���
    private EnemySpawner enemySpawner;                           // ���ӿ� �����ϴ� �� ���� ȹ���

    public float Damage => attackDamage;
    public float Rate   => attackRate;
    public float Range  => attackRange;
    public int   Level  => level + 1;

    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // ���� ���¸� SearchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        // ������ ��� ���� ���� ����
        StopCoroutine(weaponState.ToString());

        // ���� ����
        weaponState = newState;

        // ���ο� ���� ���
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if (attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        // �������κ����� �Ÿ��� ����/���� ��ǥ�� ���� ��� (��ũź��Ʈ �̿�)
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        // ���� ��� (���� -> ��)
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // ȸ�� ����
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            // ���� ������ �ִ� ���� ã�� ���� �ִ� �Ÿ� ����
            float closestDistSqr = Mathf.Infinity;

            // EnemySpawner�� EnemyList�� �ִ� �� ��� �˻�
            for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

                // ���� �� && ���� ����� ���̸� Ÿ������ ����
                if (distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            // 1. Ÿ�� ��ȿ�� �˻�
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. Ÿ���� ���� �ȿ� �ִ��� Ȯ��
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. ���� �ֱ⸸ŭ ���
            yield return new WaitForSeconds(attackRate);

            // 4. �߻�ü ����
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ��(attackTarget) ���� ����
        clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage);
    }
}
