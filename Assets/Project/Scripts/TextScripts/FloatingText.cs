using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMesh _textComponent;
    [SerializeField] private float _floatSpeed = 1.0f;
    [SerializeField] private float _fadeSpeed = 1.0f;

    private Camera _cameraToLookAt;

    private void Start()
    {
        _cameraToLookAt = Camera.main;
        Destroy(gameObject, 1.5f); // Automatically destroy after 1.5 seconds.
    }

    private void Update()
    {
        // Float the text upwards.
        transform.position += Vector3.up * _floatSpeed * Time.deltaTime;
        transform.LookAt(_cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(_cameraToLookAt.transform.forward);

        // Fade out the text.
        Color textColor = _textComponent.color;
        textColor.a -= _fadeSpeed * Time.deltaTime;
        _textComponent.color = textColor;
    }

    public void SetText(string text)
    {
        _textComponent.text = text;
    }
}
