using UnityEngine;
using System.Collections.Generic;

public static class InnEffectHandler
{
    public static void ApplyInnEffect()
    {
        if (AnimaInventoryManager.Instance != null)
        {
            List<AnimaDataSO> allAnima = AnimaInventoryManager.Instance.GetAllAnima();
            ReviveAndHealAllAnima(allAnima);
        }
    }

    private static void ReviveAndHealAllAnima(List<AnimaDataSO> animas)
    {
        if (animas == null || animas.Count == 0) return;
    
        foreach (var anima in animas)
        {
            if (anima != null)
            {
                if (anima.Animadie)
                {
                    anima.Animadie = false;
                    anima.Stamina = anima.Maxstamina;
                }
                else if (anima.Stamina < anima.Maxstamina)
                {
                    anima.Stamina = anima.Maxstamina;
                }
            }
        }
    }
}