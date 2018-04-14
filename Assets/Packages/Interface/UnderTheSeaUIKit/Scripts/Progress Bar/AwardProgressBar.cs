using UnityEngine;
using UnityEngine.Events;

namespace HungryCannibal.UnderTheSeaUIKit.ProgressBars {
	[System.Serializable]
	[ExecuteInEditMode]
	public abstract class AwardProgressBar : ProgressBar {

		[System.Serializable]
		public class ProgressAwardAwardedEvent : UnityEvent<ProgressAward> { }

		#region Properties
		public override float percent {
			get { return base.percent; }

			set {
				base.percent = value;

				//Update the awards based on the new percent
				foreach(var award in awards) {
					if(percent >= award.awardPercent) {
						//If we havent fired the 'awarded' event for this award yet, then fire it
						if(!award.eventFired) {
							award.eventFired = true;
							OnAwardAwarded.Invoke(award);
						}
					}
				}
			}
		}

		/// <summary>
		/// Return true if this progress bar has a fixed number of awards, otherwise, return false
		/// </summary>
		public abstract bool hasFixedAwardCount { get; }

		#endregion

		public ProgressAward[] awards;
		public Sprite activeAwardSprite;
		public Sprite inactiveAwardSprite;

		[Space]
		public ProgressAwardAwardedEvent OnAwardAwarded;
	}
}