using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece ��retici Moddaki Karakterin RUN ve JUMP Animasyon State'lerinde Var. Ba�ka Bir Yerde Kullan�lm�yor.
/// 
///  >*>*> De�i�kenlerin A��klamalar� <*<*< \\\
///     -> [ isItSuitableForUse ]
///     Bu De�i�ken TutorialManager Scriptinin, Task4 Metodunda Tetikleniyor. A�a��daki Metodlar Ate�
///     Topunun Kontrol�n� Sa�lad��� ��in Ate� Topunun Aktif Olmad��� Zaman �al��mamas� Gerekiyor. Bu
///     De�i�ken �se Ate� Topu Aktif Olunca True Oluyor Ve Metodlar Kullan�labilir Hale Geliyor.
///     
/// </summary>
public class AnimStateControllerForTutorial : StateMachineBehaviour
{
    public static bool isItSuitableForUse;



    // Bu Metotlar, Ate� Topunun Kullan�labilir Bir Durumda Olup Olmad���n� Belirliyor. Mesela
    // Bu Metotlar Sayesinde Karakter Sald�rma Animasyonuna Ge�ip Sald�r�rken [ canFireballBeUsed ]
    // De�i�keni False De�erinde Oldu�u ��in Oyuncu Sald�r�rken Ate� Topu F�rlat�p Oyunu Buga Sokam�yor.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isItSuitableForUse == true)
            CharacterForTutorial.chrctrTHIS.canFireballBeUsed = true;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isItSuitableForUse == true)
            CharacterForTutorial.chrctrTHIS.canFireballBeUsed = false;
    }
}