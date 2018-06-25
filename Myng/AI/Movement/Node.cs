using System;

namespace Myng.AI.Movement
{
    public class Node: IComparable
    {
        #region Properties

        public float GScore = float.MaxValue;

        public float FScore = float.MaxValue;

        public Node CameFrom = null;

        public float neighbourDist = 1;

        public bool IsFree = true;

        public bool IsTerrain = false;

        public int X;

        public int Y;

        public int I;

        public int J;

        #endregion

        #region Constructors

        public Node(int x, int y, int i, int j)
        {
            X = x;
            Y = y;
            I = i;
            J = j;
        }

        public int CompareTo(object obj)
        {
            Node cmpr = (Node)obj;

            if (FScore < cmpr.FScore) return -1;
            if (FScore > cmpr.FScore) return 1;
            if (FScore == cmpr.FScore) return 0;

            throw new InvalidOperationException("cannot compare Nodes");
        }

        #endregion
    }
}
