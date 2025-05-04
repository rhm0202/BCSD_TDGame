using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;
    [SerializeField]
    private float maxHP = 20;     // 최대 체력

    private float currentHP;      // 현재 체력

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        // 현재 체력을 최대 체력과 같게 설정
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        // 현재 체력을 damage만큼 감소
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0이 되면 게임오버
        if (currentHP <= 0)
        {
            
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 전체화면 크기의 배경인 imageScreen의 색상을 color 변수에 저장
        Color color = imageScreen.color;

        // imageScreen의 투명도를 40%로 설정
        color.a = 0.4f;
        imageScreen.color = color;

        // 투명도가 0보다 클 때까지 감소
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}