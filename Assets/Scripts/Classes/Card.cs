using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum CardStatus : int
{
    Unknown,
    Stored,
    Decked,
    Held,
    Discarded,
}

public class Card : MonoBehaviour
{

    protected string Name;
    protected CardStatus Status;

    protected List<Effect> BaseEffects, EndEffects;

    // public void SetStatus(CardStatus newStatus)
    // {
    //     Status = newStatus;
    // }
    
    private void Start()
    {
        Status = CardStatus.Unknown;
        
        BaseEffects = new List<Effect>();
        EndEffects = new List<Effect>();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    public void AddBaseEffect(Effect baseEffect)
    {
        BaseEffects.Add(baseEffect);
    }

    public void AddEndEffect(Effect endEffect)
    {
        EndEffects.Add(endEffect);
    }

    public void ApplyBaseSkill(Player player, Enemy enemy)
    {

        if (Status != CardStatus.Held)
            throw new InvalidOperationException("Card is disabled with Status " + this.Status);

        // Further adjustment required
        foreach (Effect effect in BaseEffects)
        {
            if (effect.AffectiveUnit == UnitType.Player)
            {

                effect.Apply(player);

            }
            else if (effect.AffectiveUnit == UnitType.Player)
            {

                effect.Apply(enemy);
                
            }
            else
            {
                throw new NullReferenceException("Unknown Unit Type");
            }
        }

    }
}
