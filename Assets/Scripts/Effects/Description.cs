// Describes damage, healing, armor, Deciever cost --
// everything that happens in Effects.Effect
public class EffectDescription
{
    // Healing represented by negative damage
    public float EnemyDamage;
    public float SelfDamage;
    public float EnemyArmor;
    public float SelfArmor;
    public bool Amplified;
    public bool DiscardDecievers;

    public override string ToString()
    {
        string EnemyDamageStr = $"{EnemyDamage} damage to an enemy";
        string SelfDamageStr = $"{SelfDamage} damage to yourself";
        string SelfHealStr = $"{-SelfDamage} health";
        string EnemyHealStr = $"{-EnemyDamage} health";
        string SelfArmorStr = SelfArmor > 0 ? "Gain " : "Lose" + $"{SelfArmor} armor";
        string EnemyArmorStr = "Enemy " + (EnemyArmor > 0 ? "gains " : "loses" + $"{EnemyArmor} armor");
        string UnamplifiedStr = "(Unamplified)";
        string DiscardDecieverStr = "Discard all Deciever cards.";
        string desc = "";

        if (SelfDamage > 0)
        {
            desc += "Deal " + SelfDamageStr;
            if (EnemyDamage > 0)
            {
                desc += " and " + EnemyDamageStr;
            }
            desc += ".";
        }
        else
        {
            if (EnemyDamage > 0)
            {
                desc += "Deal" + EnemyDamageStr + ".";
            }
        }
        if (SelfDamage < 0)
        {
            desc += "Gain " + SelfHealStr;
            if (EnemyDamage < 0)
            {
                desc += "and enemy gains" + EnemyHealStr + ".";
            }
        }
        else
        {
            if (EnemyDamage < 0)
            {
                desc += "Enemy gains " + EnemyHealStr + ".";
            }
        }
        if (SelfArmor != 0)
        {
            desc += SelfArmorStr + ".";
        }
        if (EnemyArmor != 0)
        {
            desc += EnemyArmorStr + ".";
        }
        desc += Amplified ? "" : UnamplifiedStr;
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
