using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

using Buffs;
using Effects;
using UI;
using Units;

namespace Managers
{
    public class BuffManager : MonoBehaviour
    {
        public List<Buff> BuffList;
        
        [NonSerialized]
        public float offsetMargin = 15f;
        [NonSerialized]
        public float scaleFactor = .4f;
        
        [SerializeField]
        protected Vector3 QueueCenter;

        [SerializeField]
        protected QueueAlignType align = QueueAlignType.Right;
        protected float StartingIndex;
        protected float TotalBuffWidth;
        
        public void Start()
        {
            if (GetComponent<Unit>() == null)
            {
                throw new ArgumentException("BuffManager is not a component of a Unit!");
            }

            RectTransform rect = transform.Find("BuffQueuePositionHolder").GetComponent<RectTransform>();
            QueueCenter = rect.anchoredPosition;
            TotalBuffWidth = rect.sizeDelta.x * scaleFactor + offsetMargin;
            SetAlign();
        }

        protected void SetAlign()
        {
            switch (align)
            {
                case QueueAlignType.Left:
                    StartingIndex = 0;
                    break;
                case QueueAlignType.Middle:
                    StartingIndex = (float)(BuffList.Count - 1) / 2;
                    break;
                case QueueAlignType.Right:
                    StartingIndex = BuffList.Count - 1;
                    break;
            }
        }

        protected void AdjustPosition(int index, bool setAlign = false)
        {
            if (setAlign) SetAlign();

            Transform thisTrans = BuffList[index].transform;
            thisTrans.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            
            // thisTrans.GetComponent<RectTransform>().localScale

            Vector3 newPos = new Vector3(
                QueueCenter.x + TotalBuffWidth * (index - StartingIndex),
                QueueCenter.y,
                QueueCenter.z);
            
            thisTrans.GetComponent<RectTransform>().anchoredPosition = newPos;
        }

        public void AdjustAllPositions()
        {
            SetAlign();
            for (var i = 0; i < BuffList.Count; i++)
            {
                AdjustPosition(i);
            }
        }
        
        public void Create(BuffType buffType, float buffAmount)
        {
            GameObject buffObject = Instantiate(Game.Ctx.CardOperator.buffPrefab, transform);
            buffObject.name = buffType.ToString("g");
            Buff buff = buffObject.GetComponent<Buff>();
            buff.Initialize(buffType, buffAmount);
            
            BuffList.Add(buff);
            AdjustAllPositions();
        }
        
        /*
        public void Create(Buff buff)
        {
            BuffList.Add(buff);
            AdjustPosition();
        }
        */

        public void Destroy(Buff buff)
        {
            BuffList.Remove(buff);
            Object.Destroy(buff.gameObject);
            AdjustAllPositions();
        }
        
        public void DestroyAll()
        {
            foreach (Buff buff in BuffList)
            {
                Object.Destroy(buff.gameObject);
            }
            BuffList.Clear();
        }
    }
}