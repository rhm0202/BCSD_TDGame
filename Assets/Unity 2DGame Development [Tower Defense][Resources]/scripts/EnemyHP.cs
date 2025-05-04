using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;              // 최대 체력
    private float currentHP;          // 현재 체력
    private bool isDie = false;       // 적이 사망 상태이면 true

    private Enemy enemy;              // Enemy 정보 접근용
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;                                      // 현재 체력을 최대 체력으로 초기화
        enemy = GetComponent<Enemy>();                          // Enemy 컴포넌트 참조
        spriteRenderer = GetComponent<SpriteRenderer>();        // 시각 효과용 SpriteRenderer 참조
    }

    public void TakeDamage(float damage)
    {
        // Tip. 여러 타워의 공격이 겹칠 수 있으므로 이미 죽은 적이면 중복 처리 방지
        if (isDie == true) return;

        // 현재 체력 감소
        currentHP -= damage;

        // 피격 깜빡임 효과
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0 이하이면 사망 처리
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(EnemyDestroyType.kill); // 적 제거 처리 (Spawner가 관리)
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 현재 색상 저장
        Color color = spriteRenderer.color;

        // 투명도 40%
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05초 대기
        yield return new WaitForSeconds(0.05f);

        // 투명도 100%
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}