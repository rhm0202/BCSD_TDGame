using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;              // �ִ� ü��
    private float currentHP;          // ���� ü��
    private bool isDie = false;       // ���� ��� �����̸� true

    private Enemy enemy;              // Enemy ���� ���ٿ�
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;                                      // ���� ü���� �ִ� ü������ �ʱ�ȭ
        enemy = GetComponent<Enemy>();                          // Enemy ������Ʈ ����
        spriteRenderer = GetComponent<SpriteRenderer>();        // �ð� ȿ���� SpriteRenderer ����
    }

    public void TakeDamage(float damage)
    {
        // Tip. ���� Ÿ���� ������ ��ĥ �� �����Ƿ� �̹� ���� ���̸� �ߺ� ó�� ����
        if (isDie == true) return;

        // ���� ü�� ����
        currentHP -= damage;

        // �ǰ� ������ ȿ��
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0 �����̸� ��� ó��
        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(EnemyDestroyType.kill); // �� ���� ó�� (Spawner�� ����)
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ����
        Color color = spriteRenderer.color;

        // ���� 40%
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05�� ���
        yield return new WaitForSeconds(0.05f);

        // ���� 100%
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}