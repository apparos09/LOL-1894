using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RM_BBTS
{
    // used for tracking inputs with either the mouse or the touch.
    public class MouseTouchInput : MonoBehaviour
    {
        // If 'true', Mouse operations are tracked.
        public bool trackMouse = true;

        // if 'true', the Touch operations are tracked.
        public bool trackTouch = true;

        // Not implementing it for this since it's unneeded, but in the future you should set up callbacks.
        // A callback for the mouse interactions. It provides the mouse position in world space.
        public delegate void MouseCallback(GameObject mouseObject, Vector3 mouseWorldPosition);

        // A callback for touch interactions.
        public delegate void TouchCallback(List<GameObject> touchObjects, List<Touch> touches);

        // The mouse interaction.
        [Header("Mouse")]

        // If set to 'true', the UI is ignored for raycasting.
        // If set to 'false', the UI can block a raycast.
        [Tooltip("if true, the UI is ignored for raycast collisions. If false, UI elements can block a raycast.")]
        public bool ignoreUI = true;

        // The mouse key for mouse operations. The default is Keycode.Mouse0, which is the left mouse button.
        public KeyCode mouseKey = KeyCode.Mouse0;

        // The world position of the mouse.
        // NOTE: it appears that the touch input is detected as a mouse input as well. The latest input overrides this variable.
        public Vector3 mouseWorldPosition;

        // The callback for mouse down.
        private MouseCallback mouseHeldCallback = null;

        // The callback for the mouse hovered.
        private MouseCallback mouseHoveredCallback = null;

        
        [Header("Mouse/Interactions")]

        // The object the mouse is hovering over.
        public GameObject mouseHoveredObject = null;

        // The object that has been clicked and held on.
        // When the mouse button is released, this is set to null. This variable gets set to null when the mouse button is released.
        public GameObject mouseHeldObject = null;

        // The last object that was clicked on. The next time someone clicks on something, this will be set to null.
        public GameObject mouseLastClickedObject = null;

        [Header("Touch")]

        // These could be put into one struct, but then it wouldn't show up in the inspector.
        // TODO: maybe change this for later.

        // NOTE: the header won't show up if the first object is not something that can be shown to the inspector.
        // The touch list cannot be shown to the inspector, so the header does not appear.
        // The order has been changed to allow the header to show up.

        // The list of currently touched objects.
        // If no object has been touched then the shared index is set to null.
        // This will always be the same size as currentTouches. If no object was touched then the element will be empty.
        public List<GameObject> touchObjects = new List<GameObject>();

        // The current touches. This does NOT show up in the inspector.
        // The touched object will be removed from the list when it is let go, but the amount of touches will be retained...
        // for the saved touch.
        public List<Touch> currentTouches = new List<Touch>();

        // The callback for touches.
        private TouchCallback touchDownCallback = null;

        // Start is called before the first frame update.
        void Start()
        {

        }

        // Gets the screen to world point using the main camera.
        public static Vector3 GetScreenToWorldPoint(Vector3 position)
        {
            return GetScreenToWorldPoint(Camera.main, position);
        }

        // Gets the screen to world point.
        public static Vector3 GetScreenToWorldPoint(Camera cam, Vector2 position)
        {
            if (cam.orthographic) // orthographic (2d camera) - uses near clip plane so that it's guaranteed to be positive.
                return cam.ScreenToWorldPoint(new Vector3(position.x, position.y, cam.nearClipPlane));
            else // perspective (3d camera)
                return cam.ScreenToWorldPoint(new Vector3(position.x, position.y, cam.focalLength));
        }

        // MOUSE //

        // Checks to see if the cursor is in the window.
        public static bool MouseInWindow()
        {
            return MouseInWindow(Camera.main);
        }

        // Checks to see if the cursor is in the window.
        public static bool MouseInWindow(Camera cam)
        {
            // Checks the x-axis and the y-axis.
            bool inX, inY;

            // Gets the viewport position.
            Vector3 viewPos = cam.ScreenToViewportPoint(Input.mousePosition);

            // Checks the horizontal and vertical.
            inX = (viewPos.x >= 0 && viewPos.x <= 1.0);
            inY = (viewPos.y >= 0 && viewPos.y <= 1.0);

            return (inX && inY);
        }

        // Gets the mouse position in screen space.
        public static Vector3 GetMousePositionInScreenSpace()
        {
            return Input.mousePosition;
        }

        // Gets the mouse position in viewport space.
        public static Vector3 GetMousePositionInViewport()
        {
            return Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        // Gets the mouse position in world space using the main camera.
        public static Vector3 GetMousePositionInWorldSpace()
        {
            // Gets the screen to world point of the mouse.
            return GetScreenToWorldPoint(Input.mousePosition);
        }

        // Gets the mouse position in world space.
        public static Vector3 GetMousePositionInWorldSpace(Camera cam)
        {
            // gets the screen to world point.
            return GetScreenToWorldPoint(cam, Input.mousePosition);
        }

        // Gets the mosut target position in world space using the main camera.
        public static Vector3 GetMouseTargetPositionInWorldSpace(GameObject refObject)
        {
            return GetMouseTargetPositionInWorldSpace(refObject.transform.position);
        }

        // Gets the mouse target position in world space with a reference vector.
        public static Vector3 GetMouseTargetPositionInWorldSpace(Vector3 refPos)
        {
            Vector3 camWPos = GetMousePositionInWorldSpace(Camera.main);
            Vector3 target = camWPos - refPos;
            return target;
        }

        // TOUCH INPUT
        // Gets the list of touches.
        public static Touch[] GetTouches()
        {
            // checks the touch count.
            if (Input.touchCount != 0)
            {
                // returns the inputs as an array.
                return Input.touches;
            }
            else
            {
                // returns an empty array.
                return new Touch[0];

            }
        }

        // Gets the touch position in world space using the main camera.
        public static Vector3 GetTouchPositionInWorldSpace(Touch touch)
        {
            return GetTouchPositionInWorldSpace(Camera.main, touch);
        }

        // Gets the touch position in world space.
        public static Vector3 GetTouchPositionInWorldSpace(Camera cam, Touch touch)
        {
            Vector3 wPos = GetScreenToWorldPoint(cam, touch.position);
            return wPos;
        }

        // Gets the touch target position in world space using the main camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Touch touch, GameObject refObject)
        {
            return GetTouchTargetPositionInWorldSpace(Camera.main, touch, refObject.transform.position);
        }

        // Gets the touch target position in world space using the provided camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Camera cam, Touch touch, GameObject refObject)
        {
            return GetTouchTargetPositionInWorldSpace(cam, touch, refObject.transform.position);
        }

        // Gets the touch target position in world space using the main camera.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Touch touch, Vector3 refPos)
        {
            return GetTouchTargetPositionInWorldSpace(Camera.main, touch, refPos);
        }

        // Gets the touch target position in world space with a reference vector.
        public static Vector3 GetTouchTargetPositionInWorldSpace(Camera cam, Touch touch, Vector3 refPos)
        {
            Vector3 camWPos = GetTouchPositionInWorldSpace(cam, touch);
            Vector3 target = camWPos - refPos;
            return target;
        }

        // Gets the touched object. Returns null if object is not in list, or if no object was touched.
        public GameObject GetTouchedObject(Touch touch)
        {
            // Checks if it's in the list.
            if (currentTouches.Contains(touch)) // In list.
            {
                int index = currentTouches.IndexOf(touch);
                return touchObjects[index];
            }
            else // Not in list.
            {
                return null;
            }
        }


        // UPDATES //
        // Check the mouse input.
        private void MouseUpdate()
        {
            Vector3 target; // ray's target
            Ray ray; // ray object
            RaycastHit hitInfo; // info on hit.
            bool rayHit; // true if the ray hit.

            // if 'ignoreUI' is true, this is always false (the UI will never block the ray).
            // if 'ignoreUI' is false, then a check is done to see if a UI element is blocking the ray.
            bool rayBlocked = (ignoreUI) ? false : EventSystem.current.IsPointerOverGameObject();

            // gets the mouse position.
            mouseWorldPosition = GetMousePositionInWorldSpace();

            // if the ray is not blocked.
            if (!rayBlocked)
            {
                // checks if the camera is perspective or orthographic.
                if (Camera.main.orthographic) // orthographic
                {
                    // tries to get a hit. Since it's orthographic, the ray goes straight forward.
                    target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                    // ray position is mouse position in world space.
                    ray = new Ray(mouseWorldPosition, target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // cast the ray about as far as the camera can see.
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }
                else // perspective
                {
                    // the target of the ray.
                    target = GetMouseTargetPositionInWorldSpace(Camera.main.gameObject);

                    // ray object. It offsets so that objects not in the camera's clipping plane will be ignored.
                    ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane,
                        target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // the max distance
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }


                // checks if the ray got a hit. If it did, save the object the mouse is hovering over.
                // also checks if object has been clicked on.
                if (rayHit)
                {
                    mouseHoveredObject = hitInfo.collider.gameObject;

                    // left mouse button has been clicked, so save to held object as well.
                    if (Input.GetKeyDown(mouseKey))
                    {
                        mouseHeldObject = hitInfo.collider.gameObject;
                        mouseLastClickedObject = mouseHeldObject;
                    }
                }
                else
                {
                    // if the camera is orthographic, attempt a 2D raycast as well.
                    if (Camera.main.orthographic)
                    {
                        // setting up the 2D raycast for the orthographic camera.
                        RaycastHit2D rayHit2D = Physics2D.Raycast(
                            new Vector2(mouseWorldPosition.x, mouseWorldPosition.y),
                            new Vector2(target.normalized.x, target.normalized.y),
                            Camera.main.farClipPlane - Camera.main.nearClipPlane
                            );

                        // if a collider was hit, then the rayhit was successful.
                        rayHit = rayHit2D.collider != null;

                        // checks rayHit value.
                        if (rayHit)
                        {
                            // the ray hit was successful.
                            rayHit = true;

                            // saves the hovered over object.
                            mouseHoveredObject = rayHit2D.collider.gameObject;

                            // left mouse button has been clicked, so save to clicked object as well.
                            if (Input.GetKeyDown(mouseKey))
                            {
                                mouseHeldObject = rayHit2D.collider.gameObject;
                                mouseLastClickedObject = mouseHeldObject;
                            }
                        }
                    }

                    // if ray hit was not successful.
                    // this means the 3D raycast failed, and the 2D raycast (orthographic only).
                    if (!rayHit)
                    {
                        // no object beng hovered over.
                        mouseHoveredObject = null;

                        // mouse hasb een clicked down again.
                        if (Input.GetKeyDown(mouseKey))
                            mouseLastClickedObject = null;
                    }
                }
            }

            // left mouse button released, so clear clicked object.
            if (Input.GetKeyUp(mouseKey))
                mouseHeldObject = null;
        }

        // Updates the touch tracking.
        public void TouchUpdate()
        {
            // clears out the list of current touches and touched objects.
            currentTouches.Clear();
            touchObjects.Clear();

            // If no touches are saved.
            if (Input.touchCount == 0)
                return;

            // Gets all of the touches.
            Touch[] touches = Input.touches;

            // Puts in the values from the new array.
            currentTouches = new List<Touch>(touches);

            // Goes through every touch.
            foreach (Touch touch in touches)
            {
                // The ray's target.
                Vector3 target;

                // The ray object to be casted, the hit info, and if it hit anything.
                Ray ray;
                RaycastHit hitInfo;
                bool rayHit;

                // Saves the touch position in world space.
                Vector2 touchWorldPos = GetTouchPositionInWorldSpace(touch);

                // The touched object.
                GameObject touchedObject = null;

                // Checks if the camera is perspective or orthographic.
                if (Camera.main.orthographic) // orthographic
                {
                    // Tries to get a hit. Since it's orthographic, the ray goes straight forward.
                    target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

                    // Ray position is touch position in world space.
                    ray = new Ray(touchWorldPos, target.normalized);

                    // the max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // cast the ray about as far as the camera can see.
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }
                else // perspective
                {
                    // the target of the ray.
                    target = GetTouchTargetPositionInWorldSpace(touch, Camera.main.gameObject);

                    // ray object. It offsets the positioning so that objects not in the camera's clipping plane will be ignored.
                    ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane,
                        target.normalized);

                    // The max distance is the far clip plane minus the near clip plane.
                    float maxDist = Camera.main.farClipPlane - Camera.main.nearClipPlane;

                    // the max distance
                    rayHit = Physics.Raycast(ray, out hitInfo, maxDist);
                }

                // RAY HIT CHECK

                // Checks if the ray got a hit. If it did, save the object the touch hit.
                if (rayHit)
                {
                    // Saves the touched object.
                    touchedObject = hitInfo.collider.gameObject;
                }
                else // No object touched.
                {
                    // if the camera is orthographic, attempt a 2D raycast as well.
                    if (Camera.main.orthographic)
                    {
                        // setting up the 2D raycast for the orthographic camera.
                        RaycastHit2D rayHit2D = Physics2D.Raycast(
                            new Vector2(touchWorldPos.x, touchWorldPos.y),
                            new Vector2(target.normalized.x, target.normalized.y),
                            Camera.main.farClipPlane - Camera.main.nearClipPlane
                            );

                        // If a collider was hit, then the rayhit was successful.
                        rayHit = rayHit2D.collider != null;

                        // Checks rayHit value.
                        if (rayHit)
                        {
                            // Saves the touched object.
                            touchedObject = rayHit2D.collider.gameObject;
                        }
                    }
                }

                // Adds the touched object.
                // If this is set to null, then the space will be empty.
                touchObjects.Add(touchedObject);

                // tap count saves how many times the object has been touched.
                // this works.
                // Debug.Log("Tap Count: " + touch.tapCount);
            }
        }

        // CALLBACKS
        // Mouse Callbacks
        // Mouse Hovered - Add
        public void OnMouseHoveredAddCallback(MouseCallback callback)
        {
            mouseHoveredCallback += callback;
        }

        // Mouse Hovered - Remove.
        public void OnMouseHoveredRemoveCallback(MouseCallback callback)
        {
            mouseHoveredCallback -= callback;
        }

        // Mouse Held - Add
        public void OnMouseHeldAddCallback(MouseCallback callback)
        {
            mouseHeldCallback += callback;
        }

        // Mouse Held - Remove
        public void OnMouseHeldRemoveCallback(MouseCallback callback)
        {
            mouseHeldCallback -= callback;
        }

        // Touch Callbacks
        // Touch Down - Add
        public void OnTouchDownAddCallback(TouchCallback callback)
        {
            touchDownCallback += callback;
        }

        // Touch Down - Remove
        public void OnTouchRemoveCallback(TouchCallback callback)
        {
            touchDownCallback -= callback;
        }

        // Triggers the callbacks.
        private void TriggerCallbacks()
        {
            // Track mouse input.
            if(trackMouse)
            {
                // Mouse hovered callback.
                if (mouseHoveredCallback != null && mouseHoveredObject != null)
                    mouseHoveredCallback(mouseHoveredObject, mouseWorldPosition);

                // Mouse down callback.
                if (mouseHeldCallback != null && mouseHeldObject != null)
                    mouseHeldCallback(mouseHeldObject, mouseWorldPosition);
            }

            // Track touch input.
            if(trackTouch)
            {
                // Touch down callback.
                if (touchDownCallback != null && currentTouches.Count != 0)
                    touchDownCallback(touchObjects, currentTouches);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            // If the mouse should be tracked.
            if (trackMouse)
                MouseUpdate();

            // If the touch should be tracked.
            if (trackTouch)
                TouchUpdate();

            // Trigger the saved callbacks.
            TriggerCallbacks();
        }
    }
}