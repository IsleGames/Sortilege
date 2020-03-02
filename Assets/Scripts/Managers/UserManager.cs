using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Object = UnityEngine.Object;


using Cards;
using Data;

namespace Managers
{
    public class UserManager : MonoBehaviour
    {
        public List<CardData> userOwnedCard;

        public DataManager DataOperator;
        
        private void Start()
        {
            DataOperator = FindObjectOfType<DataManager>();
            
            string path = "XMLData/StartingDeck";
            userOwnedCard = LoadDeckList(path);
        }

        public void Add(CardData newCardData)
        {
            userOwnedCard.Add(newCardData);
        }
        
        private List<CardData> LoadDeckList(string path)
        {
            List<CardData> cardList = new List<CardData>();
            TextAsset listText = Resources.Load<TextAsset>(path);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(listText.text);
            var cardNodes = xmlDocument.FirstChild.SelectNodes("descendant::card");
            
            foreach (XmlNode node in cardNodes)
            {
                string title = node.Attributes["name"].Value;
                int num = int.Parse(node.Attributes["number"].Value);
                for (int i = 0; i < num; i++)
                {
                    cardList.Add(DataOperator.GetCard(title));
                }
            }
            return cardList;
        }
        
        public List<CardData> GetCardData()
        {
            return userOwnedCard;
        }
    }
}