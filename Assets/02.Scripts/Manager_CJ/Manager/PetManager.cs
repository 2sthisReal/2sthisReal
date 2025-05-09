using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    private readonly List<PetData> selectedPets = new();

    public void Add(PetData pet)
    {
        if (!selectedPets.Contains(pet))
        {
            selectedPets.Add(pet);
            Debug.Log($"[PetManager] Pet added: {pet.petName}");
        }
    }

    public void Remove(PetData pet)
    {
        if (selectedPets.Remove(pet))
        {
            Debug.Log($"[PetManager] Pet removed: {pet.petName}");
        }
    }

    public List<PetData> GetAll()
    {
        return new List<PetData>(selectedPets);
    }

    public void Clear()
    {
        selectedPets.Clear();
        Debug.Log("[PetManager] All selected pets cleared.");
    }

    public bool Has(string petID)
    {
        return selectedPets.Exists(pet => pet.petID == petID);
    }
}
