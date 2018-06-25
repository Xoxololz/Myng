using Myng.Helpers;
using System;
using System.Collections.Generic;

namespace Myng.AI.Movement
{
    public class PathFinder
    {

        #region Methods

        public List<Node> FindPath(Node start, Node goal, NodeMap nodeMap)
        {
            PriorityQueue<Node> openSet = new PriorityQueue<Node>();
            List<Node> closeSet = new List<Node>();
            start.FScore = HeuristicCost(start, goal);
            start.GScore = 0;
            openSet.Enqueue(start);

            Node current = null;
            while (openSet.Count > 0)
            {
                current = openSet.Dequeue();
                if (current == goal)
                {
                    return ReconstructPath(current);                    
                }

                closeSet.Add(current);
                foreach (var neighbour in nodeMap.GetNeighbours(current.I, current.J))
                {
                    if (closeSet.Contains(neighbour)) continue;
                    if (!openSet.Contains(neighbour)) openSet.Enqueue(neighbour);

                    float tentativeGScore = current.GScore + HeuristicCost(current, neighbour)*neighbour.neighbourDist;
                    if (tentativeGScore >= neighbour.GScore) continue;

                    neighbour.CameFrom = current;
                    neighbour.GScore = tentativeGScore;
                    neighbour.FScore = neighbour.GScore + HeuristicCost(neighbour, goal);
                }

            }


            throw new Exception("Couldnt find path");
        }


        private float HeuristicCost(Node node1, Node node2)
        {
            float dist = (float)Math.Sqrt(Math.Pow((node1.X - node2.X), 2) + Math.Pow((node1.Y - node2.Y), 2));
            return dist;
        }


        private List<Node> ReconstructPath(Node goal)
        {
            List<Node> path = new List<Node>();
            Node current = goal;
            path.Add(current);
            while (current.CameFrom != null)
            {
                current = current.CameFrom;
                path.Add(current);
            }
            return path;
        }
        #endregion
    }
}
