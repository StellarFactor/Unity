using System;
using UnityEngine;
using UnityEngine.UI;

namespace StellarFactor.Minimap
{
    public class MiniMap : Singleton<MiniMap>
    {
        #region Serialized Vars
        // ======================================================
        [Header("Debug")]
        [SerializeField] Logger log = new();

        [Header("Camera")]
        [SerializeField] private Camera cam;

        [Header("UI")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image background;
        [SerializeField] private RawImage map;
        #endregion //Serialized Vars


        #region Events
        // ======================================================
        public event Action NodeUpdates;
        #endregion // Events


        #region Unity
        // ======================================================
        private void Update()
        {
            NodeUpdates.Invoke();
        }
        #endregion // Unity


        #region Public Methods
        // ======================================================
        public void InstantiateNodeAt(IMapLocation location)
        {
            Node prefab = location.GetNodePrefab();
            Node instance = Instantiate(prefab, map.transform);

            location.InstantiatedNode = instance;
        }

        public void UpdateNode(IMapLocation location)
        {
            Vector2 viewportVec
                = cam.WorldToViewportPoint(location.GetPosition());

            float clampedX = Mathf.Clamp(viewportVec.x, 0, 1);
            float clampedY = Mathf.Clamp(viewportVec.y, 0, 1);
            Vector2 clampedViewportPos = new(clampedX, clampedY);

            Vector2 nodePos
                = clampedViewportPos * map.rectTransform.rect.size;

            location.InstantiatedNode.RT.anchoredPosition = nodePos;
            location.InstantiatedNode.SetColor(location.GetNodeColor());
        }
        #endregion // Public Methods
    }
}
