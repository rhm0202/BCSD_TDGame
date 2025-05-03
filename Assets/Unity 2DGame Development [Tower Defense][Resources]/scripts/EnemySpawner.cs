using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;   // �� ������
    [SerializeField]
    private float spawnTime;          // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;    // ���� ���������� �̵� ���
    private List<Enemy> enemyList;                // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);        // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();          // ��� ������ ���� Enemy ������Ʈ

            enemy.Setup(this, wayPoints);                             // wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);                               // ����Ʈ�� ��� ������ �� ���� ����

            yield return new WaitForSeconds(spawnTime);         // spawnTime �ð� ���� ���
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);

        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }
}