using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    [SerializeField] public List<RecipeSO> recipeSOList;
}
