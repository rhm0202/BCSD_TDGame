using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;
    [SerializeField]
    private float maxHP = 20;     // �ִ� ü��

    private float currentHP;      // ���� ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // ���� ü���� �ִ� ü�°� ���� ����
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        // ���� ü���� damage��ŭ ����
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // ü���� 0�� �Ǹ� ���ӿ���
        if (currentHP <= 0)
        {
            
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // ��üȭ�� ũ���� ����� imageScreen�� ������ color ������ ����
        Color color = imageScreen.color;

        // imageScreen�� ������ 40%�� ����
        color.a = 0.4f;
        imageScreen.color = color;

        // ������ 0���� Ŭ ������ ����
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}