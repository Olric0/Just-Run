using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 
/// Bu Sınıf Öğretici Modu Kontrol Eder. Düşmanlar Ve Karakter Dışındaki Heeeer Şey Buradan Sorumludur.
/// Yani Çok Krıtik Bir Script Ve Dolayısıyla Karmaşık. Pek Kurcalamanı Önermem. Değişkenlerin Çoğunun
/// Açıklamalarını Yazmadım Çünkü Hepsi Adı Üstünde, Anlayabilirsin.
/// 
/// 
/// >*>*> Değişkenlerin Açıklamaları <*<*< \\\
/// 
/// [Header("Basic Variables")]
///     -> [ pfBG ]
///     Parallax Arkaplan Objeleridir. Oyuncu Oyunu Oynamaya Başladığı Zaman Bunlar Aktif Olur Ki Parallax Özelliği Çalışsın.
///     
///     -> [ pfGRND ]
///     Parallax Zemin Objesidir. Oyuncu Oyunu Oynamaya Başladığı Zaman Bu Aktif Olur Ki Parallax Özelliği Çalışsın.
///     
///     -> [ currentTypeWriterCoroutine ]
///     Mevcut Diyaloğu Ekrana Yazdıran Coroutine Metodudur. Oyuncu Bir Diyaloğu Pas Geçtiğinde Mevcut Metodu Sonlandırmak İçin Kullanılır.
/// 
///     -> [ currentTaskCoroutine ]
///     Mevcut Görevin Coroutine Metodudur. Oyuncu Görevi Başarısızlık İle Sonlandırdığında Görev Metodunu Durdurmak İçin Kullanılır.
///     
///     -> [ didThePlayerMakeAMistake ]
///     Bu Değişken Oyuncu Bir Görevde Hata Yaparsa True Olur Ve Oyunun Sonunda Herhangi Bir Hata Yapıldıysa BadEnding, Hata
///     Yapılmadıysa GoodEnding Textleri Yazdırılır.
///     
///     -> [ currentText ]
///     Ekrana Yazdırılmakta Olan Diyaloğu Tutar. Ekrana Bu Değişken Yazdırılır, Oyuncu Bir Diyaloğu Pas Geçince Bu Değişken Üzerinden
///     Hangi Diyalokta Olduğumuzu Buluruz.
///     
///     -> [ failedTextCount ]
///     Bu Değişken Oyuncu Bir Görevde Hata Yaparsa Bir Artar Ve Eğer Oyuncu Bir Daha Hata Yaparsa Bu Sefer Failed Text Değil Recurrent
///     Failed Text'i Yazdırır. İki Kere Hata Yaparsa Oyuncu Aynı Görevde, Recurrent Failed2'yi Yazdırır. Her Görevde Sıfırlanır!
///     
///     -> [ isDone ]
///     Bu Değişken Ekrana Bir Text Yazdırılırken False Değer Dönrürür. Ve Yazdırma İşlemi Bittiğinde True Olur. Böylelikle Oyuncu
///     Sonraki Text'e Geçebilir. Sonraki Text'e Geçtiğinde Tekrar False Olur Ki Oyuncu Text Bitmeden Geçemesin
/// 
/// 
/// [Header("Task Variables")]
///     -> [ hasFireballTaskActiveBefore ]
///     Ateş Topunun Kontrolünü Sağlıyor. 4. Görevde Oyuncuya Ateş Topu Kullanma Özelliği Verildiği İçin Bu Değişkenle Gerekli
///     Ayarlamaları Yapıyorum. Mesela Bunun Sayesinde 2 Defa Oyuncuya Ateş Topu Özelliği Verilerek Buga Girmiyor.
///     
///     -> [ numberOfEnemiesKilled ]
///     Oyuncunun Bir Görevde Kaç Düşmanı Öldürdüğünü Belirtiyor. Eğer Gerekli Sayıya Ulaşılmışsa Görev Bitiyor.
///     
///     -> [ numberOfTask ]
///     Görev Numarası, Hangi Görevin Bitip Hangi Görevin Aktif Olduğu Gibi Ayarlamalar Yapılıyor.
///     
///     -> [ isTaskCanceled ]
///     Görevin Başarısız Olup Olmadığını Belirliyor. Eğer True Değer Döndürürse Oyuncu Başarısız Olmuş Demektir Ve Düşmanlar Silinir.
/// 
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("Basic Variables")]
    public static TutorialManager tmTHIS;
    [SerializeField] private ParallaxForBG[] pfBG = new ParallaxForBG[3];
    [SerializeField] private ParallaxForGRND pfGRND;
    private IEnumerator currentTypeWriterCoroutine;
    private IEnumerator currentTaskCoroutine;
    private bool didThePlayerMakeAMistake;
    private System.Text.StringBuilder currentText = new System.Text.StringBuilder();
    private sbyte failedTextCount;
    private bool isDone;


    [Header("UI Variables")]
    [SerializeField] private Animator[] iconAnimators = new Animator[9];
    [SerializeField] private Image[] iconImages = new Image[9];
    [SerializeField] private TMPro.TextMeshProUGUI textPanelText;
    [SerializeField] private GameObject pressAnyKey;
    [SerializeField] private GameObject textPanel;


    [Header("Characters")]
    [SerializeField] private GameObject tutorialCharacter;
    [SerializeField] private GameObject gameCharacter;


    [Header("Task Variables")]
    [SerializeField] private GameObject brokenWall;
    [SerializeField] private GameObject solidWall;
    [SerializeField] private GameObject eagle;
    private bool hasFireballTaskActiveBefore;
    internal sbyte numberOfEnemiesKilled;
    internal sbyte numberOfTask;


    [Header("Audio Variables")]
    [SerializeField] private AudioSource typeWriterSoundManager;
    [SerializeField] private AudioClip typeSound;


    // Diyaloglar.
    private string[] texts = new string[20];

    private string[] goodEndingTexts = new string[5];
    private string[] badEndingTexts = new string[5];

    private string[] failedTexts = new string[4];

    private string recurrentFailedText0;
    private string recurrentFailedText1;



    private IEnumerator Start()
    {
        // Değişkenler Set Edilir.
        tmTHIS = this;
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":
                texts[ 0] = "Merhaba, görünüşe göre\nburalara ilk defa geliyorsun.\nSana bir kaç şey göstermek\nistiyorum.";
                texts[ 1] = "Maalesef reflekslerim iyi değil,\nve senden beni yönetmeni\nisteyeceğim.";
                texts[ 2] = "İleride bir kaç tane duvar var.\nBen oraya doğru gidicem,\nsende kılıcımı kullanarak\nduvarları yok edebilirsin.";
                texts[ 3] = "Saldırmak için: ENTER\nTUŞLARINDAN BİRİSİNİ veya\nFARENİN SOL TIK TUŞUNU\nkullanabilirsin.";
                texts[ 4] = "Bakalım ne kadar iyisin?";
                texts[ 5] = "Tebrikler! Gayet iyi gidiyorsun,\nherhalde seni biraz daha\nzorlamamın bir mahsuru\nolmaz?";
                texts[ 6] = "İleride daha fazla duvar var,\nancak bazılarını kırmak için\nyeterince güçlü değilim. Yani,\nkaçmaktan başka şansımız yok.";
                texts[ 7] = "Eğer diğerlerinden farklı olarak\nsağlam bir duvar görürsen: W,\nYUKARI OK veya BOŞLUK TUŞU\nile zıplaman gerekecek.";
                texts[ 8] = "Sana inanıyorum, hadi gidelim!";
                texts[ 9] = "Bravo! Gerçekten yeteneklisin.";
                texts[10] = "Ancak, burada sadece duvarlar\nyok. Bizi çiğ çiğ yemek isteyen\nkartallarda var.";
                texts[11] = "Tabi bir dağın tepesinde, kale\nyolunda olduğumuz için burada\nyırtıcı kartalların çok fazla\nolması normal.";
                texts[12] = "Ama merak etme, kılıcımla\nonları öldürebiliriz. Sadece\nzıplamamı sağla ve onları\nhavada öldürelim.";
                texts[13] = "Hazırsan başlayalım!";
                texts[14] = "Vaay, itiraf etmeliyim ki\nsandığımdan daha iyisin.\nDur sana bir şey daha\ngöstereyim.";
                texts[15] = "Bak, bir ateş topu! Umarım\nbunu daha önce söylemediğim\niçin bana kızgın değilsindir.";
                texts[16] = "Yalnız bu kılıca benzemez,\nsadece 10 saniyede bir ateş\ntopunu kullanabilirsin.";
                texts[17] = "Oradan bakınca çok güçlü birisi\ngibi mi duruyorum? Sürekli ateş\ntopu fırlatamam. Bu yüzden\nateş topunu dikkatli kullan.";
                texts[18] = "Ateş topunu kullanmak için: F,\nSAĞ OK veya FARENİN SAĞ TIK\nTUŞUNU kullanabilirsin.";
                texts[19] = "Hadi bir egzersiz daha yapalım!";

                goodEndingTexts[0] = "Braaavo! Gerçekten takdire\nşayansın! Hiç bir hata\nyapmadan başarıyla engelleri\naştın. Artık sana tamamen\ngüvenebilirim.";
                goodEndingTexts[1] = "Yalnız, şunu unutma! Issız\nbucaksız bir yerde dağın\ntepesindeyiz ve sakın yerde\nbulduğun her şeyi içme!";
                goodEndingTexts[2] = "Burada bizi zehirlemeye çalışan\ncadılar olabilir, o yüzden her\nşeyi içme!";
                goodEndingTexts[3] = "Belki bazı cadılar\nmisafirperverdir ve bizi\nzehirlemeye çalışmaz. Ama\nyinede bu oldukça tehlikeli.";
                goodEndingTexts[4] = "Umarım uyarılarımı ciddiye\nalırsın. Bol şans, ihtiyacın\nolacak.";

                badEndingTexts[0] = "Tebrikler, hiç de fena değildin.\nBence bilmen gereken şeyleri\nartık biliyorsun.";
                badEndingTexts[1] = "Yalnız, şunu unutma! Issız\nbucaksız bir yerde dağın\ntepesindeyiz ve sakın yerde\nbulduğun her şeyi içme!";
                badEndingTexts[2] = "Burada bizi zehirlemeye çalışan\ncadılar olabilir, o yüzden her\nşeyi içme!";
                badEndingTexts[3] = "Belki bazı cadılar\nmisafirperverdir ve bizi\nzehirlemeye çalışmaz. Ama\nyinede bu oldukça tehlikeli.";
                badEndingTexts[4] = "Umarım uyarılarımı ciddiye\nalırsın. Bol şans, ihtiyacın\nolacak.";

                failedTexts[0] = "Göründüğü gibi kolay değil, di\nmi? Endişelenme bir kere daha\ndene.";
                failedTexts[1] = "Hızlı refleks istiyormuş değil\nmi? Hadi bir daha deneyelim.";
                failedTexts[2] = "Biliyorum biraz zorlayıcı. Ama\nsen bunu yapabilirsin, hadi\nhazırlan tekrar deneyelim!";
                failedTexts[3] = "Eğer ateş topunu doğru\nkullanırsan başaracağını\nbiliyorum. Hadi bir kere daha\ndeneyelim!";

                recurrentFailedText0 = "Biraz daha çabalarsan olacak.\nHazırlan ve tekrar başlayalım.";
                recurrentFailedText1 = "Bir deneme daha yapalım,\ndenemekten zarar gelmez.";
                break;

            case "Azerbaijani":
                texts[ 0] = "Salam, deyəsən bura ilk\ndəfədirsiniz. Mən sizə bir neçə\nşey göstərmək istəyirəm.";
                texts[ 1] = "Təəssüf ki, reflekslərim yaxşı\ndeyil və sizdən məni\nyönləndirmənizi xahiş edəcəm.";
                texts[ 2] = "Qarşıda bir neçə divar var. Mən\nora tərəf gedəcəm və siz mənim\nqılıncımla divarları dağıda\nbilərsiniz.";
                texts[ 3] = "Hücum etmək üçün: ENTER\nDÜŞƏRİNDƏN BİRİNDƏN vəya\nSİÇANIN SOL KLİMİNDƏN\nistifadə edə bilərsiniz.";
                texts[ 4] = "Görək nə qədər yaxşısanə?";
                texts[ 5] = "Təbrik edirik! Çox yaxşı\ngedirsən, deyəsən, səni bir az\ndaha itələsəm, etiraz\netməzsənə?";
                texts[ 6] = "Qarşıda daha çox divarlar var,\namma bəzilərini qırmağa\ngücüm çatmır. Deməli,\nqaçmaqdan başqa çarəmiz\nyoxdur.";
                texts[ 7] = "Əgər siz digərlərindən fərqli\nolaraq möhkəm divar görsəniz:\nW, YUXARİ OX vəya BOŞLUQ\nDÜYMƏSİ ilə tullanmalı\nolacaqsınız.";
                texts[ 8] = "Mən sənə inanıram, gedək!";
                texts[ 9] = "Bravo! Siz həqiqətən\nistedadlısınız.";
                texts[10] = "Ancaq burada təkcə divarlar\ndeyil. Bizi çiy çiy yemək istəyən\nqartallar da var.";
                texts[11] = "Təbii ki, dağın başında, qalaya\ngedən yolda olduğumuz üçün\nburada çoxlu yırtıcı qartalların\nolması normaldır.";
                texts[12] = "Amma narahat olma, onları\nqılıncımla öldürə bilərik. Sadəcə\nməni tullanmağa məcbur et və\ngəlin onları havada öldürək.";
                texts[13] = "Hazırsansa, başlayaq!";
                texts[14] = "Vay, etiraf etməliyəm, sən\ndüşündüyümdən daha yaxşısan.\nGəl sənə başqa bir şey\ngöstərim.";
                texts[15] = "Bax, atəş topu! Ümid edirəm ki,\nbunu daha tez demədiyim üçün\nmənə hirslənmirsən.";
                texts[16] = "Yalnız bu qılınc kimi deyil,\nyalnız hər 10 saniyədən bir atəş\ntopu istifadə edə bilərsiniz.";
                texts[17] = "Mən oradan çox güclü adam\nkimi görünürəm? Hər zaman\natəş topları ata bilmirəm. Odur\nki, atəş topundan ehtiyatla\nistifadə edin.";
                texts[18] = "Atəş topundan istifadə etmək\nüçün: F, SAĞ OX vəya SİÇANIN\nSAĞ KLİMSİNDƏN istifadə edə\nbilərsiniz.";
                texts[19] = "Gəlin başqa bir məşq edək!";

                goodEndingTexts[0] = "Bravo! Siz həqiqətən\ntəqdirəlayiqsiniz! Heç bir səhv\netmədən maneələri uğurla dəf\netdiniz. İndi sizə tam etibar edə\nbilərəm.";
                goodEndingTexts[1] = "Sadece şunu hatırla! Hiçliğin\nortasında bir dağın\ntepesindeyiz ve yerde\nbulduğunuz hiçbir şeyi\niçmiyoruz!";
                goodEndingTexts[2] = "Burada bizi zəhərləməyə\nçalışan cadugərlər ola bilər, hər\nşeyi içmə!";
                goodEndingTexts[3] = "Bəlkə bəzi cadugərlər\nqonaqpərvərdirlər və bizi\nzəhərləməyə çalışmazlar.\nAncaq bu hələ də olduqca\ntəhlükəlidir.";
                goodEndingTexts[4] = "Ümid edirəm ki,\nxəbərdarlıqlarımı ciddi qəbul\nedirsiniz. Uğurlar, sizə lazım\nolacaq.";

                badEndingTexts[0] = "Təbrik edirəm, heç də pis iş görmədin. Düşünürəm ki, indi bilməli olduğunuz şeyi bilirsiniz.";
                badEndingTexts[1] = "Sadece şunu hatırla! Hiçliğin\nortasında bir dağın\ntepesindeyiz ve yerde\nbulduğunuz hiçbir şeyi\niçmiyoruz!";
                badEndingTexts[2] = "Burada bizi zəhərləməyə\nçalışan cadugərlər ola bilər, hər\nşeyi içmə!";
                badEndingTexts[3] = "Bəlkə bəzi cadugərlər\nqonaqpərvərdirlər və bizi\nzəhərləməyə çalışmazlar.\nAncaq bu hələ də olduqca\ntəhlükəlidir.";
                badEndingTexts[4] = "Ümid edirəm ki,\nxəbərdarlıqlarımı ciddi qəbul\nedirsiniz. Uğurlar, sizə lazım\nolacaq.";

                failedTexts[0] = "Göründüyü qədər asan deyil, elə\ndeyilmiə Narahat olmayın,\nyenidən cəhd edin.";
                failedTexts[1] = "Bunun üçün sürətli reflekslər\nlazım idi, elə deyilmiə Yenidən\ncəhd edək.";
                failedTexts[2] = "Bir az çətin olduğunu bilirəm.\nAncaq bunu edə bilərsiniz, gəlin\nhazırlaşaq və yenidən cəhd\nedək!";
                failedTexts[3] = "Bilirəm ki, atəş topundan\ndüzgün istifadə etsəniz, bunu\nedə bilərsiniz. Gəlin bir daha\ncəhd edək!";

                recurrentFailedText0 = "Bir az daha cəhd etsəniz baş\nverəcək. Hazır olun və yenidən\nbaşlayaq.";
                recurrentFailedText1 = "Bir dəfə də cəhd edək, cəhd\netməyin heç bir zərəri yoxdur.";
                break;

            case "English":
                texts[ 0] = "Hi, looks like this is your first\ntime here. I want to show you a\nfew things.";
                texts[ 1] = "Unfortunately my reflexes are\nnot good, and I will ask you to\nlead me.";
                texts[ 2] = "There are several walls ahead.\nI'll go there, and you can use my\nsword to destroy the walls.";
                texts[ 3] = "You can use to attack: ONE OF\nTHE ENTER KEYS or LEFT\nCLICK OF THE MOUSE key.";
                texts[ 4] = "Let's see how good are you?";
                texts[ 5] = "Congratulations! You're doing\npretty well, I suppose you don't\nmind if I push you a little harder?";
                texts[ 6] = "There are more walls ahead,\nbut I'm not strong enough to\nbreak some of them. We have\nno choice but to flee.";
                texts[ 7] = "If you see a solid wall unlike the\nothers, you will have to jump\nwith the: W, UP ARROW or\nSPACE key.";
                texts[ 8] = "I believe you, let's go!";
                texts[ 9] = "Bravo! You are really talented.";
                texts[10] = "However, there are not only\nwalls here. There are eagles\nwho want to eat us.";
                texts[11] = "Of course, we're going to the\ncastle on top of a mountain. So,\nthere is a too much predatory\neagle.";
                texts[12] = "Don't worry, we can kill them\nwith my sword. Just make me\njump and we'll kill them in the\nair.";
                texts[13] = "If you're ready, let's get started!";
                texts[14] = "Woow, I have to admit you're\nbetter than I thought. Let me\nshow you one more thing.";
                texts[15] = "Look, it's a fireball! I hope\nyou're not mad at me for not\nsaying this before.";
                texts[16] = "But this is not like a sword, you\ncan only use a fireball every 10\nseconds.";
                texts[17] = "Do I look like a very strong\nperson from there? I can't throw\nfireballs all the time, so use the\nfireball carefully.";
                texts[18] = "To use the fireball you can use:\nF, RIGHT ARROW KEY or RIGHT\nCLICK OF THE MOUSE key.";
                texts[19] = "Let's do one more exercise!";

                goodEndingTexts[0] = "Braavo! You are truly\nadmirable! You successfully\novercome obstacles without\nmaking any mistakes. Now I\ncan totally trust you.";
                goodEndingTexts[1] = "Just don't forget this! We're on\nthe mountaintop in a deserted\nplace, and don't drink\neverything you find on the\nground!";
                goodEndingTexts[2] = "There may be witches trying to\npoison us here, so don't drink\neverything!";
                goodEndingTexts[3] = "Maybe some witches are\nhospitable and don't try to\npoison us. But it's still pretty\ndangerous.";
                goodEndingTexts[4] = "I hope you take my warnings\nseriously. Good luck, you'll\nneed it.";

                badEndingTexts[0] = "Congratulations, you weren't\nbad at all. I think you know what\nyou need to know now.";
                badEndingTexts[1] = "Just don't forget this! We're on\nthe mountaintop in a deserted\nplace, and don't drink\neverything you find on the\nground!";
                badEndingTexts[2] = "There may be witches trying to\npoison us here, so don't drink\neverything!";
                badEndingTexts[3] = "Maybe some witches are\nhospitable and don't try to\npoison us. But it's still pretty\ndangerous.";
                badEndingTexts[4] = "I hope you take my warnings\nseriously. Good luck, you'll\nneed it.";

                failedTexts[0] = "It's not as easy as it looks, is it?\nDon't worry, try again.";
                failedTexts[1] = "It's wanted quick reflexes,\nright? Let's try again.";
                failedTexts[2] = "I know it's a little challenging.\nBut you can do it, let's get ready\nlet's try again!";
                failedTexts[3] = "If you use the fireball correctly,\nI know you will succeed. Let's\ntry one more time!";

                recurrentFailedText0 = "If you try a little harder you will.\nGet ready and let's start again.";
                recurrentFailedText1 = "Let's try again, it won't hurt to\ntry.";
                break;
            
            case "Spanish":
                texts[ 0] = "Hola, parece que esta es tu\nprimera vez aquí. Quiero\nmostrarte algunas cosas.";
                texts[ 1] = "Desafortunadamente mis\nreflejos no son buenos, y voy a\npedirte que me ayudes.";
                texts[ 2] = "Hay varias paredes por delante.\nIré allí y podrás usar mi espada\npara destruir las paredes.";
                texts[ 3] = "Para atacar: UNA DE LAS\nTECLAS ENTER o CLIC\nIZQUIERDO DEL RATÓN puedes\nusar.";
                texts[ 4] = "A ver, ¿qué tan bueno eres?";
                texts[ 5] = "¡Felicidades! Lo estás haciendo\nbastante bien, ¿supongo que no\nte importa si te presiono un\npoco más?";
                texts[ 6] = "Hay más paredes adelante,\npero no soy lo suficientemente\nfuerte para romper algunas de\nellas. No tenemos más remedio\nque huir.";
                texts[ 7] = "Si ves una pared sólida\ndiferente a las demás: W,\nFLECHA ARRIBA o ESPACIO\ndeberás saltar con la tecla.";
                texts[ 8] = "Te creo. ¡Vamos!";
                texts[ 9] = "¡Bravo! Eres realmente\ntalentoso.";
                texts[10] = "Sin embargo, aquí no se trata\nsolo de paredes. Hay águilas\nque nos quieren comer.";
                texts[11] = "Por supuesto, como vamos\ncamino a un castillo en la cima\nde la montaña, hay muchas\náguilas rapaces.";
                texts[12] = "No te preocupes, podemos\nmatarlos con mi espada. Solo\nhazme saltar y los mataremos\nen el aire.";
                texts[13] = "Si estás listo, ¡comencemos!";
                texts[14] = "Woow, tengo que admitir que\neres mejor de lo que pensaba.\nDéjame mostrarte una cosa\nmás.";
                texts[15] = "¡Mira, es una bola de fuego!\nEspero que no estés enojado\nconmigo por no decir esto\nantes.";
                texts[16] = "Pero esto no es una espada,\nsolo puedes usar una bola de\nfuego cada 10 segundos.";
                texts[17] = "¿Me veo como una persona\nmuy fuerte desde allí? No\npuedo lanzar bolas de fuego\ntodo el tiempo, así que usa la\nbola de fuego con cuidado.";
                texts[18] = "Para usar la bola de fuego: F,\nFLECHA DERECHA o CLIC\nDERECHO DEL RATÓN puedes\nusar.";
                texts[19] = "¡Hagamos un ejercicio más!";

                goodEndingTexts[0] = "¡Bravo! ¡Eres verdaderamente\nadmirable! Superas con éxito\nlos obstáculos sin cometer\nerrores. Ahora puedo confiar\ntotalmente en ti.";
                goodEndingTexts[1] = "¡Solo no olvides esto! ¡Estamos\nen la cima de la montaña en un\nlugar desierto, y no bebas todo\nlo que encuentres en el suelo!";
                goodEndingTexts[2] = "Puede haber brujas tratando de\nenvenenarnos aquí, ¡así que no\nte lo bebas todo!";
                goodEndingTexts[3] = "Tal vez algunas brujas sean\nhospitalarias y no intenten\nenvenenarnos. Pero sigue\nsiendo bastante peligroso.";
                goodEndingTexts[4] = "Espero que tomes mis\nadvertencias en serio. Buena\nsuerte, la necesitarás.";

                badEndingTexts[0] = "Felicidades, no estuviste nada\nmal. Creo que sabes lo que\nnecesitas saber ahora.";
                badEndingTexts[1] = "¡Solo no olvides esto! ¡Estamos\nen la cima de la montaña en un\nlugar desierto, y no bebas todo\nlo que encuentres en el suelo!";
                badEndingTexts[2] = "Puede haber brujas tratando de\nenvenenarnos aquí, ¡así que no\nte lo bebas todo!";
                badEndingTexts[3] = "Tal vez algunas brujas sean\nhospitalarias y no intenten\nenvenenarnos. Pero sigue\nsiendo bastante peligroso.";
                badEndingTexts[4] = "Espero que tomes mis\nadvertencias en serio. Buena\nsuerte, la necesitarás.";

                failedTexts[0] = "No es tan fácil como parece,\n¿verdad? No te preocupes,\ninténtalo de nuevo.";
                failedTexts[1] = "Quería reflejos rápidos,\n¿verdad? Intentemoslo de\nnuevo.";
                failedTexts[2] = "Sé que esto es un poco\ndesafiante. Pero puedes\nhacerlo, ¡preparémonos y\nvolvamos a intentarlo!";
                failedTexts[3] = "Si usas la bola de fuego\ncorrectamente, sé que tendrás\néxito. ¡Intentémoslo una vez\nmás!";

                recurrentFailedText0 = "Si te esfuerzas un poco más lo\nlograrás. Prepárate y\nempecemos de nuevo.";
                recurrentFailedText1 = "Intentémoslo de nuevo, no hará\ndaño intentarlo.";
                break;

            case "German":
                texts[ 0] = "Hallo, es sieht so aus, als wärst\ndu das erste mal hier. Ich\nmöchte dir ein paar dinge\nzeigen.";
                texts[ 1] = "Leider sind meine reflexe nicht\ngut, und ich werde dich bitten,\nmich zu führen.";
                texts[ 2] = "Vor uns sind mehrere mauern.\nIch werde dorthin gehen, und\ndu kannst mein schwert\nbenutzen, um die mauern zu\nzerstören.";
                texts[ 3] = "Du kannst zum angreifen\nbenutzen: EINE DER EINGABE-\nTASTEN oder LINKSKLICK DER\nMUSSTASTE.";
                texts[ 4] = "Mal sehen, wie gut du bist?";
                texts[ 5] = "Herzlichen glückwunsch! Du\nmachst das ziemlich gut, ich\nnehme an, es macht dir nichts\naus, wenn ich dich noch ein\nbisschen mehr anstrenge?";
                texts[ 6] = "Es liegen noch mehr mauern\nvor uns, aber ich bin nicht stark\ngenug, um einige von ihnen zu\ndurchbrechen. Wir haben keine\nandere wahl als zu fliehen.";
                texts[ 7] = "Wenn ihr im gegensatz zu den\nanderen eine feste wand seht,\nmüsst ihr mit den tasten: W,\nPFEIL OBEN oder LEERTASTE.";
                texts[ 8] = "Ich glaube dir, los geht's!";
                texts[ 9] = "Bravo! Du bist wirklich begabt.";
                texts[10] = "Aber hier gibt es nicht nur\nmauern. Es gibt auch Adler, die\nuns fressen wollen.";
                texts[11] = "Natürlich gehen wir zur burg,\ndie auf einem berg liegt. Also\ngibt es dort zu viele raubadler.";
                texts[12] = "Keine Sorge, wir können sie mit\nmeinem schwert töten. Lass\nmich einfach springen und wir\ntöten sie in der luft.";
                texts[13] = "Wenn du bereit bist, fangen wir\nan!";
                texts[14] = "Wow, ich muss zugeben, du bist\nbesser, als ich dachte. Ich will\ndir noch etwas zeigen.";
                texts[15] = "Sieh mal, ein feuerball! Ich\nhoffe, du bist mir nicht böse,\ndass ich das nicht schon früher\ngesagt habe.";
                texts[16] = "Aber das ist nicht wie ein\nschwert, man kann einen\nfeuerball nur alle 10 sekunden\nbenutzen.";
                texts[17] = "Sehe ich von dort aus wie eine\nsehr starke person aus? Ich\nkann nicht die ganze zeit\nfeuerbälle werfen, also setze\nden feuerball vorsichtig ein.";
                texts[18] = "Um den feuerball zu benutzen,\nkannst du benutzen: F, RECHTE\nPFEILTASTE oder RECHTER\nKLICK DER MUSETASTE.";
                texts[19] = "Lass uns noch eine übung\nmachen!";

                goodEndingTexts[0] = "Braavo! Sie sind wirklich\nbewundernswert! Sie haben\nHindernisse erfolgreich\nüberwunden, ohne Fehler zu\nmachen.";
                goodEndingTexts[1] = "Aber vergiss das nicht! Wir sind\nauf dem berggipfel an einem\nverlassenen ort, und trinkt nicht\nalles, was ihr auf dem boden\nfindet!";
                goodEndingTexts[2] = "Vielleicht gibt es hier hexen, die\nuns vergiften wollen, also trinkt\nnicht alles!";
                goodEndingTexts[3] = "Vielleicht sind einige hexen\ngastfreundlich und versuchen\nnicht, uns zu vergiften. Aber es\nist trotzdem ziemlich\ngefährlich.";
                goodEndingTexts[4] = "Ich hoffe, ihr nehmt meine\nwarnungen ernst. Viel glück, du\nwirst es brauchen.";
                                    
                badEndingTexts[0] = "Glückwunsch, du warst gar\nnicht so schlecht. Ich denke, ihr\nwisst jetzt, was ihr wissen\nmüsst.";
                badEndingTexts[1] = "Vergiss nur nicht das hier! Wir\nsind auf dem Berggipfel an\neinem verlassenen Ort, und\ntrinkt nicht alles, was ihr auf\ndem Boden findet!";
                badEndingTexts[2] = "Vielleicht gibt es hier hexen, die\nuns vergiften wollen, also trinkt\nnicht alles!";
                badEndingTexts[3] = "Vielleicht sind einige hexen\ngastfreundlich und versuchen\nnicht, uns zu vergiften. Aber es\nist trotzdem ziemlich\ngefährlich.";
                badEndingTexts[4] = "Ich hoffe, ihr nehmt meine\nwarnungen ernst. Viel glück, du\nwirst es brauchen.";

                failedTexts[0] = "Es ist nicht so einfach, wie es\naussieht, nicht wahr? Keine\nsorge, versuch's noch mal.";
                failedTexts[1] = "Da sind schnelle reflexe\ngefragt, nicht wahr? Versuchen\nwir es noch einmal.";
                failedTexts[2] = "Ich weiß, es ist ein bisschen\nschwierig. Aber du schaffst es,\nmach dich bereit, wir versuchen\nes noch einmal!";
                failedTexts[3] = "Wenn du den feuerball richtig\neinsetzt, wirst du es schaffen.\nLass es uns noch einmal\nversuchen!";

                recurrentFailedText0 = "Wenn du dich ein bisschen\nmehr anstrengst, wirst du es\nschaffen. Mach dich bereit und\nlass uns noch einmal anfangen.";
                recurrentFailedText1 = "Versuchen wir es noch einmal,\nes kann nicht schaden, es zu\nversuchen.";
                break;
        }

        // Atamalar Yapıldıktan Sonra Önlem Amaçlı 0.3f Saniye Beklenir Ve Öğretici Başlar.
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(TypeWriterController0());
    }

    #region Type Writer Controllers (0-1-2-3-4)
    private IEnumerator TypeWriterController0()
    {
        PrintADialogToTheScreen(texts[0], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[1], TypeWriter(","));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[2], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        // Özel Writer İle Ekrana  [ ENTER1, ENTER2 Ve SOL TIK ]  Tuşlarının İkonları Çıkartılır.
        // Diyalog Ekrana Yazdırıldıktan Sonra İse Çıkartılmış Olan İkonlar Yok Edilir.
        PrintADialogToTheScreen(texts[3], SpecialTypeWriter(1));
        yield return StartCoroutine(WriterLoop());
        for (sbyte i = 0; i < 3; i++)
            iconAnimators[i].SetBool("CloseTheImage", true);

        PrintADialogToTheScreen(texts[4], TypeWriter(string.Empty));
        yield return StartCoroutine(WriterLoop());


        //>*>*>   1. Görevi Başlatma.   <*<*<\\\
        numberOfTask++;
        StartCoroutine(StartTask(Task1()));
    }
    internal IEnumerator TypeWriterController1()
    {
        SwitchToTutorialMode();


        PrintADialogToTheScreen(texts[5], TypeWriter("!,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[6], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        // Özel Writer İle Ekrana  [ ENTER1, ENTER2 Ve SOL TIK ]  Tuşlarının İkonları Çıkartılır.
        // Diyalog Ekrana Yazdırıldıktan Sonra İse Çıkartılmış Olan İkonlar Yok Edilir.
        PrintADialogToTheScreen(texts[7], SpecialTypeWriter(2));
        yield return StartCoroutine(WriterLoop());
        for (sbyte i = 3; i < 6; i++)
            iconAnimators[i].SetBool("CloseTheImage", true);

        PrintADialogToTheScreen(texts[8], TypeWriter(","));
        yield return StartCoroutine(WriterLoop());


        //>*>*>   2. Görevi Başlatma.   <*<*<\\\
        numberOfTask++;
        StartCoroutine(StartTask(Task2()));
    }
    internal IEnumerator TypeWriterController2()
    {
        SwitchToTutorialMode();


        PrintADialogToTheScreen(texts[9], TypeWriter("!,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[10], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[11], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[12], TypeWriter(","));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[13], TypeWriter(string.Empty));
        yield return StartCoroutine(WriterLoop());


        //>*>*>   3. Görevi Başlatma.   <*<*<\\\
        numberOfTask++;
        StartCoroutine(StartTask(Task3()));
    }
    internal IEnumerator TypeWriterController3()
    {
        SwitchToTutorialMode();


        PrintADialogToTheScreen(texts[14], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[15], TypeWriter("!,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[16], TypeWriter(".,"));
        yield return StartCoroutine(WriterLoop());

        PrintADialogToTheScreen(texts[17], TypeWriter("?,"));
        yield return StartCoroutine(WriterLoop());

        // Özel Writer İle Ekrana  [ ENTER1, ENTER2 Ve SOL TIK ]  Tuşlarının İkonları Çıkartılır.
        // Diyalog Ekrana Yazdırıldıktan Sonra İse Çıkartılmış Olan İkonlar Yok Edilir.
        PrintADialogToTheScreen(texts[18], SpecialTypeWriter(3));
        yield return StartCoroutine(WriterLoop());
        for (sbyte i = 6; i < 9; i++)
            iconAnimators[i].SetBool("CloseTheImage", true);

        PrintADialogToTheScreen(texts[19], TypeWriter(string.Empty));
        yield return StartCoroutine(WriterLoop());


        //>*>*>   4. Görevi Başlatma.   <*<*<\\\
        numberOfTask++;
        StartCoroutine(StartTask(Task4()));
    }
    internal IEnumerator TypeWriterController4()
    {
        SwitchToTutorialMode();

        // Artık Ateş Topu Aktif Ve Öğreticiye Tekrar Geçildiğinde İkonun Dolu Gözükmesi İçin Bu Kod Var.
        GameObject.Find("Canvas/FireballIcon/Icon").GetComponent<Image>().fillAmount = 1.0f;

        // Bu If Şu İşe Yarar: Eğer Karakter Öğreticinin Herhangi Bir Yerinde Hata Yaparsa Oyun Sonunda Ona Farklı Bir Metin Çıkarır.
        // Metinden Sonra İse Oyuncuyu Normal Oyuna Geri Gönderir.
        if (didThePlayerMakeAMistake == true)
        {
            PrintADialogToTheScreen(badEndingTexts[0], TypeWriter(","));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(badEndingTexts[1], TypeWriter(string.Empty));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(badEndingTexts[2], TypeWriter(string.Empty));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(badEndingTexts[3], TypeWriter(string.Empty));
            yield return StartCoroutine(WriterLoop());

            // Text 5 Kısmı. (Son Writer Tetiklenir Ve Öğretici Modu Sona Erer)
            PrintADialogToTheScreen(badEndingTexts[4], LastTypeWriter());
            yield return StartCoroutine(WriterLoop());
        }
        else
        {
            PrintADialogToTheScreen(goodEndingTexts[0], TypeWriter("!"));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(goodEndingTexts[1], TypeWriter("."));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(goodEndingTexts[2], TypeWriter("."));
            yield return StartCoroutine(WriterLoop());

            PrintADialogToTheScreen(goodEndingTexts[3], TypeWriter("."));
            yield return StartCoroutine(WriterLoop());
            // Text 5 Kısmı. (Son Writer Tetiklenir Ve Öğretici Modu Sona Erer)
            PrintADialogToTheScreen(goodEndingTexts[4], LastTypeWriter());
            yield return StartCoroutine(WriterLoop());
        }

        // Öğretici Modu Bitti Ve Oyuncu Ana Menüye Gitti.
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region Tasks Methods
    private IEnumerator Task1()
    {
        // Düşmanları Sırasıyla Çağırma.
        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_0";
        yield return new WaitForSeconds(2.0f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(9.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_2";
        Instantiate(brokenWall, new Vector2(12.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_3";
    }
    private IEnumerator Task2()
    {
        // Düşmanları Sırasıyla Çağırma.
        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_0";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_0";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(9.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_1";
        Instantiate(solidWall, new Vector2(16.5f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_2";
        yield return new WaitForSeconds(2.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_2";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_3";
    }
    private IEnumerator Task3()
    {
        // Düşmanları Sırasıyla Çağırma.
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_0";
        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_0";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_0";
        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_2";
        yield return new WaitForSeconds(1.0f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_2";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_3";
        yield return new WaitForSeconds(1.0f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_4";
        yield return new WaitForSeconds(2.0f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_5";
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_3";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_6";
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_4";
    }
    private IEnumerator Task4()
    {
        // Düşmanları Sırasıyla Çağırma.
        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_0";
        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_0";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_1";
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_0";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_2";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_1";
        yield return new WaitForSeconds(1.0f);

        Instantiate(solidWall, new Vector2(9.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_2";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_1";
        yield return new WaitForSeconds(1.5f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_3";
        Instantiate(solidWall, new Vector2(12.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_4";
        yield return new WaitForSeconds(2.0f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_3";
        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_2";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_3";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_4";
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_5";
        yield return new WaitForSeconds(1.5f);

        Instantiate(brokenWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_4";
        Instantiate(brokenWall, new Vector2(12.0f, -1.18f), Quaternion.identity).name = "BrokenWallTTRL_5";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_5";
        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_6";
        yield return new WaitForSeconds(2.0f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_7";
        yield return new WaitForSeconds(1.5f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_6";
        Instantiate(eagle, new Vector2(14.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_7";
        yield return new WaitForSeconds(1.0f);

        Instantiate(solidWall, new Vector2(8.0f, -1.18f), Quaternion.identity).name = "SolidWallTTRL_8";
        yield return new WaitForSeconds(1.0f);

        Instantiate(eagle, new Vector2(10.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_8";
        Instantiate(eagle, new Vector2(14.0f, 0.5f), Quaternion.identity).name = "EagleTTRL_9";
    }
    #endregion

    #region Task Controller Methods
    internal IEnumerator StopTask()
    {
        // Düşman Objenin Script'inden Verilen Parametrede Ki Görevi Sonlandırma.
        StopCoroutine(currentTaskCoroutine);

        // Parallax Mekaniğini Kapatma.
        foreach (ParallaxForBG item in pfBG)
            item.enabled = false;
        pfGRND.enabled = false;

        // Tekrardan Öğreticiye Geçme.
        textPanel.SetActive(true);
        pressAnyKey.SetActive(true);
        tutorialCharacter.SetActive(true);
        gameCharacter.SetActive(false);

        // Sahnede Kalmış Düşman Varsa Yok Etme. Ayrıca Oyuncu 4. Görevdeyse Ateş Topunu Sıfırlar.
        switch (numberOfTask)
        {
            case 1:
                for (sbyte i = 1; i <= 3; i++)
                    if (GameObject.Find("BrokenWallTTRL_" + i) == true)
                        Destroy(GameObject.Find("BrokenWallTTRL_" + i));
                break;

            case 2:
                if (GameObject.Find("BrokenWallTTRL_1") == true)
                {
                    Destroy(GameObject.Find("BrokenWallTTRL_1"));
                }
                else if (GameObject.Find("BrokenWallTTRL_2") == true)
                {
                    Destroy(GameObject.Find("BrokenWallTTRL_2"));
                }
                for (sbyte i = 0; i <= 3; i++)
                    if (GameObject.Find("SolidWallTTRL_" + i) == true)
                        Destroy(GameObject.Find("SolidWallTTRL_" + i));
                break;

            case 3:
                if (GameObject.Find("BrokenWallTTRL_0") == true)
                {
                    Destroy(GameObject.Find("BrokenWallTTRL_0"));
                }
                else if (GameObject.Find("BrokenWallTTRL_1") == true)
                {
                    Destroy(GameObject.Find("BrokenWallTTRL_1"));
                }
                for (sbyte i = 0; i <= 4; i++)
                    if (GameObject.Find("SolidWallTTRL_" + i) == true)
                        Destroy(GameObject.Find("SolidWallTTRL_" + i));
                for (sbyte i = 0; i <= 6; i++)
                    if (GameObject.Find("EagleTTRL_" + i) == true)
                        Destroy(GameObject.Find("EagleTTRL_" + i));
                break;

            case 4:
                // Ateş Topunu Sıfırlama.
                AnimStateControllerForTutorial.isItSuitableForUse = false;
                GameObject.Find("Canvas").transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().fillAmount = 0;

                for (sbyte i = 0; i <= 5; i++)
                    if (GameObject.Find("BrokenWallTTRL_" + i) == true)
                        Destroy(GameObject.Find("BrokenWallTTRL_" + i));
                for (sbyte i = 0; i <= 8; i++)
                    if (GameObject.Find("SolidWallTTRL_" + i) == true)
                        Destroy(GameObject.Find("SolidWallTTRL_" + i));
                for (sbyte i = 1; i <= 9; i++)
                    if (GameObject.Find("EagleTTRL_" + i) == true)
                        Destroy(GameObject.Find("EagleTTRL_" + i));
                break;
        }

        // Görev Sonlandırıldı Ve Öğreticiye Geri Geçildi,  [ failedTextCount ]  İle Oyuncu
        // Kaç Kez Aynı Bölümde Hata Yaptıysa; Ona Göre Ekrana Bir Diyalog Yazdırılır.
        if (failedTextCount == 0)
        {
            currentText.Clear();
            currentText.Append(failedTexts[numberOfTask - 1]);
            currentTypeWriterCoroutine = TypeWriter("?");
            StartCoroutine(currentTypeWriterCoroutine);
        }
        else if (failedTextCount == 1)
        {
            currentText.Clear();
            currentText.Append(recurrentFailedText0);
            currentTypeWriterCoroutine = TypeWriter(".");
            StartCoroutine(currentTypeWriterCoroutine);
        }
        else
        {
            currentText.Clear();
            currentText.Append(recurrentFailedText1);
            currentTypeWriterCoroutine = TypeWriter(",");
            StartCoroutine(currentTypeWriterCoroutine);
        }
        yield return StartCoroutine(WriterLoop());

        // Bir Sonraki Sefere, Başarısızlık Mesajını Değiştirmek İçin Değişken Kontrolü Ve Görevi Yeniden Başlatma.
        didThePlayerMakeAMistake = true;
        failedTextCount++;

        // Burada currentTaskCoroutine'a Baştan Bir Atama Yapıyorum. Görünce Bu Ne Lan Diyebilirsin Ama Coroutine'lar
        // Üzerinde Tecrübeliysen Anlayabilirsinki; Eğer Bu If'ler İle Atama Yapmazsam StartTask'da Task Metodunu Çalıştırırken
        // Metodun Ortasından Başlayacaktır. Çünkü Görevin X Noktasında Oyuncu Başarısız Oldu Ve Start Verirken Metot Başlangıçtan
        // Değil, X'den Başlayacaktır. Bu Sebeple Tekrar Atama Yaparsam, Oyuncu Mevcut Görevde BAŞTAN Başlayabilir.
        if (currentTaskCoroutine.ToString().Contains("Task4"))
            currentTaskCoroutine = Task4();
        else if (currentTaskCoroutine.ToString().Contains("Task3"))
            currentTaskCoroutine = Task3();
        else if (currentTaskCoroutine.ToString().Contains("Task2"))
            currentTaskCoroutine = Task2();
        else
            currentTaskCoroutine = Task1();
        StartCoroutine(StartTask(currentTaskCoroutine));
    }
    private IEnumerator StartTask(IEnumerator taskCoroutine)
    {
        // Öğreticiye Özel İşlevleri Kapatma.
        textPanel.SetActive(false);
        pressAnyKey.SetActive(false);
        tutorialCharacter.SetActive(false);
        gameCharacter.SetActive(true);

        // Parallax Mekaniğini Aktif Etme.
        foreach (ParallaxForBG item in pfBG)
            item.enabled = true;
        pfGRND.enabled = true;

        // Eğer Oyuncu Bir Önceki Görevi Havadayken Bitirirse, Tekrar Göreve Başlarken Havadan Başlamaması İçin Yazılmış Bir Kod.
        gameCharacter.transform.position = new Vector2(-1.0f, -0.73f);

        // Type Writer'dan Kalma Olan Pitch Değerini Düzeltme.
        typeWriterSoundManager.pitch = 1.0f;

        // Burayı Silme Oyun Buga Girer. Görevin Bitip Bitmediğini Ayarlayan Değişken Bu. Tekrardan Default Değerlerine Çeviriyorum.
        numberOfEnemiesKilled = 0;

        // 4. Taskda Mıyız Diye Kontrol Etme.
        if (numberOfTask == 4 && hasFireballTaskActiveBefore == false)
        {
            // 4. Görevde Ateş Topu Özelliği Eklendiği İçin Bu Kodlar Onları Set Ediyor. "Bunların Yeri Burası Mı?"
            // Diyebilirsin, Deme Çünkü Yeri Burası. Eğer Writer'da Yapsam Karakter Aktif Olmadığı İçin Error Verir.
            // 0.2f Saniye Bekleme Ne İş Diyecek Olursan Olası Bugları Önlemek İçin, Bekleniyor Ki Sistem Kendine Gelsin.
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(true);
            Destroy(tutorialCharacter.transform.GetChild(0).gameObject);
            yield return new WaitForSeconds(0.2f);
            AnimStateControllerForTutorial.isItSuitableForUse = true;
            CharacterForTutorial.chrctrTHIS.isPlayerCanUseFireball = true;
            CharacterForTutorial.chrctrTHIS.StartCoroutine(CharacterForTutorial.chrctrTHIS.WaitToUsingFireballForFirstTime());

            hasFireballTaskActiveBefore = true;
        }
        else if (numberOfTask == 4)
        {
            AnimStateControllerForTutorial.isItSuitableForUse = true;
            CharacterForTutorial.chrctrTHIS.StartCoroutine(CharacterForTutorial.chrctrTHIS.WaitToUsingFireballForFirstTime());
        }

        // Görevi Başlatma.
        currentTaskCoroutine = taskCoroutine;
        StartCoroutine(currentTaskCoroutine);
    }
    internal void IsTheTaskOver()
    {
        numberOfEnemiesKilled++;
        if (numberOfEnemiesKilled == 4 && numberOfTask == 1)
            StartCoroutine(TypeWriterController1());
        else if (numberOfEnemiesKilled == 7 && numberOfTask == 2)
            StartCoroutine(TypeWriterController2());
        else if (numberOfEnemiesKilled == 14 && numberOfTask == 3)
            StartCoroutine(TypeWriterController3());
        else if (numberOfEnemiesKilled == 25 && numberOfTask == 4)
            StartCoroutine(TypeWriterController4());
    }
    #endregion

    #region Type Writer Methots
    /// <summary>
    /// 
    /// >*>*> Metot Açıklamaları <*<*< \\\
    /// 
    /// > [ PrintADialogueToTheScreen() ]
    ///     Diyalog Ve Metot Atamalarını Yapıp TypeWriter Metodunu Çalıştırır. Bu Atamalar Olmaz İse Oyuncu Diyaloğu Pas Geçemez.
    /// 
    /// > [ TypeWriter() ]
    ///     Amacı Mevcut Diyaloğu Ekrana Yazdırmaktır. waitWhenYouSeeThese Parametresi İle Metoda Hangi Noktalama İşaretlerini
    ///     Gördüğünde Beklemesi Gerektiğini Söyleyebilirsin. Bu Şu İşe Yarar: Mesela İçinde "?" Olmayan Bir Diyalokta Sistem
    ///     Defalarca Kez "?" Aramaz. Bu Büyük Bir Kazanç Sağlıyor. Detaylar Metodun İçinde Var.
    /// 
    /// > [ SpecialTypeWriter() ]
    ///     "W, YUKARI OK veya BOŞLUK TUŞU ile zıplaman gerekecek."  Metininde Olduğu Gibi Oyuncuya Tuşların İşlevi Açıklanırken
    ///     Ekrana Bu Tuşların İkonları Çıkartılır. Bun İkonların Sırası İle Ne Zaman Çıkacağını Ayarlamak İçin Bu Metodu Kullanıyoruz.
    /// 
    /// > [ LastTypeWriter() ]
    ///     Bu Metodun Normal Writer Metodlarından Bir Farkı Yoktur. Sadece PressAnyKey Objesini Yok Edip, PressAnyKeyToLeave
    ///     Objesini Aktif Eder. Bu Metot Sadece Öğreticinin Son Metnini Çalıştırırken Yani Öğretici Biterken Kullanılır.
    /// 
    /// > [ StartKeepWaitingAnimationForInputIcons() ]
    ///     Bu Metot Sadece Özel Writer'larda Tetiklenir. Amacı, Aktifleştirilmiş Input Görsellerini Ekranda Tutmaktır.
    ///     
    /// > [ WriterLoop() ]
    ///     Bu Metot Sadece WriterControlers Metotlarında Tetiklenir. Amacı Ekrana Bir Metin Yazdırılırken, Kullanıcının
    ///     Öbür Metine Geçebilmesidir. Ayrıca Bu Metot Sayesinde Oyuncu Henüz Ekrana Yazdırılmamış Olan Bir Diyaloğu
    ///     Pas Geçebilir. Eğer Pas Geçilen Diyalog Ekrana Tuş İkonlarını Çıkartıyorsa Yine Gerekli Özel İşlemler
    ///     Yapılır, Ardından CloseTheImagesWhenThePlayerClicked Metodu İle Etkileşime Girilir.
    /// 
    /// > [ CloseTheImagesWhenThePlayerClicked() ]
    ///     Eğer Oyuncu Ekrana Tuş İkonlarının Çıkartılacağı Bir Diyaloğu Pas Geçerse, İkonlar Ekrana Çıkartılır Ardından
    ///     Bu Metot Tetiklenerek Oyuncu Tekrar Bir Tuşa Bastığında Ekrana Çıkartılmış Olan İkonlar Kapatılır.
    /// 
    /// </summary>
    private void PrintADialogToTheScreen(string text, IEnumerator typeWriterCoroutine)
    {
        currentText.Clear();
        currentText.Append(text);
        currentTypeWriterCoroutine = typeWriterCoroutine;
        StartCoroutine(currentTypeWriterCoroutine);
    }
    private IEnumerator TypeWriter(string waitWhenYouSeeThese)
    {
        // Önceki Text'den Kalan Press Any Key Textini Kapatma Ve Text Metnini Sıfırlama.
        pressAnyKey.SetActive(false);
        textPanelText.text = null;

        // Parametre Olarak Alınan (typeWriterText) Text Ekrana Yazdırılmaya Başlar.
        switch (waitWhenYouSeeThese)
        {
            /// <summary>
            /// 
            ///   1 case "?"   -> [ ? ]            Çıkarsa BEKLER
            ///   2 case "?,"  -> [ ? ] veya [ , ] Çıkarsa BEKLER
            ///   3 case "!"   -> [ ! ]            Çıkarsa BEKLER
            ///   4 case "!,"  -> [ ! ] veya [ , ] Çıkarsa BEKLER
            ///   5 case "."   -> [ . ]            Çıkarsa BEKLER
            ///   6 case ".,"  -> [ . ] veya [ , ] Çıkarsa BEKLER
            ///   7 case ","   -> [ , ]            Çıkarsa BEKLER
            ///   8 default    -> [   ]                    BEKLEMEZ
            /// 
            /// </summary>

            case "?":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Cümledeki Noktalama İşaretlerine Göre Bekleme.
                    if (i == '?')
                        yield return new WaitForSeconds(0.3f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case "?,":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Cümledeki Noktalama İşaretlerine Göre Bekleme.
                    if (i == '?')
                        yield return new WaitForSeconds(0.3f);
                    else if (i == ',')
                        yield return new WaitForSeconds(0.2f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case "!":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Cümledeki Noktalama İşaretlerine Göre Bekleme.
                    if (i == '!')
                        yield return new WaitForSeconds(0.3f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case "!,":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Cümledeki Noktalama İşaretlerine Göre Bekleme.
                    if (i == '!')
                        yield return new WaitForSeconds(0.3f);
                    else if (i == ',')
                        yield return new WaitForSeconds(0.2f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case ".":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Bekleme.
                    if (i == '.')
                        yield return new WaitForSeconds(0.3f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case ".,":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Bekleme.
                    if (i == '.')
                        yield return new WaitForSeconds(0.3f);
                    else if (i == ',')
                        yield return new WaitForSeconds(0.2f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            case ",":
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Bekleme.
                    if (i == ',')
                        yield return new WaitForSeconds(0.2f);
                    else
                        yield return new WaitForSeconds(0.075f);
                }
                break;

            default:
                foreach (char i in currentText.ToString())
                {
                    // Text'e Yavaş Yavaş Yazı Yazdırma.
                    textPanelText.text += i;

                    // Type Writer Sesini Ayarlama.
                    typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
                    typeWriterSoundManager.PlayOneShot(typeSound);

                    // Bekleme.
                    yield return new WaitForSeconds(0.075f);
                }
                break;
        }

        // Sisteme Metinin Bittiği Bildirilir Ve Press Any Key Yazısı Ortaya Çıkar.
        isDone = true;
        pressAnyKey.SetActive(true);
    }
    private IEnumerator SpecialTypeWriter(sbyte stageNo)
    {
        ///  <summary>
        /// 
        /// >*>*> Değişkenlerin Açıklamaları <*<*< \\\
        /// 
        /// > [ stageNo ]
        ///     Bu Değişken Sayesinde Hangi İkonların Ekrana Çıkacağı Belli Olur. Eğer Stage 1'deyse
        ///     Sadece  [ ENTER 1, ENTER 2 VE SOL FARE TUŞU ]  İkonları Ekrana Çıkar. Bunu Nerden Mi Anlıyoruz
        ///     Diyecek Olursan,  [ Canvas>InputIcons ]  Objesinde Hangi Stage'de Hangi İkonlar Var Görebilirsin.
        ///     Ayrıca Bu Değişken Sayesinde Özel Karakterin (specialCharacter) Değeri Belli Oluyor. Mesela
        ///     İkinci Stage'de, İlk Part İkonu Yani Çıkması Gereken İlk İkon:  [ W ]  İkonudur. Bu Sebeple Eğer
        ///     Sistem 2. Stage'de İse Her Halükarda specialCharacter W Olmalıdır. Çıkması Gereken İkonları
        ///     Sırası  [ Canvas>InputIcons ]  Kısmında Vardır. Sırayla Ekrana Çıkartılır.
        ///     
        /// > [ partNo ]
        ///     Bu Değişken Sayesinde Bir Stage'de Hangi İkonların Çıktığı Belli Olmuş Olur Ve Düzen Sağlanır.
        ///     Mesela, Bunun Sayesinde Ekrana 2. Olarak Çıkması Gereken İkon 2. Olarak Çıkar 1. Olarak Çıkmaz.
        ///     Bir Sırayla Gidilir, 1. İkon Çıkmadan Öbürleri Ne Olursa Olsun Çıkamaz.
        ///     
        /// > [ areTheIconsEligibleToBeActive ]
        ///     Bu Değişken Sayesinde ":" Harfi Ekrana Yazdırılmadan Hiç Bir İkon Ekrana Çıkamaz Ve Aktif Olamaz.
        /// 
        /// </summary>


        // Önceki Text'den Kalan Press Any Key Textini Kapatma Ve Text Metnini Sıfırlama.
        pressAnyKey.SetActive(false);
        textPanelText.text = null;

        // Değişken Atamaları.
        sbyte partNo = 1;
        char specialCharacter = ' ';
        bool areTheIconsEligibleToBeActive = false;
        switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
        {
            case "Turkish":
                switch (stageNo)
                {
                    case 1: specialCharacter = 'L'; break;
                    case 2: specialCharacter = 'W'; break;
                    case 3: specialCharacter = 'F'; break;
                }
                break;
            case "Azerbaijani":
                switch (stageNo)
                {
                    case 1: specialCharacter = 'R'; break;
                    case 2: specialCharacter = 'W'; break;
                    case 3: specialCharacter = 'F'; break;
                }
                break;
            case "English":
                switch (stageNo)
                {
                    case 1: specialCharacter = 'H'; break;
                    case 2: specialCharacter = 'W'; break;
                    case 3: specialCharacter = 'F'; break;
                }
                break;
            case "Spanish":
                switch (stageNo)
                {
                    case 1: specialCharacter = 'C'; break;
                    case 2: specialCharacter = 'W'; break;
                    case 3: specialCharacter = 'F'; break;
                }
                break;
            case "German":
                switch (stageNo)
                {
                    case 1: specialCharacter = '-'; break;
                    case 2: specialCharacter = 'W'; break;
                    case 3: specialCharacter = 'F'; break;
                }
                break;
        }

        // İkonları Kontrollü Bir Şekilde Çıkarma.
        foreach (char i in currentText.ToString())
        {
            // Text'e Yavaş Yavaş Yazı Yazdırma.
            textPanelText.text += i;

            // Type Writer Sesini Ayarlama.
            typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
            typeWriterSoundManager.PlayOneShot(typeSound);

            // Cümledeki Noktalama İşaretlerine Göre Bekleme.
            if (areTheIconsEligibleToBeActive == true && i == specialCharacter)
            {
                switch (PlayerPrefs.GetString("LeanLocalization.CurrentLanguage"))
                {
                    case "Turkish":
                        switch (stageNo)
                        {
                            case 1:
                                // 1. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'L':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'I';
                                            partNo++;

                                            iconAnimators[0].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[0].enabled = true;
                                        }
                                        break;
                                    case 'I':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'U';
                                            partNo++;

                                            iconAnimators[1].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[1].enabled = true;
                                        }
                                        break;
                                    case 'U':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[2].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[2].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(0, 2));
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                // 2. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'W':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'K';
                                            partNo++;

                                            iconAnimators[3].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[3].enabled = true;
                                        }
                                        break;
                                    case 'K':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'U';
                                            partNo++;

                                            iconAnimators[4].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[4].enabled = true;
                                        }
                                        break;
                                    case 'U':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[5].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[5].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(3, 5));
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                // 3. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'F':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'K';
                                            partNo++;

                                            iconAnimators[6].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[6].enabled = true;
                                        }
                                        break;
                                    case 'K':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'I';
                                            partNo++;

                                            iconAnimators[7].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[7].enabled = true;
                                        }
                                        break;
                                    case 'I':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[8].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[8].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(6, 8));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case "Azerbaijani":
                        switch (stageNo)
                        {
                            case 1:
                                // 1. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'R':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'D';
                                            partNo++;

                                            iconAnimators[0].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[0].enabled = true;
                                        }
                                        break;
                                    case 'D':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'L';
                                            partNo++;

                                            iconAnimators[1].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[1].enabled = true;
                                        }
                                        break;
                                    case 'L':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[2].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[2].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(0, 2));
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                // 2. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'W':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'X';
                                            partNo++;

                                            iconAnimators[3].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[3].enabled = true;
                                        }
                                        break;
                                    case 'X':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'U';
                                            partNo++;

                                            iconAnimators[4].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[4].enabled = true;
                                        }
                                        break;
                                    case 'U':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[5].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[5].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(3, 5));
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                // 3. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'F':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'X';
                                            partNo++;

                                            iconAnimators[6].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[6].enabled = true;
                                        }
                                        break;
                                    case 'X':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'M';
                                            partNo++;

                                            iconAnimators[7].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[7].enabled = true;
                                        }
                                        break;
                                    case 'M':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[8].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[8].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(6, 8));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case "English":
                        switch (stageNo)
                        {
                            case 1:
                                // 1. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'H':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'N';
                                            partNo++;

                                            iconAnimators[0].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[0].enabled = true;
                                        }
                                        break;
                                    case 'N':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'I';
                                            partNo++;

                                            iconAnimators[1].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[1].enabled = true;
                                        }
                                        break;
                                    case 'I':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[2].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[2].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(0, 2));
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                // 2. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'W':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'O';
                                            partNo++;

                                            iconAnimators[3].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[3].enabled = true;
                                        }
                                        break;
                                    case 'O':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'E';
                                            partNo++;

                                            iconAnimators[4].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[4].enabled = true;
                                        }
                                        break;
                                    case 'E':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[5].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[5].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(3, 5));
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                // 3. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'F':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'Y';
                                            partNo++;

                                            iconAnimators[6].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[6].enabled = true;
                                        }
                                        break;
                                    case 'Y':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'S';
                                            partNo++;

                                            iconAnimators[7].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[7].enabled = true;
                                        }
                                        break;
                                    case 'S':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[8].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[8].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(6, 8));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case "Spanish":
                        switch (stageNo)
                        {
                            case 1:
                                // 1. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'C':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'E';
                                            partNo++;

                                            iconAnimators[0].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[0].enabled = true;
                                        }
                                        break;
                                    case 'E':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'Ó';
                                            partNo++;

                                            iconAnimators[1].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[1].enabled = true;
                                        }
                                        break;
                                    case 'Ó':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[2].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[2].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(0, 2));
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                // 2. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'W':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'B';
                                            partNo++;

                                            iconAnimators[3].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[3].enabled = true;
                                        }
                                        break;
                                    case 'B':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'O';
                                            partNo++;

                                            iconAnimators[4].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[4].enabled = true;
                                        }
                                        break;
                                    case 'O':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[5].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[5].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(3, 5));
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                // 3. Stage'in İkonlarının Çıkması
                                switch (specialCharacter)
                                {
                                    case 'F':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'R';
                                            partNo++;

                                            iconAnimators[6].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[6].enabled = true;
                                        }
                                        break;
                                    case 'R':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'Ó';
                                            partNo++;

                                            iconAnimators[7].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[7].enabled = true;
                                        }
                                        break;
                                    case 'S':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[8].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[8].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(6, 8));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case "German":
                        switch (stageNo)
                        {
                            case 1:
                                // 1. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case '-':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'E';
                                            partNo++;

                                            iconAnimators[0].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[0].enabled = true;
                                        }
                                        break;
                                    case 'E':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'A';
                                            partNo++;

                                            iconAnimators[1].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[1].enabled = true;
                                        }
                                        break;
                                    case 'A':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[2].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[2].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(0, 2));
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                // 2. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'W':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'O';
                                            partNo++;

                                            iconAnimators[3].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[3].enabled = true;
                                        }
                                        break;
                                    case 'O':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'S';
                                            partNo++;

                                            iconAnimators[4].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[4].enabled = true;
                                        }
                                        break;
                                    case 'S':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[5].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[5].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(3, 5));
                                        }
                                        break;
                                }
                                break;
                            case 3:
                                // 3. Stage'in İkonlarının Çıkması.
                                switch (specialCharacter)
                                {
                                    case 'F':
                                        if (partNo == 1)
                                        {
                                            specialCharacter = 'S';
                                            partNo++;

                                            iconAnimators[6].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[6].enabled = true;
                                        }
                                        break;
                                    case 'S':
                                        if (partNo == 2)
                                        {
                                            specialCharacter = 'A';
                                            partNo++;

                                            iconAnimators[7].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[7].enabled = true;
                                        }
                                        break;
                                    case 'A':
                                        if (partNo == 3)
                                        {
                                            iconAnimators[8].enabled = true;
                                            yield return new WaitForSeconds(0.05f);
                                            iconImages[8].enabled = true;

                                            StartCoroutine(StartKeepWaitingAnimationForInputIcons(6, 8));
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            else if (i == ':')
            {
                areTheIconsEligibleToBeActive = true;
                yield return new WaitForSeconds(0.3f);
            }
            else if (i == ',')
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.075f);
        }

        // Sisteme Metinin Bittiği Bildirilir Ve Press Any Key Yazısı Ortaya Çıkar
        isDone = true;
        pressAnyKey.SetActive(true);
    }
    private IEnumerator LastTypeWriter()
    {
        // Önceki Text'den Kalan Press Any Key Textini Kapatma Ve Text Metnini Sıfırlama.
        pressAnyKey.SetActive(false);
        textPanelText.text = null;

        // Parametre Olarak Alınan (typeWriterText) Text Ekrana Yazdırılmaya Başlar.
        foreach (char i in currentText.ToString())
        {
            // Text'e Yavaş Yavaş Yazı Yazdırma.
            textPanelText.text += i;

            // Type Writer Sesini Ayarlama.
            typeWriterSoundManager.pitch = Random.Range(0.8f, 1.2f);
            typeWriterSoundManager.PlayOneShot(typeSound);

            // Bekleme.
            if (i == '.')
                yield return new WaitForSeconds(0.3f);
            else if (i == ',')
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.075f);
        }
        
        // Bitiş, Konuşma Animasyonu Biter Ve Press Any Key To Leave Yazısı Ortaya Çıkar.
        Destroy(pressAnyKey);
        yield return new WaitForSeconds(0.03f);
        GameObject.Find("Canvas").transform.GetChild(4).gameObject.SetActive(true);
        isDone = true;
    }
    public IEnumerator StartKeepWaitingAnimationForInputIcons(sbyte minimumNumberOfIcons, sbyte maximumNumberOfIcons)
    {
        yield return new WaitForSeconds(0.8f);
        for (sbyte i = minimumNumberOfIcons; i <= maximumNumberOfIcons; i++)
            iconAnimators[i].SetBool("KeepWaiting", true);
    }
    private IEnumerator WriterLoop()
    {
        yield return new WaitForSeconds(0.01f);

        while (true)
        {
            if (Input.anyKeyDown == true && Input.GetKey(KeyCode.Escape) == false && GameObject.Find("Canvas/QuitPanel") == false)
            {
                if (isDone == false)
                {
                    // Ekrana Yazdırılmakta Olan Metin Pas Geçilerek Tamamlanır.
                    textPanelText.text = currentText.ToString();
                    StopCoroutine(currentTypeWriterCoroutine);
                    if (currentTypeWriterCoroutine.ToString().Contains("SpecialTypeWriter"))
                    {
                        // Bu If Koşulunun İçindeki Kodlar Sayesinde, Eğer Oyuncu Ekrana Tuş İkonlarının
                        // Çıkartılacağı Bir Diyalog Esnasında O Diyaloğu Geçerse. Henüz Ekrana
                        // Çıkartılmakta Olan Veya Çıkartılmamış Olan İkonlar Ekrana Çıkartılır.
                        for (sbyte i = 0; i < 3; i++)
                        {
                            if (GameObject.Find("Canvas/InputIcons").transform.GetChild(i).name == "Stage" + (i + 1))
                            {
                                switch (i)
                                {
                                    case 0:
                                        GameObject.Find("Canvas/InputIcons/Stage1").name = "Stage1_Done";
                                        iconAnimators[0].enabled = true;
                                        iconAnimators[1].enabled = true;
                                        iconAnimators[2].enabled = true;
                                        for (sbyte j = 0; j < 3; j++)
                                        {
                                            iconAnimators[j].SetBool("KeepWaiting", true);
                                        }
                                        yield return new WaitForSeconds(0.05f);
                                        iconImages[0].enabled = true;
                                        iconImages[1].enabled = true;
                                        iconImages[2].enabled = true;

                                        StartCoroutine(CloseTheImagesWhenThePlayerClicked(0, 3));
                                        break;
                                    case 1:
                                        GameObject.Find("Canvas/InputIcons/Stage2").name = "Stage2_Done";

                                        iconAnimators[3].enabled = true;
                                        iconAnimators[4].enabled = true;
                                        iconAnimators[5].enabled = true;
                                        for (sbyte j = 3; j < 6; j++)
                                        {
                                            iconAnimators[j].SetBool("KeepWaiting", true);
                                        }
                                        yield return new WaitForSeconds(0.05f);
                                        iconImages[3].enabled = true;
                                        iconImages[4].enabled = true;
                                        iconImages[5].enabled = true;

                                        StartCoroutine(CloseTheImagesWhenThePlayerClicked(3, 6));
                                        break;
                                    case 2:
                                        GameObject.Find("Canvas/InputIcons/Stage3").name = "Stage3_Done";

                                        iconAnimators[6].enabled = true;
                                        iconAnimators[7].enabled = true;
                                        iconAnimators[8].enabled = true;
                                        for (sbyte j = 6; j < 9; j++)
                                        {
                                            iconAnimators[j].SetBool("KeepWaiting", true);
                                        }
                                        yield return new WaitForSeconds(0.05f);
                                        iconImages[6].enabled = true;
                                        iconImages[7].enabled = true;
                                        iconImages[8].enabled = true;

                                        StartCoroutine(CloseTheImagesWhenThePlayerClicked(6, 9));
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    // Press Any Key Yazısı Ortaya Çıkar, Konuşma Animasyonu Durdurulur Ve Sisteme Diyaloğun Bittiği Bildirilir.      
                    pressAnyKey.SetActive(true);
                    isDone = true;
                }
                // Bu Koşul, Eğer Ekrana Yazdırılan Metin Bitmişse Veya Pas Geçilmişse, Oyuncunun Bir Sonraki Adıma Geçmesine İzin Verir.
                else
                {
                    isDone = false;
                    break;
                }
            }
            yield return null;
        }
    }
    private IEnumerator CloseTheImagesWhenThePlayerClicked(sbyte minImageNumber, sbyte maxImageNumber)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                // Ekrana Çıkartılmış Olan Ikonları Kapatmak İçin.
                for (sbyte i = minImageNumber; i < maxImageNumber; i++)
                {
                    iconAnimators[i].enabled = false;
                    iconImages[i].enabled = false;
                }
                break;
            }
            yield return null;
        }
    }
    #endregion

    #region Others
    private void SwitchToTutorialMode()
    {
        // Tekrardan Öğreticiye Geçme.
        textPanel.SetActive(true);
        pressAnyKey.SetActive(true);
        gameCharacter.SetActive(false);
        tutorialCharacter.SetActive(true);

        // Parallax Mekaniğini Kapatma.
        foreach (ParallaxForBG item in pfBG)
            item.enabled = false;
        pfGRND.enabled = false;

        // Önceki Görevden Kalma Sayacı Sıfırlama.
        failedTextCount = 0;

        // Eğer Oyuncu Bir Önceki Görevi Havadayken Bitirirse, Tekrar Göreve Başlarken Havadan Başlamaması İçin Yazılmış Bir Kod.
        gameCharacter.transform.position = new Vector2(-1.0f, -0.73f);
    }
    #endregion
}