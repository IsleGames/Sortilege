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
        public Stats(float a, float b)
        { Damage = a;
          Armor = b;
        }
    }
    // Healing represented by negative damage
    public Stats EnemyStats;
    public Stats SelfStats;
    public bool NotAmplified = false;
    public bool DiscardDecievers = false;

    public void Update(List<Effect> EffectList, List<Card> queue, List<Card> discardPile, int cardsInHand)
    {
        EnemyStats = new Stats(0, 0);
        SelfStats = new Stats(0, 0);
        NotAmplified = false;
        DiscardDecievers = false;

        foreach (var effect in EffectList)
        {
            if (queue.Count  < effect.minStreak)
            {
                continue;
            }
            
            if (effect.affectiveUnit == UnitType.Enemy)
            {
                GetStats(EnemyStats, discardPile, cardsInHand, effect);
            }

            else
            {
                GetStats(SelfStats, discardPile, cardsInHand, effect);
            }

            NotAmplified = NotAmplified || (effect.notAmplified);
            DiscardDecievers = DiscardDecievers || (effect.type == EffectType.DiscardDeceiver);
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

    Dictionary<BuffType, int> BuffCounts = new Dictionary<BuffType, int>()
    {
        { BuffType.Block, 0 },
        { BuffType.Forge, 0 },
        { BuffType.Thorns, 0 },
        { BuffType.Plague, 0 },
        { BuffType.Flinch, 0 },
        { BuffType.Voodoo, 0 },
        { BuffType.Breeze, 0 },
    };


    public override string ToString()
    {
        string BlockStr = 
            $"Enemy skips their next {BuffCounts[BuffType.Block] + BuffCounts[BuffType.Flinch]} attacks";
        string ForgeStr = $"Draw {BuffCounts[BuffType.Forge]} more cards next turn";
        string ThornsStr = $"Deal {BuffCounts[BuffType.Thorns]} damage back when hit";
        string PlagueStr = $"Deal 1 damage to each enemy for the next {BuffCounts[BuffType.Plague]} turns";
        //string FlinchStr: see BlockStr
        string VoodooStr = $"If you don't lose health this turn, gain {BuffCounts[BuffType.Voodoo]} health";
        // Breeze is still unimplemented
        string desc = "";
        if (BuffCounts[BuffType.Block] > 0 || BuffCounts[BuffType.Flinch] > 0) desc += BlockStr + ".";
        if (BuffCounts[BuffType.Forge] > 0) desc += ForgeStr + ".";
        if (BuffCounts[BuffType.Thorns] > 0) desc += ThornsStr + ".";
        if (BuffCounts[BuffType.Plague] > 0) desc += PlagueStr + ".";
        if (BuffCounts[BuffType.Voodoo] > 0) desc += VoodooStr + ".";
        return desc;
    }

    public void Update(List<BuffEffect> EffectList, List<Card> queue)
    {
        for (int type = 0; type < 7 /* update for more buffeffects*/; type++)
        {
            BuffCounts[(BuffType)type] = 0;
        }

        foreach (var effect in EffectList)
        {
            if (queue.Count >= effect.minStreak)
            {
                BuffCounts[effect.type] += (int) effect.amount;
            }
        }
    }
}
