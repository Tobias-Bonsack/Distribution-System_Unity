using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class Graph
    {
        public string _name;

        public HashSet<int> _connections = new HashSet<int>();
        private int _powerLevel = 0;
        public int PowerLevel
        {
            get
            {
                return _powerLevel;
            }
            set
            {
                _powerLevel = value;
            }
        }
        public Graph(string name)
        {
            _name = name;
        }
    }
}
