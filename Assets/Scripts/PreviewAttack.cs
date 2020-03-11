using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Enemies;
using UnityEngine.UI;
using TMPro;


public class PreviewAttack : MonoBehaviour
{
    public Enemy enemyUnit;
    EffectDescription effectDescription = new EffectDescription();
    BuffDescription buffDescription = new BuffDescription();
    public TextMeshProUGUI description; // TODO: m
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        enemyUnit.onTurnBegin.AddListener(() => Hide());
        enemyUnit.onTurnEnd.AddListener(() => Show());
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyUnit.nextAttack != null)
        {
            effectDescription.Update(enemyUnit.nextAttack.effectList, 1, 0, 0);
            buffDescription.Update(enemyUnit.nextAttack.buffEffectList, 1);


            description.text = effectDescription.ToString() + "\n" + buffDescription.ToString();
        }

        if (Game.Ctx.BattleOperator.activeUnit == Game.Ctx.BattleOperator.player)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {

        description.enabled = true;
        background.enabled = true;

    }

    public void Hide()
    {
        background.enabled = false;
        description.enabled = false;
    }





}
