using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using Cards;

public class Select : MonoBehaviour

{
    public enum Direction { Up, Down };

    private Direction direction = Direction.Up;
    private Button continueButton;
    public bool selected;

    private void Start()
    {
        continueButton = FindObjectOfType<Button>();
        continueButton.onClick.AddListener(AddCard);
        continueButton.onClick.AddListener(() => StartCoroutine(Abscond(direction)));
    }


    private void OnMouseUpAsButton()
    {
        selected = true;
        direction = Direction.Down;
        continueButton.interactable = true;
    }

    public void AddCard()
    {
        if (selected)
        {
            Game.Ctx.UserOperator.Add(GetComponent<Card>().cardData);
        }
    }

    private IEnumerator Abscond(Direction direction)
    {
        float t = 0.5f;
        float v = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            v += 150 * t * t * t;
            var position = transform.position;
            if (direction == Direction.Down)
            {
                v *= -1;
            }
            position.y += v * Time.deltaTime;
            transform.position = position;
            yield return null;
        }
    }

}
