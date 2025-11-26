//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

/// <summary>
/// Similar to SpringPosition, but also moves the panel's clipping. Works in local coordinates.
/// </summary>

[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	static public SpringPanel current;

	/// <summary>
	/// Target position to spring the panel to.
	/// </summary>

	public Vector3 target = Vector3.zero;

	/// <summary>
	/// Strength of the spring. The higher the value, the faster the movement.
	/// </summary>

	public float strength = 10f;

	public delegate void OnFinished ();

	/// <summary>
	/// Delegate function to call when the operation finishes.
	/// </summary>

	public OnFinished onFinished;

	[System.NonSerialized] UIPanel mPanel;
	[System.NonSerialized] Transform mTrans;
	[System.NonSerialized] UIScrollView mDrag;
	[System.NonSerialized] float mDelta = 0f;

	/// <summary>
	/// Cache the transform.
	/// </summary>

	public void Start ()
	{
		mPanel = GetComponent<UIPanel>();
		mDrag = GetComponent<UIScrollView>();
		mTrans = transform;
	}

	/// <summary>
	/// Advance toward the target position.
	/// </summary>

	void Update () { AdvanceTowardsPosition(); }

	/// <summary>
	/// Advance toward the target position.
	/// </summary>

	public virtual void AdvanceTowardsPosition (bool IM = false)
	{
        float delta = RealTime.deltaTime;

        bool trigger = false;
        Vector3 before = mTrans.localPosition;
        Vector3 after = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, delta);
        //     Debug.Log(target);
        if ((after - target).sqrMagnitude < 0.01f || IM)
        {
            after = target;
            enabled = false;
            trigger = true;
        }
        mTrans.localPosition = after;

        Vector3 offset = after - before;
        Vector2 cr = mPanel.clipOffset;
        cr.x -= offset.x;
        cr.y -= offset.y;
        mPanel.clipOffset = cr;

        if (mDrag != null) mDrag.UpdateScrollbars(false);

        if (trigger && onFinished != null)
        {
            current = this;
            onFinished();
            current = null;
        }
    }

	/// <summary>
	/// Start the tweening process.
	/// </summary>

	static public SpringPanel Begin (GameObject go, Vector3 pos, float strength)
	{
		var sp = go.GetComponent<SpringPanel>();
		if (sp == null) sp = go.AddComponent<SpringPanel>();
		sp.target = pos;
		sp.strength = strength;
		sp.onFinished = null;
		sp.enabled = true;
		return sp;
	}

	/// <summary>
	/// Stop the tweening process.
	/// </summary>

	static public SpringPanel Stop (GameObject go)
	{
		var sp = go.GetComponent<SpringPanel>();

		if (sp != null && sp.enabled)
		{
			if (sp.onFinished != null) sp.onFinished();
			sp.enabled = false;
		}
		return sp;
	}
}
