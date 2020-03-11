using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Security.Cryptography;
using System.Xml;
using _Editor;
using UnityEngine;
using Object = UnityEngine.Object;


using Cards;
using Data;

namespace Managers
{
    public class UserManager : MonoBehaviour
    {
        public List<CardData> userOwnedCard;
        public int totalRound;
        
        public DataManager DataOperator;
        public string XMLCardPath = "XMLData/StartingDeck";
        public string XMLEnemyPath = "XMLData/EnemyAppearance";
        
        private void Start()
        {
            DataOperator = FindObjectOfType<DataManager>();
            
            LoadInitialData();
        }

        public void LoadInitialData()
        {
            userOwnedCard = LoadDeckList();
            totalRound = GetTotalRoundCount();
        }

        public void Add(CardData newCardData)
        {
            userOwnedCard.Add(newCardData);
        }
        
        private int GetTotalRoundCount()
        {
            List<EnemyData> enemyList = new List<EnemyData>();
            
            TextAsset listText = Resources.Load<TextAsset>(XMLEnemyPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(listText.text);

            var enemyData = xmlDocument.FirstChild.SelectNodes("descendant::round");
            
            return enemyData.Count;
        }
        
        public List<EnemyData> GetEnemyData(int roundCount)
        {
            List<EnemyData> enemyList = new List<EnemyData>();
            
            TextAsset listText = Resources.Load<TextAsset>(XMLEnemyPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(listText.text);

            var enemyData = xmlDocument.FirstChild.SelectNodes("descendant::round")[roundCount];
            
            foreach (XmlNode node in enemyData)
            {
                string title = node.Attributes["name"].Value;
                int num = int.Parse(node.Attributes["number"].Value);
                
                for (int i = 0; i < num; i++)
                {
                    EnemyData addedEnemy = DataOperator.GetEnemy(title);
                    
                    if (!addedEnemy)
                        throw new InvalidExpressionException(title + "enemy not exist - check your spelling");
                    enemyList.Add(addedEnemy);
                }
            }
            return enemyList;
        }
        
        private List<CardData> LoadDeckList()
        {
            List<CardData> cardList = new List<CardData>();
            TextAsset listText = Resources.Load<TextAsset>(XMLCardPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(listText.text);
            var cardNodes = xmlDocument.FirstChild.SelectNodes("descendant::card");
            
            foreach (XmlNode node in cardNodes)
            {
                string title = node.Attributes["name"].Value;
                int num = int.Parse(node.Attributes["number"].Value);
                for (int i = 0; i < num; i++)
                {
                    CardData addedCard = DataOperator.GetCard(title);
                    if (!addedCard)
                        throw new InvalidExpressionException(title + "card not exist - check your spelling");
                    
                    cardList.Add(addedCard);
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