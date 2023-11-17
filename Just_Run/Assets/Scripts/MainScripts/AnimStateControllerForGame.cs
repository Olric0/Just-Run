using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece Karakterin RUN ve JUMP Animasyon State'lerinde Var. Baþka Bir Yerde Kullanýlmýyor.
/// 
/// </summary>
public class AnimStateControllerForGame : StateMachineBehaviour
{
    // Bu Metotlar, Ateþ Topunun Kullanýlabilir Bir Durumda Olup Olmadýðýný Belirliyor. Mesela
    // Bu Metotlar Sayesinde Karakter Saldýrma Animasyonuna Geçip Saldýrýrken [ canFireballBeUsed ]
    // Deðiþkeni False Deðerinde Olduðu Ýçin Oyuncu Saldýrýrken Ateþ Topu Fýrlatýp Oyunu Buga Sokamýyor.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => Character.chrctrTHIS.canFireballBeUsed = true;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => Character.chrctrTHIS.canFireballBeUsed = false;
}