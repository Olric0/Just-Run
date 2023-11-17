using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece Karakterin RUN ve JUMP Animasyon State'lerinde Var. Ba�ka Bir Yerde Kullan�lm�yor.
/// 
/// </summary>
public class AnimStateControllerForGame : StateMachineBehaviour
{
    // Bu Metotlar, Ate� Topunun Kullan�labilir Bir Durumda Olup Olmad���n� Belirliyor. Mesela
    // Bu Metotlar Sayesinde Karakter Sald�rma Animasyonuna Ge�ip Sald�r�rken [ canFireballBeUsed ]
    // De�i�keni False De�erinde Oldu�u ��in Oyuncu Sald�r�rken Ate� Topu F�rlat�p Oyunu Buga Sokam�yor.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => Character.chrctrTHIS.canFireballBeUsed = true;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => Character.chrctrTHIS.canFireballBeUsed = false;
}