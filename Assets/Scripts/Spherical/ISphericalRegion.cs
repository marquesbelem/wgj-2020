using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISphericalRegion {

    SphericalCoordinates SphereCoordinatesRef { get; }
    bool Contains(Vector3 dir);
    bool WouldOverlap(ISphericalRegion other, Vector3 hypotheticalUp);
    float AngularExtension(Vector3 dir);

}
