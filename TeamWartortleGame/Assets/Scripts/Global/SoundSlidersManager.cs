//Si occupa di aggiornare i valori della musica e salvarli
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlidersManager : MonoBehaviour, IUpdateData
{ 
    //riferimento al GameManag di scena
    [SerializeField]
    private GameManag g = default;

    private float musicValue, //indica il volume della musica di gioco
        sfxValue, //indica il volume degli effetti sonori di gioco
        masterValue; //indica il volume di tutti i suoni di gioco

    //riferimento all'audio mixer master
    [SerializeField]
    private AudioMixer master = default;
    //riferimenti agli slider dei volumi
    [SerializeField]
    private Slider musicSlider = default,
        sfxSlider = default,
        masterSlider = default;
   

    private void Start()
    {
       
        musicSlider.value = g.savedMusicVolume;
        sfxSlider.value = g.savedSfxVolume;
        masterSlider.value = g.savedMasterVolume;

        /*
        musicValue = musicSlider.value;
        sfxValue = sfxSlider.value;
        masterValue = masterSlider.value;

        //aggiorna i valori dell'audio mixer
        master.SetFloat("MASTER_volume", masterValue);
        master.SetFloat("MUSICHE_volume", musicValue);
        master.SetFloat("SFX_volume", sfxValue);*/

        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSfxVolume();

    }


    public void UpdateData()
    {
        //aggiorna i dati riguardanti le musiche di gioco
        g.savedMusicVolume = musicValue;
        g.savedSfxVolume = sfxValue;
        g.savedMasterVolume = masterValue;
        Debug.Log("Dati dei volumi aggiornati");
    }

    public void ChangeMusicVolume() { musicValue = musicSlider.value; master.SetFloat("MusicVolume", musicValue); }
    public void ChangeSfxVolume() { sfxValue = sfxSlider.value; master.SetFloat("SfxVolume", sfxValue); }
    public void ChangeMasterVolume() { masterValue = masterSlider.value; master.SetFloat("MasterVolume", masterValue); }


}
