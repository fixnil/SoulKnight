using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsLibrary : MonoBehaviour
{
    public static ParticleEffectsLibrary GlobalAccess;

    private void Awake()
    {
        GlobalAccess = this;

        _currentActivePEList = new List<Transform>();

        TotalEffects = ParticleEffectPrefabs.Length;

        CurrentParticleEffectNum = 1;

        // Warn About Lengths of Arrays not matching
        if (ParticleEffectSpawnOffsets.Length != TotalEffects)
        {
            Debug.LogError("ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
        }

        if (ParticleEffectPrefabs.Length != TotalEffects)
        {
            Debug.LogError("ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
        }

        // Setup Starting PE Name String
        _effectNameString = ParticleEffectPrefabs[CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
    }

    // Stores total number of effects in arrays - NOTE: All Arrays must match length.
    public int TotalEffects = 0;
    public int CurrentParticleEffectIndex = 0;
    public int CurrentParticleEffectNum = 0;
    //	public string[] ParticleEffectDisplayNames;
    public Vector3[] ParticleEffectSpawnOffsets;
    // How long until Particle Effect is Destroyed - 0 = never
    public float[] ParticleEffectLifetimes;
    public GameObject[] ParticleEffectPrefabs;

    // Storing for deleting if looping particle effect
    private string _effectNameString = "";
    private List<Transform> _currentActivePEList;

    private void Start()
    {
    }

    public string GetCurrentPENameString()
    {
        return ParticleEffectPrefabs[CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
    }

    public void PreviousParticleEffect()
    {
        // Destroy Looping Particle Effects
        if (ParticleEffectLifetimes[CurrentParticleEffectIndex] == 0)
        {
            if (_currentActivePEList.Count > 0)
            {
                for (var i = 0; i < _currentActivePEList.Count; i++)
                {
                    if (_currentActivePEList[i] != null)
                    {
                        Destroy(_currentActivePEList[i].gameObject);
                    }
                }

                _currentActivePEList.Clear();
            }
        }

        // Select Previous Particle Effect
        if (CurrentParticleEffectIndex > 0)
        {
            CurrentParticleEffectIndex -= 1;
        }
        else
        {
            CurrentParticleEffectIndex = TotalEffects - 1;
        }

        CurrentParticleEffectNum = CurrentParticleEffectIndex + 1;

        // Update PE Name String
        _effectNameString = ParticleEffectPrefabs[CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
    }
    public void NextParticleEffect()
    {
        // Destroy Looping Particle Effects
        if (ParticleEffectLifetimes[CurrentParticleEffectIndex] == 0)
        {
            if (_currentActivePEList.Count > 0)
            {
                for (var i = 0; i < _currentActivePEList.Count; i++)
                {
                    if (_currentActivePEList[i] != null)
                    {
                        Destroy(_currentActivePEList[i].gameObject);
                    }
                }

                _currentActivePEList.Clear();
            }
        }

        // Select Next Particle Effect
        if (CurrentParticleEffectIndex < TotalEffects - 1)
        {
            CurrentParticleEffectIndex += 1;
        }
        else
        {
            CurrentParticleEffectIndex = 0;
        }

        CurrentParticleEffectNum = CurrentParticleEffectIndex + 1;

        // Update PE Name String
        _effectNameString = ParticleEffectPrefabs[CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
    }

    private Vector3 _spawnPosition = Vector3.zero;
    public void SpawnParticleEffect(Vector3 positionInWorldToSpawn)
    {
        // Spawn Currently Selected Particle Effect
        _spawnPosition = positionInWorldToSpawn + ParticleEffectSpawnOffsets[CurrentParticleEffectIndex];
        var newParticleEffect = GameObject.Instantiate(ParticleEffectPrefabs[CurrentParticleEffectIndex], _spawnPosition, ParticleEffectPrefabs[CurrentParticleEffectIndex].transform.rotation);
        newParticleEffect.name = "PE_" + ParticleEffectPrefabs[CurrentParticleEffectIndex];
        // Store Looping Particle Effects Systems
        if (ParticleEffectLifetimes[CurrentParticleEffectIndex] == 0)
        {
            _currentActivePEList.Add(newParticleEffect.transform);
        }

        _currentActivePEList.Add(newParticleEffect.transform);
        // Destroy Particle Effect After Lifetime expired
        if (ParticleEffectLifetimes[CurrentParticleEffectIndex] != 0)
        {
            Destroy(newParticleEffect, ParticleEffectLifetimes[CurrentParticleEffectIndex]);
        }
    }
}
