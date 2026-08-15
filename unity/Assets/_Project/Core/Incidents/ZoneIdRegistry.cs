using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Incidents
{
    // Maintained list of known zone id strings, used by ZoneIdDrawer to render a dropdown
    // instead of a free-text field wherever a zoneId is authored.
    [CreateAssetMenu(menuName = "Valkirie/Incidents/Zone Id Registry")]
    public class ZoneIdRegistry : ScriptableObject
    {
        public List<string> knownZoneIds = new();
    }
}
