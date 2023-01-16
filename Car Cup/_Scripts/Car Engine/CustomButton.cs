using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CarCup
{
    [RequireComponent(typeof(Image))]
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
    {
        public bool pressured = false;
        public int clickCount = 0;
        public CustomButtonType type = CustomButtonType.None;

        [Space]
        [SerializeField] private Color color = Color.red;

        private Image _image;
        private Color startColor = Color.white;
        private float clickTime = 0f;

        private void Awake()
        {
            _image = GetComponent<Image>();
            startColor = _image.color;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressured = true;
            _image.color = color;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressured = false;
            _image.color = startColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickCount++;
            clickTime = Time.time;
        }

        private void Update()
        {
            if (clickTime + 0.5f < Time.time) clickCount = 0;
        }
    }
}