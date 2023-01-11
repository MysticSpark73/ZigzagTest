using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zagzag.Core.Data
{
    public static class Parameters
    {
        #region ObjectPoolerKeys

        public static readonly string pooler_key_part_zigzag = "PartZigzag";
        public static readonly string pooler_key_part_up = "PartUp";
        public static readonly string pooler_key_part_long_up = "PartLongUp";

        public static string GetRandomPoolerKey()
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    return pooler_key_part_zigzag;
                case 1:
                    return pooler_key_part_up;
                case 2:
                    return pooler_key_part_long_up;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
