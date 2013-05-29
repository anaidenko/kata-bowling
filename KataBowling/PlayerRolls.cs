using System.Collections.Generic;

namespace KataBowling
{
    public class PlayerRolls : List<int>
    {
        public SafePlayerRolls Safe
        {
            get { return new SafePlayerRolls(this); }
        }

        #region Constructors

        public PlayerRolls()
        {
        }

        public PlayerRolls(IEnumerable<int> collection)
            : base(collection)
        {
        }

        public PlayerRolls(params int[] collection)
            : base(collection)
        {
        }

        #endregion

        public class SafePlayerRolls : PlayerRolls
        {
            internal SafePlayerRolls(IEnumerable<int> collection)
            {
                AddRange(collection);
            }

            public new int this[int index]
            {
                get
                {
                    if (index >= Count) return 0;
                    return base[index];
                }
                set { base[index] = value; }
            }
        }
    }
}