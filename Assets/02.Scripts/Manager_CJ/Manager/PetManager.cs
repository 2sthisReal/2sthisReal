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
        }
    }

    public void Remove(PetData pet)
    {
        if (selectedPets.Remove(pet))
        {
        }
    }

    public List<PetData> GetAll()
    {
        return new List<PetData>(selectedPets);
    }

    public void Clear()
    {
        selectedPets.Clear();
    }

    public bool Has(string petID)
    {
        return selectedPets.Exists(pet => pet.petID == petID);
    }
}
