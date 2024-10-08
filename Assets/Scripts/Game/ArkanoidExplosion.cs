using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.Game
{
    public class ArkanoidExplosion : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float _explosiveRadius = 1f;
        [SerializeField] private LayerMask _explosiveLayerMask;
        [SerializeField] private GameObject _explosionVfxPrefab;
        [SerializeField] private AudioClip _explosionAudioClip;

        #endregion

        #region Unity lifecycle

        private void Start()
        {
            Explode();
        }

        #endregion

        #region Public methods

        public void Explode()
        {
            AudioService.Instance.PlaySfx(_explosionAudioClip);

            if (_explosionVfxPrefab != null)
            {
                Instantiate(_explosionVfxPrefab, transform.position, Quaternion.identity);
            }

            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(transform.position, _explosiveRadius, _explosiveLayerMask);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.TryGetComponent(out Block block))
                {
                    block.ForceDestroy();
                }
            }

            Destroy(gameObject);
        }

        #endregion
    }
}