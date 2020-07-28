using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class SpriteAnimation : MonoBehaviour
{
	private Image ImageSource;
	private int mCurFrame = 0;
	private float mDelta = 0;
	private float mTotalTime = 0;

	public Sprite[] SpriteFrames;
	public bool IsPlaying = false;
	public bool Foward = true;
	public bool AutoPlay = false;
	public bool Loop = false;

	public int FrameCount
	{
		get
		{
			return SpriteFrames.Length;
		}
	}

	void Awake()
	{
		ImageSource = GetComponent<Image>();
	}

	void Start()
	{
		if (AutoPlay)
		{
			Play();
		}
		else
		{
			IsPlaying = false;
		}
	}

	private void SetSprite(int idx)
	{
		ImageSource.sprite = SpriteFrames[idx];
		ImageSource.SetNativeSize();
	}

	public void Play()
	{
		IsPlaying = true;
		Foward = true;
	}

	public void PlayReverse()
	{
		IsPlaying = true;
		Foward = false;
	}

	void Update()
	{
		//此句基本无意义，因为隐藏了 不会update
		if(!this.isActiveAndEnabled){
			return;
		}

		if (!IsPlaying || 0 == FrameCount)
		{
			return;
		}

		mDelta += Time.deltaTime;
		mTotalTime += Time.deltaTime;

		// 10s后超时 最多显示10秒，避免卡死
		if (mTotalTime > 10.0) {

			this.transform.parent.gameObject.SetActive (false);
			mTotalTime = 0;

			return;
		}

		if (mDelta > 1.0 / SpriteFrames.Length)
		{
			mDelta = 0;
			if(Foward)
			{
				mCurFrame++;
			}
			else
			{
				mCurFrame--;
			}

			if (mCurFrame >= FrameCount)
			{
				if (Loop)
				{
					mCurFrame = 0;
				}
				else
				{
					IsPlaying = false;
					return;
				}
			}
			else if (mCurFrame<0)
			{
				if (Loop)
				{
					mCurFrame = FrameCount-1;
				}
				else
				{
					IsPlaying = false;
					return;
				}          
			}

			SetSprite(mCurFrame);
		}
	}

	public void Pause()
	{
		IsPlaying = false;
	}

	public void Resume()
	{
		if (!IsPlaying)
		{
			IsPlaying = true;
		}
	}

	public void Stop()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		IsPlaying = false;
	}

	public void Rewind()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		Play();
	}
}