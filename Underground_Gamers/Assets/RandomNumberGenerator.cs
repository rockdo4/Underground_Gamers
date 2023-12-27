using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    // 평균이 0이고 표준 편차가 1인 정규 분포를 따르는 난수를 생성
    float damage;
    float acc;
    float GenerateRandomNumber()
    {
        float u1 = 1f - Random.Range(0f, 1f);
        float u2 = 1f - Random.Range(0f, 1f);

        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);

        // 평균이 0, 표준 편차가 1인 정규 분포로 변환
        return randStdNormal;
    }

    void Start()
    {
        damage = 100f;
        acc = 40f;
        

        // 정규 분포를 따르는 난수
        for (int i = 0; i < 20; ++i)
        {
            float randomValue = GenerateRandomNumber();
            damage *= (1.1f - Mathf.Abs((randomValue * 50.0f / acc)) * 0.2f);
            damage = 100f;
        }
    }
}