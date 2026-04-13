using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private int weak = 0;
    private int miss = 0;
    private int critical = 0;
    private float maxDamage = 0f;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0);
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
        
        weak = 0;
        miss = 0;
        critical = 0;
        maxDamage = 0f;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();

    }

    
    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        level++;
        baseDamage += 20f;
        UpdateUI();
    }

    public void Attack()
    {
        float stdDev = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, stdDev);

        float weakLine = baseDamage + 2f * stdDev;
        float missLine = baseDamage - 2f * stdDev;

        attackCount++;

        if (normalDamage < missLine)
        {
            miss++;
            logDisplay.text = "[명중 실패] 데미지: 0";
            UpdateUI();
            return;
        }

        float finalDamage = normalDamage;
        bool isWeakPoint = false;
        bool isCrit = false;

        if (normalDamage > weakLine)
        {
            isWeakPoint = true;
            weak++;
            finalDamage *= 2f;
        }

        if (Random.value < critRate)
        {
            isCrit = true;
            critical++;
            finalDamage *= critMult;
        }

        totalDamage += finalDamage;

        if (finalDamage > maxDamage)
        {
            maxDamage = finalDamage;
        }

        if (isWeakPoint && isCrit)
        {
            logDisplay.text = string.Format("[약점 + 치명타] 데미지: {0:F1}", finalDamage);
        }
        else if (isWeakPoint)
        {
            logDisplay.text = string.Format("[약점 공격] 데미지: {0:F1}", finalDamage);
        }
        else if (isCrit)
        {
            logDisplay.text = string.Format("[치명타] 데미지: {0:F1}", finalDamage);
        }
        else
        {
            logDisplay.text = string.Format("[일반타] 데미지: {0:F1}", finalDamage);
        }

        UpdateUI();
    }

    public void Attack1000()
    {
        totalDamage = 0;
        attackCount = 0;
        weak = 0;
        miss = 0;
        critical = 0;
        maxDamage = 0f;

        for (int i = 0; i < 1000; i++)
        {
            float stdDev = baseDamage * stdDevMult;
            float normalDamage = GetNormalStdDevDamage(baseDamage, stdDev);

            float weakLine = baseDamage + 2f * stdDev;
            float missLine = baseDamage - 2f * stdDev;

            attackCount++;

            if (normalDamage < missLine)
            {
                miss++;
                continue;
            }

            float finalDamage = normalDamage;

            if (normalDamage > weakLine)
            {
                weak++;
                finalDamage *= 2f;
            }

            if (Random.value < critRate)
            {
                critical++;
                finalDamage *= critMult;
            }

            totalDamage += finalDamage;

            if (finalDamage > maxDamage)
            {
                maxDamage = finalDamage;
            }
        }

        logDisplay.text = string.Format(
            "[1000회 결과]\n약점 공격: {0}\n명중 실패: {1}\n전체 치명타: {2}\n최대 데미지: {3:F1}",
            weak, miss, critical, maxDamage);

        UpdateUI();
    }
    private void UpdateUI()
    {
        float stdDev = baseDamage * stdDevMult;
        float minDamage = baseDamage - (3 * stdDev);
        float maxNormalDamage = baseDamage + (3 * stdDev);

        float weakLine = baseDamage + (2 * stdDev);
        float missLine = baseDamage - (2 * stdDev);

        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수: {1}\n평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
