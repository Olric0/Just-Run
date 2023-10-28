using UnityEngine;
using TMPro;

namespace Lean.Localization
{
	/// <summary>
	/// 
	/// TextMeshPro Metinlerini, Yani TextMeshProUGUI'leri Bu Script Çevirir.
	/// 
	/// </summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(TextMeshProUGUI))]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Localized TextMeshProUGUI")]
	public class LeanLocalizedTextMeshProUGUI : LeanLocalizedBehaviour
	{
		// Çevirinin Güncellenmesi Gereken Zamanlarda Çağırılıyor.
		public override void UpdateTranslation(LeanTranslation translation)
		{
            // Çevirinin Yapılacağı Text Get Ediliyor.
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

			// Çeviri Burada Yapılıyor.
			if (translation != null && translation.Data is string)
                text.text = LeanTranslation.FormatText((string)translation.Data, text.text, this, gameObject);
        }
	}
}