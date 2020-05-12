using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    [SerializeField]
    public uint level;        // Level of the character.
    [SerializeField]
    public ushort hits;         // Whole number of hits the character can take
    [SerializeField]
    public ushort halfHits;     // Set to 5 totoal hits has a franction of 1/2
    [SerializeField]
    public ushort spellPoints;  // Whole number of spell points the character has (Mage or Elf)
    [SerializeField]
    public ushort halfSP;       // Set to 5 totoal spell points has a franction of 1/2
    [SerializeField]
    public uint strength;     // Strength points
    [SerializeField]
    public uint intel;          // Intelligence (IQ) points
    [SerializeField]
    public uint wisdom;       // Wisdom points
    [SerializeField]
    public uint constitution;        // Constitution points
    [SerializeField]
    public uint dex;          // Dexterity points
    [SerializeField]
    public uint agility;      // Agility points
    [SerializeField]
    public ushort size;         // Size points
    [SerializeField]
    public ulong gold;          // Amount of gold the character has

    /***
     *      This is the base creator that we need to use to access the public methods in this class.
    ***/
    public Characteristics()
    {   //Instance creator

    }   // Characteristics()
}
