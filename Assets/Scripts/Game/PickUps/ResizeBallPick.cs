using Arkanoid.Services;
using UnityEngine;

namespace Arkanoid.Game.PickUps
{
    public class ResizeBallPick : PickUp
    {
        #region Variables

        [SerializeField] private float _resizeCoefficient = 1.5f;

        #endregion

        #region Protected methods

        protected override void PerformActions()
        {
            base.PerformActions();

            LevelService.Instance.RescaleBalls(_resizeCoefficient);
        }

        #endregion
    }
}