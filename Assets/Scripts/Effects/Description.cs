// Dynamic descriptions for list of effects
// The intent is for cards to use the text 
// in CardData.description unless they're on top
// of the queue, when they use one of these.
// This is a liiiittle bit of a cop-out to avoid 
// caring about the context an effect appears in

using System.Collections.Generic;
using Effects;
using Cards;
using UnityEngine;

// Describes damage, healing, armor, Deciever cost --
// everything that happens in Effects.Effect
public class EffectDescription
{
    public class Stats
    {
        // Healing represented by negative damage
        public float Damage;
        public float Armor;
        public Stats(float a, float b)
        { Damage = a;
          Armor = b;
        }
        public void Multiply(float n)
        {
            Damage *= n;
            Armor *= n;
        }
    }
    public Stats EnemyStatsLowBound;
    public Stats EnemyStatsHighBound;
    public Stats SelfStatsLowBound;
    public Stats SelfStatsHighBound;
    public bool NotAmplified = false;
    public bool DiscardDecievers = false;

    public EffectDescription()
    {
        Initialize();
    }

    //Aggregate all the effects that appear on a single card
    public void Update(List<Effect> EffectList, int currentStreak, int DeceiversInDiscard, int cardsInHand)
    {
        Initialize();

        foreach (var effect in EffectList)
        {
            if (currentStreak < effect.minStreak)
            {
                continue;
            }

            if (effect.affectiveUnit == UnitType.Player)
            {
                GetStats(effect, SelfStatsLowBound, SelfStatsHighBound, currentStreak); 
            }

            else
            {
                GetStats(effect, EnemyStatsLowBound, EnemyStatsHighBound, currentStreak);
            }

           
            DiscardDecievers = DiscardDecievers || (effect.type == EffectType.DiscardDeceiver);
        }
        
    }

    private void Initialize()
    {
        EnemyStatsLowBound = new Stats(0, 0);
        EnemyStatsHighBound = new Stats(0, 0);
        SelfStatsLowBound = new Stats(0, 0);
        SelfStatsHighBound = new Stats(0, 0);
        NotAmplified = false;
        DiscardDecievers = false;
    }

    // Figure out how much damage / healing / armor an effect is going to affect
    private static void GetStats(Effect effect, Stats lowBound, Stats highBound, int currentStreak)
    {
        (float low, float high) = effect.EffectRange(currentStreak);
        switch (effect.type)
        {
            case EffectType.Barrier:
                lowBound.Armor += low;
                highBound.Armor += high;
                break;
            case EffectType.Damage:
            case EffectType.DamageIgnoreBarrier:
            case EffectType.DamageOnDeceiverInDiscardPile:
            case EffectType.DiscardAllWithPerCardDamage:
                lowBound.Damage += low;
                highBound.Damage += high;
                break;
            case EffectType.Heal:
                lowBound.Damage -= high;
                highBound.Damage -= low;
                break;
            default:
                // Unimplemented
                break;
        }
        
    }

    string DescribeRange(float low, float high)
    {
        if (Mathf.Approximately(low, high))
        {
            return $"{ low}";
        }
        else
        {
            return $"{low} - {high}";
        }
    }

    // Generate reasonable text 
    public override string ToString()
    {
        string EnemyDamageStr = $"{DescribeRange(EnemyStatsLowBound.Damage, EnemyStatsHighBound.Damage)} dmg to an enemy";
        string SelfDamageStr = $"{DescribeRange(SelfStatsLowBound.Damage, SelfStatsHighBound.Damage)} dmg to you";
        string SelfHealStr = $"{DescribeRange(-SelfStatsLowBound.Damage, -SelfStatsHighBound.Damage)} health";
        string EnemyHealStr = $"{DescribeRange(-EnemyStatsLowBound.Damage, - EnemyStatsHighBound.Damage)} health";
        string SelfArmorStr = $"{(SelfStatsLowBound.Armor > 0 ? "Gain " : "Lose ")}{DescribeRange(SelfStatsLowBound.Armor, SelfStatsHighBound.Armor)} armor";
        string EnemyArmorStr = $"Enemy {(EnemyStatsLowBound.Armor > 0 ? "gains" : "loses")} {DescribeRange(EnemyStatsLowBound.Armor, EnemyStatsHighBound.Armor)} armor";
        string DiscardDecieverStr = "Discard all Deceiver cards.";
        string desc = "";

        if (SelfStatsLowBound.Damage > 0)
        {
            desc += "Deal " + SelfDamageStr;
            if (EnemyStatsLowBound.Damage > 0)
            {
                desc += " and " + EnemyDamageStr;
            }
            desc += ". ";
        }
        else
        {
            if (EnemyStatsLowBound.Damage > 0)
            {
                desc += $"Deal {EnemyDamageStr}. ";
            }
        }
        if (SelfStatsHighBound.Damage < 0)
        {
            desc += "Gain " + SelfHealStr;
            if (EnemyStatsHighBound.Damage < 0)
            {
                desc += $"and enemy gains {EnemyHealStr}. ";
            }
        }
        else
        {
            if (EnemyStatsHighBound.Damage< 0)
            {
                desc += $"Enemy gains {EnemyHealStr}. ";
            }
        }
        if (SelfStatsHighBound.Armor != 0)
        {
            desc += SelfArmorStr + ". ";
        }
        if (EnemyStatsHighBound.Armor != 0)
        {
            desc += EnemyArmorStr + ". ";
        }
        
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

    // Generate reasonable text
    public override string ToString()
    {
        string BlockStr = 
            $"Enemy skips their next {BuffCounts[BuffType.Block] + BuffCounts[BuffType.Flinch]} attacks";
        string ForgeStr = $"Draw {BuffCounts[BuffType.Forge]} extra cards";
        string ThornsStr = $"Deal {BuffCounts[BuffType.Thorns]} damage when next hit";
        string PlagueStr = $"Deal 1 damage to each enemy for the next {BuffCounts[BuffType.Plague]} turns";
        //string FlinchStr: see BlockStr
        string VoodooStr = $"If you don't lose health this turn, gain {BuffCounts[BuffType.Voodoo]} health";
        // Breeze is still unimplemented
        string desc = "";
        if (BuffCounts[BuffType.Block] > 0 || BuffCounts[BuffType.Flinch] > 0) desc += BlockStr + ". ";
        if (BuffCounts[BuffType.Forge] > 0) desc += ForgeStr + ". ";
        if (BuffCounts[BuffType.Thorns] > 0) desc += ThornsStr + ". ";
        if (BuffCounts[BuffType.Plague] > 0) desc += PlagueStr + ". ";
        if (BuffCounts[BuffType.Voodoo] > 0) desc += VoodooStr + ". ";
        return desc;
    }

    public void Update(List<BuffEffect> EffectList, float streak)
    {
        // Reset all the counts to 0
        for (int type = 0; type < 7 /* update for more buffeffects*/; type++)
        {
            BuffCounts[(BuffType)type] = 0;
        }

        foreach (var effect in EffectList)
        {
            if (streak >= effect.minStreak)
            {
                BuffCounts[effect.type] += (int)(effect.amount * streak);
            }
        }
    }
}
