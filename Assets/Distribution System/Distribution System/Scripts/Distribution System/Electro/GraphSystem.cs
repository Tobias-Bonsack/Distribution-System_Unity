using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class GraphSystem
    {
        public static readonly Dictionary<string, Graph> baseGraphen = new Dictionary<string, Graph>();
        public static readonly Dictionary<string, Graph> combineGraphen = new Dictionary<string, Graph>();
        public static readonly Dictionary<string, HashSet<GraphMember>> graphs = new Dictionary<string, HashSet<GraphMember>>();
        public static void AddBaseGraph(string name)
        {
            if (baseGraphen.ContainsKey(name)) return;
            baseGraphen.Add(name, new Graph(name));
            graphs.Add(name, new HashSet<GraphMember>());
        }
        public static void AddCombineGraph(string[] couple, int code)
        {
            string combinedName = couple[0] + ":" + couple[1];

            if (combineGraphen.ContainsKey(combinedName))
            {
                combineGraphen[combinedName]._connections.Add(code);
                return;
            }

            combineGraphen.Add(combinedName, new Graph(combinedName));
            combineGraphen[combinedName]._connections.Add(code);

            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }
        public static void RemoveCombineGraph(string combineName, int code)
        {
            combineGraphen[combineName]._connections.Remove(code);
            if (combineGraphen[combineName]._connections.Count > 0) return;

            combineGraphen.Remove(combineName);
            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }
        public static void AddPowerSource(string baseName, int addValue)
        {
            baseGraphen[baseName].PowerLevel += addValue;
            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }
        private static void UpdatePowerLevel(string combineName)
        {
            List<string> visited = new List<string>();
            combineGraphen[combineName].PowerLevel = CalculatePowerLevel(visited, combineName);
        }
        private static int CalculatePowerLevel(List<string> visited, string combinedGraph)
        {
            int result = 0;
            visited.Add(combinedGraph);

            string[] vs = combinedGraph.Split(':');
            foreach (string baseGraph in vs)
            {
                if (!visited.Contains(baseGraph))
                {
                    visited.Add(baseGraph);
                    result += baseGraphen[baseGraph].PowerLevel;
                    List<string> cGs = GetCombinedGraphen(baseGraph);
                    foreach (string cG in cGs)
                    {
                        if (!visited.Contains(cG))
                        {
                            result += CalculatePowerLevel(visited, cG);
                        }
                    }
                }
            }
            return result;
        }
        private static void UpdateAllAbleToReceive()
        {
            foreach (string key in baseGraphen.Keys)
            {
                UpdateAbleToReceive(key);
            }
        }
        private static void UpdateAbleToReceive(string baseName)
        {
            foreach (GraphMember member in graphs[baseName])
            {
                bool ableToReceive = false;
                if (baseGraphen[baseName].PowerLevel > 0) ableToReceive = true;
                else
                {
                    foreach (string cG in GetCombinedGraphen(baseName))
                    {
                        if (combineGraphen[cG].PowerLevel > 0) ableToReceive = true;
                    }
                }
                member.AbleToReceive = ableToReceive;
            }
        }
        private static List<string> GetCombinedGraphen(string baseGraph)
        {
            List<string> result = new List<string>();

            foreach (string key in combineGraphen.Keys)
            {
                if (key.Contains(baseGraph)) result.Add(key);
            }

            return result;
        }
    }
}