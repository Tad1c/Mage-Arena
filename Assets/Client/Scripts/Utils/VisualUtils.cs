using UnityEditor;
using UnityEngine;

public static class VisualUtils
{

    /// <summary>
    /// Minimum Smooth Speed we will set closingSpeed to in SmoothMove. 
    /// </summary>
    private const float k_MinSmoothSpeed = 6.0f;

    /// <summary>
    /// In SmoothMove we set a velocity proportional to our distance, to roughly approximate a spring effect.
    /// This is the constant we use for that calculation. 
    /// </summary>
    private const float k_TargetCatchupTime = 0.1f;

    /// <summary>
    /// Smoothly interpolates towards the parent transform. 
    /// </summary>
    /// <param name="moveTransform">The transform to interpolate</param>
    /// <param name="targetTransform">The transform to interpolate towards.  </param>
    /// <param name="timeDelta">Time in seconds that has elapsed, for purposes of interpolation.</param>
    /// <param name="closingSpeed">The closing speed in m/s. This is updated by SmoothMove every time it is called, and will drop to 0 whenever the moveTransform has "caught up". </param>
    public static void SmoothMove(Transform moveTransform, Vector3 targetPosition, float timeDelta, ref float closingSpeed)
    {
        var posDiff = targetPosition - moveTransform.position;
        float posDiffMag = posDiff.magnitude;

        if (posDiffMag > 0)
        {
            closingSpeed = Mathf.Max(closingSpeed, Mathf.Max(k_MinSmoothSpeed, posDiffMag / k_TargetCatchupTime));

            float maxMove = timeDelta * closingSpeed;
            float moveDist = Mathf.Min(maxMove, posDiffMag);
            posDiff *= (moveDist / posDiffMag);

            moveTransform.position += posDiff;

            if (moveDist == posDiffMag)
            {
                //we capped the move, meaning we exactly reached our target transform. Time to reset our velocity.
                closingSpeed = 0;
            }
        }
        else
        {
            closingSpeed = 0;
        }
    }
}