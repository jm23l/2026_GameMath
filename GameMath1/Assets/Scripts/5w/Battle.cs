using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public int playerAttack = 30;
    public float critChance = 0.3f;
    public int enemyMaxHp = 300;
    public int critMultipulier = 2;

    public int forceCritMissCount = 3;
    public int forceNormalCount = 2;

    public TMP_Text battleInfoText;
    public TMP_Text enemyHpText;
    public TMP_Text itemChanceText;
    public TMP_Text dropCountText;

    public Button attackButton;

    public int currentEnemyHp;

    private int totalAttackCount = 0;
    private int critCount = 0;
    private int missCritStreak = 0;
    private int critStreak = 0;

    private float commonChance = 50f;
    private float rareChance = 30f;
    private float epicChance = 15f;
    private float legendChance = 5f;

    private float currentCommonChance;
    private float currentRareChance;
    private float currentEpicChance;
    private float currentLegendChance;

    private int commonCount = 0;
    private int rareCount = 0;
    private int epicCount = 0;
    private int legendCount = 0;

    private void Start()
    {
        currentEnemyHp = enemyMaxHp;
        ResetItemChance();

        if(attackButton != null)
        {
            attackButton.onClick.AddListener(Attack);
        }

        UpdateUI("게임 시작");
    }

    public void Attack()
    {
        bool iscrit = RollCrit(out string critMessage);

        int damage = playerAttack;
        if (iscrit)
        {
            damage *= critMultipulier;

        }

        totalAttackCount++;

        if (iscrit)
        {
            critCount++;
            critStreak++;
            missCritStreak = 0;
        }
        else
        {
            missCritStreak++;
            critStreak = 0;
        }

        currentEnemyHp -= damage;
        if (currentEnemyHp < 0)
        {
            currentEnemyHp = 0;
        }

        string log = $"{totalAttackCount}번째 공격 : {(iscrit ? "치명타" : "일반공격")} / 데미지 {damage}";
        if(critMessage != "")
        {
            log += $"({critMessage})";
        }

        if (currentEnemyHp <= 0)
        {
            string dropResult = DropItem();
            log += $"\n적 처치! 드롭 아이템 : {dropResult}";
            currentEnemyHp = enemyMaxHp;
            log += "\n새 적 등장";
        }

        UpdateUI(log);
    }

    bool RollCrit(out string message)
    {
        message = "";

        if (missCritStreak >= forceCritMissCount)
        {
            message = "보정 발동 - 강제 치명타";
            return true;
        }
        if(critStreak >= forceNormalCount)
        {
            message = "보정 발동 - 강제 일반공격";
            return false;
        }

        float r = Random.value;
        if (r < critChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    string DropItem()
    {
        float r = Random.Range(0f, 100f);

        if (r < currentCommonChance)
        {
            commonCount++;
            IncreaseLegendChance();
            return "일반";
        }
        else if (r < currentCommonChance + currentRareChance)
        {
            rareCount++;
            IncreaseLegendChance();
            return "고급";
        }
        else if (r < currentEpicChance + currentRareChance + currentEpicChance)
        {
            epicCount++;
            IncreaseLegendChance();
            return "희귀";
        }
        else
        {
            legendCount++;
            ResetItemChance();
            return "전설";
        }
    }

    void IncreaseLegendChance()
    {
        currentCommonChance -= 0.5f;
        currentRareChance -= 0.5f;
        currentEpicChance -= 0.5f;
        currentLegendChance += 1.5f;

        currentCommonChance = Mathf.Max(0f, currentCommonChance);
        currentRareChance = Mathf.Max(0f, currentRareChance);
        currentEpicChance = Mathf.Max(0f, currentEpicChance);
    }

    void ResetItemChance()
    {
        currentCommonChance = commonChance;
        currentRareChance = rareChance;
        currentEpicChance = epicChance;
        currentLegendChance = legendChance;
    }

    void UpdateUI(string log)
    {
        float actualCritRate = 0f;
        if (totalAttackCount > 0)
        {
            actualCritRate = (float)critCount / totalAttackCount * 100f;
        }

        if (battleInfoText != null)
        {
            battleInfoText.text =
                $"전체 공격 횟수 : {totalAttackCount}\n" +
                $"발생한 치명타 횟수 : {critCount}\n" +
                $"설정된 치명타 확률 : {critChance * 100f:0.00}%\n" +
                $"실제 치명타 확률 : {actualCritRate:0.00}%";
        }
        if (enemyHpText != null)
        {
            enemyHpText.text = $"체력 : {currentEnemyHp}/{enemyMaxHp}";
        }

        if (itemChanceText != null)
        {
            itemChanceText.text =
                $"현재 아이템 확률\n" +
                $"일반 : {currentCommonChance:0.0}%\n" +
                $"고급 : {currentRareChance:0.0}%\n" +
                $"희귀 : {currentEpicChance:0.0}%\n" +
                $"전설 : {currentLegendChance:0.0}%";
        }

        if (dropCountText != null)
        {
            dropCountText.text =
                $"현재 드롭된 아이템\n" +
                $"일반 : {commonCount}\n" +
                $"고급 : {rareCount}\n" +
                $"희귀 : {epicCount}\n" +
                $"전설 : {legendCount}";
        }

       

        Debug.Log(log);
    }
   
}
