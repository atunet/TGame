using UnityEngine;
using System.Collections;

public class BGMController : MonoBehaviour 
{
    private static BGMController s_instance = null;
    public static BGMController Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject obj = new GameObject("BGMController");
                s_instance = obj.AddComponent<BGMController>(); 
                DontDestroyOnLoad(obj);
            }

            return s_instance;
        }
    }

    private AudioSource m_asMusic = null;
    private AudioSource m_asSound = null;

    //============ music ==============
    private AudioClip m_acMain = null;
    private AudioClip m_acFight = null;
   
    //============ sound ==============
    private AudioClip m_itemCollision = null;
    private AudioClip m_rocketEngine = null;
    private AudioClip m_acOk = null;
    private AudioClip m_acCancel = null;
    private AudioClip m_acSelectItem = null;
    private AudioClip m_acPowerUsed = null;
    private AudioClip m_acStar01 = null;
    private AudioClip m_acStar02 = null;
    private AudioClip m_acStar03 = null;
       
    private float m_musicVolume = 1f;
    private float m_soundVolume = 1f;
        
        
	void Start () 
    {        
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            m_musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else 
            PlayerPrefs.SetFloat("MusicVolume", m_musicVolume);
        
        
        if(PlayerPrefs.HasKey("SoundVolume"))
        {
            m_soundVolume = PlayerPrefs.GetFloat("SoundVolume");
        }
        else 
            PlayerPrefs.SetFloat("SoundVolume", m_soundVolume);
        
        m_asSound = gameObject.AddComponent<AudioSource>();
        m_asMusic = gameObject.AddComponent<AudioSource>();
        m_asMusic.loop = true;

        StartCoroutine(LoadMusicAB());
	}
    
    private IEnumerator LoadMusicAB()
    {
        WWW musicWWW = new WWW("path/to/music assetbundle");
        yield return musicWWW;

        AssetBundle musicAB = musicWWW.assetBundle;

        m_acMain = musicAB.LoadAsset("bgm_main") as AudioClip;
        m_acFight = musicAB.LoadAsset("bgm_fight") as AudioClip;
        m_acOk = musicAB.LoadAsset("sound_ok") as AudioClip;
        m_acCancel = musicAB.LoadAsset("sound_cancel") as AudioClip;
        m_acStar01 = musicAB.LoadAsset("sound_star01") as AudioClip;
        m_acStar02 = musicAB.LoadAsset("sound_star02") as AudioClip;
        m_acStar03 = musicAB.LoadAsset("sound_star03") as AudioClip;

        musicAB.Unload(false);

        PlayMusic(EBGMusic.BG_MUSIC_MAIN);
    }
    
    
    public void PlayMusic(EBGMusic bgm_)
    {
        if(bgm_ == EBGMusic.BG_MUSIC_FIGHT)
        {
            if(m_asMusic.isPlaying) m_asMusic.Stop();
            m_asMusic.volume = m_musicVolume;
            m_asMusic.clip = m_acFight;
            m_asMusic.Play();
        }
        else if(bgm_ == EBGMusic.BG_MUSIC_MAIN)
        {
            if(m_asMusic.clip != m_acMain)
            {
                if(m_asMusic.isPlaying) m_asMusic.Stop();
                m_asMusic.clip = m_acMain;
            }
            
            if(!m_asMusic.isPlaying)
            {
                m_asMusic.volume = m_musicVolume;
                m_asMusic.Play();
            }
        }
    }
    
    public void StopMusic()
    {
        m_asMusic.Stop();
        m_asMusic.clip = null;
    }
    
    public void PlaySound(ESound sound_)
    {
        //Debug.Log("Sound volume:" + s_soundVolume);
        
        if (sound_ == ESound.SOUND_OK)
        {
            m_asSound.clip = m_acOk;
        } else if (sound_ == ESound.SOUND_CANCEL)
        {
            m_asSound.clip = m_acCancel;
        } else if (sound_ == ESound.SOUND_SELECT_ITEM)
        {
            m_asSound.clip = m_acSelectItem;
        } else if (sound_ == ESound.SOUND_POWER_USED)
        {
            m_asSound.clip = m_acPowerUsed;
        } else if (sound_ == ESound.SOUND_STAR_01)
        {
            m_asSound.clip = m_acStar01;
        } else if (sound_ == ESound.SOUND_STAR_02)
        {
            m_asSound.clip = m_acStar02;
        } else if (sound_ == ESound.SOUND_STAR_03)
        {
            m_asSound.clip = m_acStar03;
        } else if (sound_ == ESound.SOUND_ITEM_COLLISION)
        {
            m_asSound.clip = m_itemCollision;
        }
        
        m_asSound.volume = m_soundVolume;
        m_asSound.Play();
    }
    
}
