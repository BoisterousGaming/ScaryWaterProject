using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHandler : MonoBehaviour
{
    Vector3 mv3PlayerPos = Vector3.zero;
	Material mFogMaterial;
	Transform mTransform;

    public GameObject _fog = null;
    public float _scrollSpeed = 0.01f;
    public Renderer _renderer;

    void Start ()
    {
        if (_fog != null)
            _renderer = _fog.GetComponent<Renderer>();
        
        mFogMaterial = _fog.GetComponent<Renderer>().material;
        mTransform = transform;
        mv3PlayerPos = new Vector3(0.0f, 10.07f, 0.0f);
    }

	void Update ()
    {
        if (_fog != null)
        {
            float offset = Time.time * _scrollSpeed;
            _renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0.17f));
        }

        if (!DayNightHandler.Instance._bNightTime)
            StartCoroutine(FadeTo(0.25f, 1f));

        if (DayNightHandler.Instance._bNightTime)
            StartCoroutine(FadeTo(0.0f, 1f));

        mv3PlayerPos.z = PlayerManager.Instance._playerHandler._tPlayerTransform.position.z + 30.0f;
        mTransform.position = Vector3.Lerp(mTransform.position, mv3PlayerPos, Time.deltaTime * 4.0f);
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = mFogMaterial.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            mFogMaterial.color = newColor;
            yield return null;
        }
    }
}
