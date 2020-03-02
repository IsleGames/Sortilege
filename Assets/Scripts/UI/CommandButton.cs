using System;
using _Editor;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CommandButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleTMP, _descTMP;

        public void Start()
        {
            _titleTMP = transform.Find("TMPTitle").GetComponent<TextMeshProUGUI>();
            _descTMP = transform.Find("TMPDescription").GetComponent<TextMeshProUGUI>();
        }

        public void Click()
        {
	        if (Game.Ctx.player.waitingForAction)
			{
				if (!Game.Ctx.inSelectEnemyMode)
				{
					Game.Ctx.inSelectEnemyMode = true;
					_titleTMP.text = "Cancel";
					_descTMP.text = "Back to card arrangement";
				}
				else
				{
					Game.Ctx.inSelectEnemyMode = false;
					_titleTMP.text = "Attack!";
					_descTMP.text = "Choose you target";
				}
			}
			else
			{
				Debugger.Log("Nope, you can't end your turn now");
			}
        }
    }
}