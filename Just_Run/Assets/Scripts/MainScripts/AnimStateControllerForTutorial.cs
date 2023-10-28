using UnityEngine;


/// <summary>
/// 
/// Bu Script Sadece Öðretici Moddaki Karakterin RUN ve JUMP Animasyon State'lerinde Var. Baþka Bir Yerde Kullanýlmýyor.
/// 
///  >*>*> Deðiþkenlerin Açýklamalarý <*<*< \\\
///     -> [ isItSuitableForUse ]
///     Bu Deðiþken TutorialManager Scriptinin, Task4 Metodunda Tetikleniyor. Aþaðýdaki Metodlar Ateþ
///     Topunun Kontrolünü Saðladýðý Ýçin Ateþ Topunun Aktif Olmadýðý Zaman Çalýþmamasý Gerekiyor. Bu
///     Deðiþken Ýse Ateþ Topu Aktif Olunca True Oluyor Ve Metodlar Kullanýlabilir Hale Geliyor.
///     
/// </summary>
public class AnimStateControllerForTutorial : StateMachineBehaviour
{
    public static bool isItSuitableForUse;



    // Bu Metotlar, Ateþ Topunun Kullanýlabilir Bir Durumda Olup Olmadýðýný Belirliyor. Mesela
    // Bu Metotlar Sayesinde Karakter Saldýrma Animasyonuna Geçip Saldýrýrken [ canFireballBeUsed ]
    // Deðiþkeni False Deðerinde Olduðu Ýçin Oyuncu Saldýrýrken Ateþ Topu Fýrlatýp Oyunu Buga Sokamýyor.
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