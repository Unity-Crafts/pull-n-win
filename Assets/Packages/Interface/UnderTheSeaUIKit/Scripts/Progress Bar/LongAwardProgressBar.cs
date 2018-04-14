using UnityEngine;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	public sealed class LongAwardProgressBar : AwardProgressBar {
		public override bool hasFixedAwardCount { get { return false; } }

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();

			//This method can be called before OnEnable, so, we need to check for that!
			if(!IsActive()) return;

			//Update bar layout
			UpdateLayout();
		}

		public override void UpdateLayout() {
			var newSize = new Vector2((rectTransform.rect.height / inactiveAwardSprite.rect.height) * inactiveAwardSprite.rect.width, rectTransform.rect.height);

			foreach(var award in awards) {
				award.image.rectTransform.sizeDelta = newSize;
				award.image.rectTransform.anchoredPosition = new Vector2(fillBar.rectTransform.rect.x + (fillBar.rectTransform.rect.width * award.awardPercent), award.image.rectTransform.anchoredPosition.y);
			}
		}
	}
}