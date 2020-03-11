using System;
using System.Collections;
using System.Collections.Generic;
using _Editor;
using Data;
using UI;
using Units;
using Units.Enemies;
using UnityEngine;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public List<Enemy> EnemyList;
        
        public List<EnemyData> EnemyInitData;
        [SerializeField]
        private Enemy curEnemy;
        
        private IEnumerator AttackSeq;

        public GameObject enemyPrefab;
    
        private void Start()
        {
            enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy");

			if (Game.Ctx.UserOperator && !Game.Ctx.isTutorial)
			{
				EnemyInitData = Game.Ctx.UserOperator.GetEnemyData(Game.Ctx.GameOperator.GetRoundCount());
			}
			else
			{
				// keep cardList the same as in inspector
			}
            
            foreach (EnemyData cardData in EnemyInitData)
			{
				GameObject newEnemyObj = Instantiate(enemyPrefab, transform);
				Enemy newEnemy = newEnemyObj.GetComponent<Enemy>();
				
				newEnemy.Initialize(cardData);

                EnemyList.Add(newEnemy);
            }
            SetAllPositions();

            curEnemy = null;
        }

        public void AddEnemy(Enemy enemy)
        {
            EnemyList.Add(enemy);
        }

        public void SetAllPositions()
        {
            RectTransform rectT = transform.Find("EnemySpawnArea").GetComponent<RectTransform>();
            int count = EnemyList.Count;

            float locXdelta = rectT.rect.width / (count + 1);
            
            var anchoredPosition = rectT.anchoredPosition;
            float locYup = anchoredPosition.y - 50f;
            float locYdown = anchoredPosition.y + 50f;

            float curX = anchoredPosition.x + rectT.rect.xMin + locXdelta;
            for (int i = 0; i < count; i++)
            {
                float curY = locYup;
                if (i % 2 == 0) curY = locYdown;

                RectTransform enemyRectT = EnemyList[i].GetComponent<RectTransform>();
                enemyRectT.anchoredPosition = new Vector3(curX, curY, 0f);
                
                var rect = enemyRectT.rect;
                float curSizeD = Mathf.Min(locXdelta, enemyRectT.sizeDelta.x);
                enemyRectT.sizeDelta = new Vector2(curSizeD, rect.height);

                var rectHealthTrans = EnemyList[i].GetComponentInChildren<HealthBar>().GetComponent<RectTransform>();
                rectHealthTrans.sizeDelta = 
                    new Vector2(curSizeD, rectHealthTrans.rect.height);
                EnemyList[i].GetComponentInChildren<HealthBar>().SetRectSize();
                    
                curX += locXdelta;
            }
        }

        public bool IsAllEnemyDead()
        {
            bool ret = true;
            
            foreach (Enemy enemy in EnemyList)
                if (!enemy.GetComponent<Health>().IsDead())
                {
                    ret = false;
                    break;
                }

            return ret;
        }

        public void Destroy(Enemy enemy)
        {
            EnemyList.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        private IEnumerator NextEnemy()
        {
            foreach (Enemy enemy in EnemyList)
                if (!enemy.GetComponent<Health>().IsDead())
                {
                    curEnemy = enemy;
                    yield return null;
                }

            curEnemy = null;
            yield return null;
        }

        public void InitEnemy()
        {
            AttackSeq = NextEnemy();
        }
        
        public Enemy GetNextEnemy()
        {
            AttackSeq.MoveNext();
            return curEnemy;
        }
    }
}