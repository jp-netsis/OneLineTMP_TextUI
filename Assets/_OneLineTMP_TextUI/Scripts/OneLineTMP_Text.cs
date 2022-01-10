using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

namespace jp.netsis.TMPUtility
{
    [RequireComponent(typeof(TMP_Text))]
    public class OneLineTMP_Text : MonoBehaviour
    {
        private TMP_Text _tmpText;
        private RectTransform _rectTransform;
        private RectTransform _parentRectTransform;

        public float _scrollSpeed = 0.1f;
        public float _scrollStartWaitTime = 0.5f;
        public float _scrollRestartWaitTime = 0.1f;

        private string _text;
        public string text
        {
            set
            {
                var prefferdSize= _tmpText.GetPreferredValues(value);
                _tmpText.rectTransform.sizeDelta = prefferdSize;
                var nowPos = _rectTransform.anchoredPosition;
                nowPos.x = 0f;
                _rectTransform.anchoredPosition = nowPos;
                if (_parentRectTransform.sizeDelta.x < _tmpText.rectTransform.sizeDelta.x)
                {
                    StartScrollCoroutine();
                }
                _text = value;
                _tmpText.text = value;
            }
            get => _text;
        }

        private Coroutine _scrollCoroutine;
        private WaitForSeconds _startWaitForSeconds;
        private WaitForSeconds _restartWaitForSeconds;

        private void OnEnable()
        {
            _tmpText = GetComponent<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
            _parentRectTransform = transform.parent.GetComponent<RectTransform>();
            if (_parentRectTransform.sizeDelta.x < _tmpText.rectTransform.sizeDelta.x)
            {
                StartScrollCoroutine();
            }

            text = _tmpText.text; // Set Immidiate Size

            _startWaitForSeconds = new WaitForSeconds(_scrollStartWaitTime);
            _restartWaitForSeconds = new WaitForSeconds(_scrollRestartWaitTime);
        }

        IEnumerator ScrollUpdate()
        {
            yield return _startWaitForSeconds;
            while (true)
            {
                var nowPos = _rectTransform.anchoredPosition;
                nowPos.x -= _scrollSpeed;
                if (nowPos.x + _rectTransform.sizeDelta.x < 0)
                {
                    yield return _restartWaitForSeconds;
                    nowPos.x = _rectTransform.sizeDelta.x;
                }
                _rectTransform.anchoredPosition = nowPos;
                yield return null;
            }
        }

        void StartScrollCoroutine()
        {
            if (_scrollCoroutine != null)
            {
                StopScrollCoroutine();
            }
            _scrollCoroutine = StartCoroutine(ScrollUpdate());
        }

        void StopScrollCoroutine()
        {
            if (_scrollCoroutine != null)
            {
                StopCoroutine(_scrollCoroutine);
                _scrollCoroutine = null;
            }
        }
        
        private void OnDisable()
        {
            StopScrollCoroutine();
        }
    }
}
