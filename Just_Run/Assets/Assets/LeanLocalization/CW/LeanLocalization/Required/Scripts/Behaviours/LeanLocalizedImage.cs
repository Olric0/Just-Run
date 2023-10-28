using UnityEngine;
using UnityEngine.UI;

namespace Lean.Localization
{
	/// <summary>
	/// 
	/// Görselin Çevirisini, Veya Dile Göre Texture'sini Bu Script Ayarlıyor.
	/// 
	/// </summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLocalizedImage")]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized Image")]
	public class LeanLocalizedImage : LeanLocalizedBehaviour
	{
        // Çevirinin, Yani Görselin Güncellenmesi Gereken Zamanlarda Çağırılıyor.
        public override void UpdateTranslation(LeanTranslation translation)
		{
            // Çeviri Burada Yapılıyor.
            if (translation != null && translation.Data is Sprite)
                GetComponent<Image>().sprite = (Sprite)translation.Data;
		}
	}
}