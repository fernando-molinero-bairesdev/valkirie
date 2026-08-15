using UnityEngine;

namespace Valkirie.Core.Incidents
{
    // Marks a string field as a zone id so the Editor can draw it as a dropdown
    // (see Core/Editor/ZoneIdDrawer.cs) sourced from a ZoneIdRegistry asset, instead of
    // free text that's one typo away from a trigger silently never firing.
    public class ZoneIdAttribute : PropertyAttribute { }
}
