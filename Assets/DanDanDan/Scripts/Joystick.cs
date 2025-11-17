using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Assets.DanDanDan.Scripts 
{ 
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform joystickBackground;
        [SerializeField] private RectTransform joystickHandle;

        private Vector2 inputVector;

        public float Horizontal => inputVector.x;

        public float Vertical => inputVector.y;

        //Metodo de llamada de EventSystem - IDragHandler, se ejecuta mientras se mantenga presion
        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                joystickBackground,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 position ))
            {
                position.x = position.x / joystickBackground.sizeDelta.x * 2;
                position.y = position.y / joystickBackground.sizeDelta.y * 2;

                inputVector = (position.magnitude > 1.0f) ? position.normalized : position;

                joystickHandle.anchoredPosition = new Vector2
                    (
                    inputVector.x * (joystickBackground.sizeDelta.x / 2),
                    inputVector.y * (joystickBackground.sizeDelta.y / 2)
                    );
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
        }


    }

}
