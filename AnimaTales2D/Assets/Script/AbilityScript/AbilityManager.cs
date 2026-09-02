using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    float goldSymbol = 0f;
    public float GoldSymbol => goldSymbol;
    float mixSymbol = 0f;
    public float MixSymbol => mixSymbol;
    float dropSymbol = 0f;
    public float DropSymbol => dropSymbol;
    float shieldSymbol = 0f;
    public float ShieldSymbol => shieldSymbol;
    [SerializeField]
    List<AbilitySO> abilitys = new();
    public List<AbilitySO> Abilitys => abilitys;

    private const string GoldSymbolName = "GoldSymbol";
    private const string MixSymbolName = "MixSymbol";
    private const string DropSymbolName = "DropSymbol";
    private const string StatSymbolName = "StatSymbol";
    private const string PermanShieldSymbolName = "PermanShieldSymbol";
    private const string TemporShieldSymbolName = "TemporShieldSymbol";

    public void GetSymbol(AbilitySO ability)
    {
        abilitys.Add(ability);
        switch (ability.data.name)
        {
            case GoldSymbolName:
                goldSymbol += ability.data.value;
                break;
            case MixSymbolName:
                mixSymbol += ability.data.value;
                break;
            case DropSymbolName:
                dropSymbol += ability.data.value;
                break;
            case StatSymbolName:
                {
                    var battleAnima = GetPlayerInfo().battleAnima;
                    for (int i = 0; i < battleAnima.Count; i++)
                    {
                        battleAnima[i].weight *= (1 + ability.data.value);
                    }
                    break;
                }
            case PermanShieldSymbolName:
                shieldSymbol += ability.data.value;
                break;
            case TemporShieldSymbolName:
                {
                    var battleAnima = GetPlayerInfo().battleAnima;
                    for (int i = 0; i < battleAnima.Count; i++)
                    {
                        battleAnima[i].Shield += battleAnima[i].Maxstamina * ability.data.value;
                    }
                    break;
                }
        }
    }

    // Looked up lazily: GameObject.Find is only needed for the stat/shield-symbol cases above.
    private PlayerInfo GetPlayerInfo()
    {
        return GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
    }
}
