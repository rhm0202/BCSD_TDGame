using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private int damage;

    public void Setup(Transform target, int damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target; // Ÿ���� �������� Ÿ��
        this.damage = damage; // Ÿ���� �������� ���ݷ�
    }

    private void Update()
    {
        if (target != null) // target�� �����ϸ�
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            // ���� ������ target�� ������� �߻�ü ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;                 // ���� �ƴ� ���� �ε����� ��
        if (collision.transform != target) return;                  // ���� target�� ���� �ƴ� ��

       // collision.GetComponent<Enemy>().OnDie();                    // �� ��� �Լ� ȣ��
       collision.GetComponent<EnemyHP>().TakeDamage(damage);          // �� ü�� ����
        Destroy(gameObject);                                       // �߻�ü ������Ʈ ����
    }
}
