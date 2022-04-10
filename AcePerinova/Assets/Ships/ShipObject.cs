using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AcePerinova.Selectables{
    /// <summary>
    /// Stores data about a specific ship's stats, select art, name, weapons, and potentially sound effects, and more.
    /// </summary>
    [CreateAssetMenu(menuName = "Selectables/Ships")]
    public class ShipObject : ScriptableObject
    {
        public string ShipName;
        [TextArea(1,25)]
        public string bio;
        public Texture2D nameArt, selectArt;

        public float
            acceleration = 30,
            pitch = 150,
            yaw = 80,
            roll = 200;

    }
}


