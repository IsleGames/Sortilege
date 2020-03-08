using System;
using UnityEngine;

namespace Managers
{
    public class SortOrderManager : MonoBehaviour
    {
        private int sortOrder = 0;

        private void Awake()
        {
            ResetSortOrder();
        }

        public void ResetSortOrder()
        {
            sortOrder = 0;
        }
        
        public int GetSortOrder()
        {
            int ret = sortOrder;
            sortOrder += 1;
            return ret;
        }
    }
}