using System.Collections.Generic;
using UnityEngine;

namespace Frog
{
    public class Frogs : MonoBehaviour
    {
        [SerializeField] private List<GameObject> activeFrogs = new();
        public List<GameObject> ActiveFrogs => activeFrogs;
    }
}