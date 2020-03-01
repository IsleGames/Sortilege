using System.Collections.Generic;
using Effects;
using Cards;

// Describes damage, healing, armor, Deciever cost --
// everything that happens in Effects.Effect
public class EffectDescription
{
    public class Stats
    {
        public float Damage;
        public float Armor;
    }
    // Healing represented by negative damage
    public Stats EnemyStats;
    public Stats SelfStats;
    public bool NotAmplified = false;
    public bool DiscardDecievers;

    public void Update(List<Effect> EffectList, List<Card> queue, List<Card> discardPile, int cardsInHand)
    {
        foreach (var effect in EffectList)
        {
            if (queue.Count > effect.minStreak)
            {
                if (effect.affectiveUnit == UnitType.Enemy)
                {
                    GetStats(EnemyStats, discardPile, cardsInHand, effect);
                }

                else
                {
                    GetStats(SelfStats, discardPile, cardsInHand, effect);
                }
                    
            
                NotAmplified = NotAmplified || (effect.notAmplified);
                DiscardDecievers = (effect.type == EffectType.DiscardDeceiver);
            }
        }
    }

    private static void GetStats(Stats statBlock, List<Card> discardPile, int cardsInHand, Effect effect)
    {
        switch (effect.type)
        {
            case EffectType.Barrier:
                statBlock.Armor += effect.amount;
                break;
            case EffectType.Damage:
            case EffectType.DamageIgnoreBarrier:
                statBlock.Damage += effect.amount;
                break;
            case EffectType.Heal:
                statBlock.Damage -= effect.amount;
                break;
            case EffectType.DamageOnDeceiverInDiscardPile:
                foreach (var discarded in discardPile)
                {
                    if (discarded.GetComponent<MetaData>().strategy == StrategyType.Deceiver)
                    {
                        statBlock.Damage += effect.amount;
                    }
                }
                break;
            case EffectType.DiscardAllWithPerCardDamage:
                statBlock.Damage += effect.amount * cardsInHand;
                break;
            default:
                // Unimplemented
                break;
        }
    }

    public override string ToString()
    {
        string EnemyDamageStr = $"{EnemyStats.Damage} damage to an enemy";
        string SelfDamageStr = $"{SelfStats.Damage} damage to yourself";
        string SelfHealStr = $"{-SelfStats.Damage} health";
        string EnemyHealStr = $"{-EnemyStats.Damage} health";
        string SelfArmorStr = SelfStats.Armor > 0 ? "Gain " : "Lose" + $"{SelfStats.Armor} armor";
        string EnemyArmorStr = "Enemy " + (EnemyStats.Armor > 0 ? "gains " : "loses" + $"{EnemyStats.Armor} armor");
        string UnamplifiedStr = "(Unamplified)";
        string DiscardDecieverStr = "Discard all Deciever cards.";
        string desc = "";

        if (SelfStats.Damage > 0)
        {
            desc += "Deal " + SelfDamageStr;
            if (EnemyStats.Damage > 0)
            {
                desc += " and " + EnemyDamageStr;
            }
            desc += ".";
        }
        else
        {
            if (EnemyStats.Damage > 0)
            {
                desc += "Deal" + EnemyDamageStr + ".";
            }
        }
        if (SelfStats.Damage < 0)
        {
            desc += "Gain " + SelfHealStr;
            if (EnemyStats.Damage < 0)
            {
                desc += "and enemy gains" + EnemyHealStr + ".";
            }
        }
        else
        {
            if (EnemyStats.Damage< 0)
            {
                desc += "Enemy gains " + EnemyHealStr + ".";
            }
        }
        if (SelfStats.Armor != 0)
        {
            desc += SelfArmorStr + ".";
        }
        if (EnemyStats.Armor != 0)
        {
            desc += EnemyArmorStr + ".";
        }
        desc += NotAmplified ? UnamplifiedStr : "";
        desc += DiscardDecievers ? DiscardDecieverStr : "";
        return desc;
    }
}

// Describes everything in BuffEffect.cs
public class BuffDescription
{
    public int Block;
    public int Forge;
    public int Thorns;
    public int Plague;
    public int Flinch;
    public int Voodoo;
    public int Breeze;

    public override string ToString()
    {
        string BlockStr = $"Enemy skips their next {Block + Flinch} attacks";
        string ForgeStr = $"Draw {Forge} more cards next turn";
        string ThornsStr = $"Deal {Thorns} damage back when hit";
        string PlagueStr = $"Deal 1 damage to each enemy for the next {Plague} turns";
        //string FlinchStr: see BlockStr
        string VoodooStr = $"If you don't lose health this turn, gain {Voodoo} health";
        // Breeze is still unimplemented
        string desc = "";
        if (Block > 0 || Flinch > 0) desc += BlockStr + ".";
        if (Forge > 0) desc += ForgeStr + ".";
        if (Thorns > 0) desc += ThornsStr + ".";
        if (Plague > 0) desc += PlagueStr + ".";
        if (Voodoo > 0) desc += VoodooStr + ".";
        return desc;
    }
}
