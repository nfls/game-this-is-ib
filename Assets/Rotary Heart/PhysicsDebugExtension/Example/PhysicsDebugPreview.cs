using UnityEngine;
using Physics = RotaryHeart.Lib.PhysicsExtension.Physics;

public class PhysicsDebugPreview : MonoBehaviour
{

    public enum PhysicsType
    {
        BoxSingle,
        BoxAll,
        BoxNonAlloc,

        CapsuleSingle,
        CapsuleAll,
        CapsuleNonAlloc,

        Line,

        RaySingle,
        RayAll,
        RayNonAlloc,

        SphereSingle,
        SphereAll,
        SphereNonAlloc,

        CheckBox,
        CheckCapsule,
        CheckSphere,

        OverlapBox,
        OverlapBoxNonAlloc,
        OverlapCapsule,
        OverlapCapsuleNonAlloc,
        OverlapSphere,
        OverlapSphereNonAlloc
    }

    public PhysicsType castType;
    public bool enableInEditor = true;
    public bool enableInRuntime = true;
    public float drawDuration;
    public Color hitColor = Color.green;
    public Color noHitColor = Color.red;
    public bool useRay;
    public float distance = 5;

    RaycastHit[] results = new RaycastHit[5];
    Collider[] colliderResults = new Collider[5];

    // Update is called once per frame
    void Update()
    {
        Vector3 startPoint = transform.position;
        Vector3 direction = transform.forward;
        Vector3 endPoint = startPoint + direction * distance;
        Ray ray = new Ray(startPoint, direction);

        switch (castType)
        {
            case PhysicsType.BoxSingle:
                if (enableInEditor) Physics.BoxCast(startPoint, Vector3.one, direction, transform.rotation, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.BoxCast(startPoint, Vector3.one, direction, transform.rotation, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.BoxAll:
                if (enableInEditor) Physics.BoxCastAll(startPoint, Vector3.one, direction, transform.rotation, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.BoxCastAll(startPoint, Vector3.one, direction, transform.rotation, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.BoxNonAlloc:
                if (enableInEditor) Physics.BoxCastNonAlloc(startPoint, Vector3.one, direction, results, transform.rotation, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.BoxCastNonAlloc(startPoint, Vector3.one, direction, results, transform.rotation, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.CapsuleSingle:
                if (enableInEditor) Physics.CapsuleCast(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CapsuleCast(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.CapsuleAll:
                if (enableInEditor) Physics.CapsuleCastAll(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CapsuleCastAll(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.CapsuleNonAlloc:
                if (enableInEditor) Physics.CapsuleCastNonAlloc(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, results, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CapsuleCastNonAlloc(startPoint - transform.up * 0.5f, startPoint + transform.up * 0.5f, 1, direction, results, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.Line:
                if (enableInEditor) Physics.Linecast(startPoint, endPoint, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.Linecast(startPoint, endPoint, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.RaySingle:
                if (useRay) {
                    if (enableInEditor) Physics.Raycast(ray, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.Raycast(ray, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.Raycast(startPoint, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.Raycast(startPoint, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.RayAll:
                if (useRay) {
                    if (enableInEditor) Physics.RaycastAll(ray, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.RaycastAll(ray, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.RaycastAll(startPoint, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.RaycastAll(startPoint, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.RayNonAlloc:
                if (useRay) {
                    if (enableInEditor) Physics.RaycastNonAlloc(ray, results, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.RaycastNonAlloc(ray, results, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.RaycastNonAlloc(startPoint, direction, results, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.RaycastNonAlloc(startPoint, direction, results, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.SphereSingle:
                if (useRay) {
                    if (enableInEditor) Physics.SphereCast(ray, 1, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCast(ray, 1, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.SphereCast(startPoint, 1, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCast(startPoint, 1, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.SphereAll:
                if (useRay) {
                    if (enableInEditor) Physics.SphereCastAll(ray, 1, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCastAll(ray, 1, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.SphereCastAll(startPoint, 1, direction, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCastAll(startPoint, 1, direction, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.SphereNonAlloc:
                if (useRay) {
                    if (enableInEditor) Physics.SphereCastNonAlloc(ray, 1, results, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCastNonAlloc(ray, 1, results, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                } else {
                    if (enableInEditor) Physics.SphereCastNonAlloc(startPoint, 1, direction, results, distance, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                    if (enableInRuntime) Physics.SphereCastNonAlloc(startPoint, 1, direction, results, distance, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                }
                
                break;
            case PhysicsType.CheckBox:
                if (enableInEditor) Physics.CheckBox(startPoint, Vector3.one * 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CheckBox(startPoint, Vector3.one * 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.CheckCapsule:
                if (enableInEditor) Physics.CheckCapsule(startPoint, endPoint, 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CheckCapsule(startPoint, endPoint, 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.CheckSphere:
                if (enableInEditor) Physics.CheckSphere(startPoint, 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.CheckSphere(startPoint, 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapBox:
                if (enableInEditor) Physics.OverlapBox(startPoint, Vector3.one * 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapBox(startPoint, Vector3.one * 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapBoxNonAlloc:
                if (enableInEditor) Physics.OverlapBoxNonAlloc(startPoint, Vector3.one * 3, colliderResults, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapBoxNonAlloc(startPoint, Vector3.one * 3, colliderResults, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapCapsule:
                if (enableInEditor) Physics.OverlapCapsule(startPoint, endPoint, 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapCapsule(startPoint, endPoint, 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapCapsuleNonAlloc:
                if (enableInEditor) Physics.OverlapCapsuleNonAlloc(startPoint, endPoint, 3, colliderResults, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapCapsuleNonAlloc(startPoint, endPoint, 3, colliderResults, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapSphere:
                if (enableInEditor) Physics.OverlapSphere(startPoint, 3, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapSphere(startPoint, 3, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
            case PhysicsType.OverlapSphereNonAlloc:
                if (enableInEditor) Physics.OverlapSphereNonAlloc(startPoint, 3, colliderResults, Physics.PreviewCondition.Editor, drawDuration, hitColor, noHitColor);
                if (enableInRuntime) Physics.OverlapSphereNonAlloc(startPoint, 3, colliderResults, Physics.PreviewCondition.Game, drawDuration, hitColor, noHitColor);
                break;
        }
    }
}
