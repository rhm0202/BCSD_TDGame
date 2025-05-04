using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;           // �� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab;   // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;        // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    //private float spawnTime;          // �� ���� �ֱ�
    //[SerializeField]
    private Transform[] wayPoints;    // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP playerHP;    // ���� ���������� �̵� ���
    [SerializeField]
    private PlayerGold playerGold;    // ���� ���������� �̵� ���

    private Wave currentWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList;                // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;

    // ���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private void Awake()
    {
        enemyList = new List<Enemy>();
        // �� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;

        currentEnemyCount = currentWave.maxEnemyCount;

        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ����ŭ ���� �����ϰ� �ڷ�ƾ ����
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // �� ������Ʈ ����
            // enemyIndex = ���� ���̺꿡 �����ϴ� �� ������ �� ������ �ε���
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);

            // Enemy ������Ʈ ��������
            Enemy enemy = clone.GetComponent<Enemy>();

            // this�� �ڽ� (EnemySpawner Ŭ����)
            // wayPoint ������ �Ű������� Setup() ȣ��
            // ����Ʈ�� ��� ������ �� ���� ����
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            // �� ü�¹� UI ���� �� ����
            SpawnEnemyHPSlider(clone);

            // ������ �� �� ����
            spawnEnemyCount++;

            // spawnTime �ð� ���� ���
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        // ���� ��ǥ�������� �������� ��
        if (type == EnemyDestroyType.Arrive)
        {
            // �÷��̾��� ü�� -1
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.kill)
        {
            // ���� ������ ���� ��� �� ��� ȹ��
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);

        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // Slider UI �ν��Ͻ��� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽ����� �����Ǿ� �־�� ȭ�鿡 ���δ�
        sliderClone.transform.SetParent(canvasTransform);
    
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �Ѿƴٴ� ����� ������ ��ġ�� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);

        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}