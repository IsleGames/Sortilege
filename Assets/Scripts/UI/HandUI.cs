using UnityEngine;

public class HandUI : MonoBehaviour
{
    int handLimit;
    int cardnum = 0;
    int width = 100;

    public static HandUI handui;

    private void Start()
    {
        handLimit = Game.Ctx.CardOperator.handLimit;
        handui = this;
    }

    public void addCard(Cards.Card card)
    {
        if (cardnum == handLimit) return;
        Vector3 position = new Vector3(transform.position.x, transform.position.y);
        position.x += cardnum * width;
        cardnum += 1;
        
    }

}
