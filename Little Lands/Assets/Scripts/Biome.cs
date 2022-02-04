using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    GameObject biomeBody;
    
    [System.Serializable]
    struct Fusion
    {
        public Biome fusionPartner;
        public Biome result;

        /// <summary>
        /// The visual effect prefab that should spawn upon this fusion happening.
        /// </summary>
        public GameObject effects;
    }

    [SerializeField]
    Fusion[] fusions;
}
