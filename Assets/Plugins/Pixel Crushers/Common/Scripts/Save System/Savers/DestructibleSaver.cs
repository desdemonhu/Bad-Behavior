﻿// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System;

namespace PixelCrushers
{

    /// <summary>
    /// Saves when a GameObject has been destroyed or disabled. The next time the game
    /// or scene is loaded, if the GameObject has previously been destroyed/disabled, this
    /// script will destroy/deactivate it again. It will also spawn a replacement destroyed
    /// version if a prefab is assigned.
    /// </summary>
    [AddComponentMenu("")]
    public class DestructibleSaver : Saver
    {

        [Serializable]
        public class DestructibleData
        {
            public bool destroyed = false;
            public Vector3 position;
        }

        public enum Mode { OnDisable, OnDestroy }

        [Tooltip("Event to watch for.")]
        [SerializeField]
        private Mode m_mode = Mode.OnDestroy;

        [Tooltip("Instantiate this if already destroyed when loading game or scene.")]
        private GameObject m_destroyedVersionPrefab;

        private DestructibleData m_data = new DestructibleData();
        private bool m_ignoreOnDestroy = false;

        public Mode mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public GameObject destroyedVersionPrefab
        {
            get { return m_destroyedVersionPrefab; }
            set { m_destroyedVersionPrefab = value; }
        }

        public override void OnBeforeSceneChange()
        {
            base.OnBeforeSceneChange();
            m_ignoreOnDestroy = true;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (m_mode != Mode.OnDisable) return;
            RecordDestruction();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (m_mode != Mode.OnDestroy) return;
            RecordDestruction();
        }

        protected virtual void RecordDestruction()
        {
            if (!m_ignoreOnDestroy)
            {
                m_data.destroyed = true;
                m_data.position = transform.position;
                SaveSystem.UpdateSaveData(this, SaveSystem.Serialize(m_data));
            }
            m_ignoreOnDestroy = false;
        }

        public override string RecordData()
        {
            return SaveSystem.Serialize(m_data);
        }

        public override void ApplyData(string data)
        {
            var destructibleData = SaveSystem.Deserialize<DestructibleData>(data);
            if (destructibleData == null) return;
            m_data = destructibleData;
            if (destructibleData.destroyed)
            {
                if (destroyedVersionPrefab != null)
                {
                    Instantiate(destroyedVersionPrefab, destructibleData.position, transform.rotation);
                }
                Destroy(gameObject);
            }
        }

    }

}
