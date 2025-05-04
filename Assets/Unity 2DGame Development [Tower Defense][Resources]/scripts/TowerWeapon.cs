using System.Collections;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;          // 발사체 프리팹
    [SerializeField]
    private Transform spawnPoint;                 // 발사체 생성 위치
    [SerializeField]
    private float attackRate = 0.5f;              // 공격 속도
    [SerializeField]
    private float attackRange = 2.0f;             // 공격 범위
    [SerializeField]
    private int attackDamage = 1;               // 공격력
    private int level = 0;                                       // 타워 레벨
    private WeaponState weaponState = WeaponState.SearchTarget;  // 타워 무기의 상태
    private Transform attackTarget = null;                       // 공격 대상
    private EnemySpawner enemySpawner;                           // 게임에 존재하는 적 정보 획득용

    public float Damage => attackDamage;
    public float Rate   => attackRate;
    public float Range  => attackRange;
    public int   Level  => level + 1;

    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // 최초 상태를 SearchTarget으로 설정
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        // 이전에 재생 중인 상태 종료
        StopCoroutine(weaponState.ToString());

        // 상태 변경
        weaponState = newState;

        // 새로운 상태 재생
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
        // 원점으로부터의 거리와 수평/수직 좌표로 각도 계산 (아크탄젠트 이용)
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        // 각도 계산 (라디안 -> 도)
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            // 제일 가까이 있는 적을 찾기 위해 최대 거리 설정
            float closestDistSqr = Mathf.Infinity;

            // EnemySpawner의 EnemyList에 있는 적 모두 검사
            for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

                // 범위 내 && 가장 가까운 적이면 타겟으로 설정
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
            // 1. 타겟 유효성 검사
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. 타겟이 범위 안에 있는지 확인
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. 공격 주기만큼 대기
            yield return new WaitForSeconds(attackRate);

            // 4. 발사체 생성
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // 생성된 발사체에게 공격대상(attackTarget) 정보 제공
        clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage);
    }
}
