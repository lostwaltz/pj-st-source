using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class FootStepSoundController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Vector3 lastPosition;
    private IFootstep currentFootstep;
    private Dictionary<string, SoundData> soundCache = new Dictionary<string, SoundData>();

    [Header("Sound Clips")]
    [SerializeField] private string[] dirtFootstepClips;
    [SerializeField] private string[] blockFootstepClips;
    [SerializeField] private string[] metalFootstepClips;
    [SerializeField] private string[] gungetClips;
    [SerializeField] private float movementThreshold = 0.1f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        LoadAudioClips();
        InitializeSoundData();
        UpdateFootstep();
    }
    //TODO: 더 나은 방법 생각해보기 
    private void Update()
    {
        if (rb != null && rb.velocity.magnitude > movementThreshold)
        {
            PlayGungetsSound();
        }
        else if (Vector3.Distance(transform.position, lastPosition) > movementThreshold * Time.deltaTime)
        {
            PlayGungetsSound();
        }

        // float movement = Vector3.Distance(transform.position, lastPosition);
        // if (movement > movementThreshold * Time.deltaTime)
        // {
        //     PlayGungetsSound();
        // }

        lastPosition = transform.position;

    }

    private void LoadAudioClips()
    {
        AudioClip[] dirtClips = Resources.LoadAll<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Dirt");
        AudioClip[] blockClips = Resources.LoadAll<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Block");
        AudioClip[] metalClips = Resources.LoadAll<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Metal");
        AudioClip[] gungetSounds = Resources.LoadAll<AudioClip>(Constants.Sound.Character.GUNGETS);

        if (dirtClips.Length == 0)
        {
            Debug.LogError("오디오못불러옴");
            return;
        }

        dirtFootstepClips = dirtClips.Select(clip => clip.name).ToArray();
        blockFootstepClips = blockClips.Select(clip => clip.name).ToArray();
        metalFootstepClips = metalClips.Select(clip => clip.name).ToArray();
        gungetClips = gungetSounds.Select(clip => clip.name).ToArray();
    }

    private void InitializeSoundData()
    {
        foreach (var clip in dirtFootstepClips)
        {
            soundCache[clip] = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Dirt/" + clip),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        foreach (var clip in blockFootstepClips)
        {
            soundCache[clip] = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Block/" + clip),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        foreach (var clip in metalFootstepClips)
        {
            soundCache[clip] = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.Character.FOOTSTEPS + "Metal/" + clip),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        foreach (var clip in gungetClips)
        {
            soundCache[clip] = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.Character.GUNGETS + clip),
                Volume = 0.03f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
    }

    private void UpdateFootstep()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            TerrainType terrainType = GetTerrainType(hit.collider);
            currentFootstep = terrainType switch
            {
                TerrainType.Dirt => new DirtFootstep(dirtFootstepClips, soundCache),
                TerrainType.Metal => new MetalFootstep(metalFootstepClips, soundCache),
                TerrainType.Block => new BlockFootstep(blockFootstepClips, soundCache),
                _ => new DirtFootstep(dirtFootstepClips, soundCache)
            };
        }
    }

    private TerrainType GetTerrainType(Collider collider)
    {
        if (collider.CompareTag("Dirt")) return TerrainType.Dirt;
        if (collider.CompareTag("Block")) return TerrainType.Block;
        if (collider.CompareTag("Metal")) return TerrainType.Metal;
        return TerrainType.Default;
    }

    public void PlayFootstepSound()
    {
        UpdateFootstep();
        if (currentFootstep == null)
        {
            Debug.Log("currentFootstep is null");
            return;
        }
        currentFootstep.PlayFootstep(transform.position);
    }


    private float lastGungetsSoundTime = 0f;
    private const float GUNGETS_SOUND_INTERVAL = 0.4f;

    private void PlayGungetsSound()
    {
        if (Time.time - lastGungetsSoundTime < GUNGETS_SOUND_INTERVAL) return;
        if (gungetClips.Length == 0) return;

        string randomSound = gungetClips[Random.Range(0, gungetClips.Length)];
        Core.SoundManager.CreateSoundBuilder()
        .WithPosition(transform.position)
        .WithRandomPitch()
        .Play(soundCache[randomSound]);

        lastGungetsSoundTime = Time.time;
    }

}
public interface IFootstep
{
    void PlayFootstep(Vector3 position);
}

public class DirtFootstep : IFootstep
{
    private readonly string[] clipNames;
    private readonly Dictionary<string, SoundData> soundCache;

    public DirtFootstep(string[] clipNames, Dictionary<string, SoundData> soundCache)
    {
        this.clipNames = clipNames;
        this.soundCache = soundCache;
    }

    public void PlayFootstep(Vector3 position)
    {
        if (clipNames.Length == 0) return;
        string randomSound = clipNames[Random.Range(0, clipNames.Length)];
        Core.SoundManager.CreateSoundBuilder()
            .WithPosition(position)
            .WithRandomPitch()
            .Play(soundCache[randomSound]);
    }
}

public class BlockFootstep : IFootstep
{
    private readonly string[] clipNames;
    private readonly Dictionary<string, SoundData> soundCache;

    public BlockFootstep(string[] clipNames, Dictionary<string, SoundData> soundCache)
    {
        this.clipNames = clipNames;
        this.soundCache = soundCache;
    }

    public void PlayFootstep(Vector3 position)
    {
        if (clipNames.Length == 0) return;
        string randomSound = clipNames[Random.Range(0, clipNames.Length)];
        Core.SoundManager.CreateSoundBuilder()
            .WithPosition(position)
            .WithRandomPitch()
            .Play(soundCache[randomSound]);
    }
}

public class MetalFootstep : IFootstep
{
    private readonly string[] clipNames;
    private readonly Dictionary<string, SoundData> soundCache;

    public MetalFootstep(string[] clipNames, Dictionary<string, SoundData> soundCache)
    {
        this.clipNames = clipNames;
        this.soundCache = soundCache;
    }

    public void PlayFootstep(Vector3 position)
    {
        if (clipNames.Length == 0) return;
        string randomSound = clipNames[Random.Range(0, clipNames.Length)];
        Core.SoundManager.CreateSoundBuilder()
            .WithPosition(position)
            .WithRandomPitch()
            .Play(soundCache[randomSound]);
    }
}

public enum TerrainType
{
    Dirt,
    Metal,
    Block,
    Default
}

