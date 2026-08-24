using UnityEngine;

namespace KevinIglesias
{
    public class BowStringController : MonoBehaviour
    {
        public LineRenderer bowstringLine;

        public Transform tip01;
        public Transform tip02;
        public Transform nockPoint;

        public Transform bowstringAnchorPoint;

        public Transform limb01;
        public Transform limb02;

        public AnimationCurve bowReleaseCurve;

        private Vector3 nockPointRestLocalPosition;
        private Vector3 initialLimb01LocalEulerAngles;
        private Vector3 initialLimb02LocalEulerAngles;

        private Coroutine bowAnimation;

        void OnEnable()
        {
            if (nockPoint)
            {
                nockPointRestLocalPosition = nockPoint.localPosition;
            }

            if (limb01 && limb02)
            {
                initialLimb01LocalEulerAngles = limb01.localEulerAngles;
                initialLimb02LocalEulerAngles = limb02.localEulerAngles;
            }
        }

        void LateUpdate()
        {
            CreateBowstring();
        }

        void CreateBowstring()
        {
            if (!bowstringLine || !tip01 || !tip02 || !nockPoint)
                return;

            bowstringLine.enabled = true;
            bowstringLine.useWorldSpace = true;
            bowstringLine.positionCount = 3;

            bowstringLine.startWidth = 0.01f;
            bowstringLine.endWidth = 0.01f;

            bowstringLine.SetPosition(0, tip01.position);
            bowstringLine.SetPosition(1, nockPoint.position);
            bowstringLine.SetPosition(2, tip02.position);
        }
        public void LoadBow(float duration)
        {
            if (bowAnimation != null)
            {
                StopCoroutine(bowAnimation);
            }
            bowAnimation = StartCoroutine(LoadBowCoroutine(duration));
        }

        public void ShootBow(float duration)
        {
            if (bowAnimation != null)
            {
                StopCoroutine(bowAnimation);
            }
            bowAnimation = StartCoroutine(ShootBowCoroutine(duration));
        }

        private System.Collections.IEnumerator LoadBowCoroutine(float duration)
        {
            Vector3 limb01LoadLocalEulerAngles = new Vector3(initialLimb01LocalEulerAngles.x, initialLimb01LocalEulerAngles.y, initialLimb01LocalEulerAngles.z - 15f);
            Vector3 limb02LoadLocalEulerAngles = new Vector3(initialLimb02LocalEulerAngles.x, initialLimb02LocalEulerAngles.y, initialLimb02LocalEulerAngles.z - 15f);

            nockPoint.localPosition = nockPointRestLocalPosition;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                limb01.localEulerAngles = Vector3.Lerp(initialLimb01LocalEulerAngles, limb01LoadLocalEulerAngles, t);
                limb02.localEulerAngles = Vector3.Lerp(initialLimb02LocalEulerAngles, limb02LoadLocalEulerAngles, t);

                nockPoint.position = Vector3.Lerp(nockPoint.position, bowstringAnchorPoint.position, t);

                yield return null;
            }
        }

        private System.Collections.IEnumerator ShootBowCoroutine(float duration)
        {
            Vector3 limb01LoadLocalEulerAngles = new Vector3(initialLimb01LocalEulerAngles.x, initialLimb01LocalEulerAngles.y, initialLimb01LocalEulerAngles.z - 15f);
            Vector3 limb02LoadLocalEulerAngles = new Vector3(initialLimb02LocalEulerAngles.x, initialLimb02LocalEulerAngles.y, initialLimb02LocalEulerAngles.z - 15f);

            Vector3 initialNockRestLocalPosition = nockPoint.localPosition;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                float curveValue = bowReleaseCurve != null ? bowReleaseCurve.Evaluate(t) : t;

                limb01.localEulerAngles = Vector3.LerpUnclamped(limb01LoadLocalEulerAngles, initialLimb01LocalEulerAngles, curveValue);
                limb02.localEulerAngles = Vector3.LerpUnclamped(limb02LoadLocalEulerAngles, initialLimb02LocalEulerAngles, curveValue);

                nockPoint.localPosition = Vector3.LerpUnclamped(initialNockRestLocalPosition, nockPointRestLocalPosition, curveValue);

                yield return null;
            }
        }
    }
}