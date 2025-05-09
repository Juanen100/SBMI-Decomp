using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDrop : ItemDrop
{
	private int id;

	public RecipeDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action callback)
		: base(position, fixedOffset, direction, definition, creationTime, callback)
	{
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("RecipeCollected");
		session.TheGame.craftManager.UnlockRecipe(definition.Did, session.TheGame);
		CraftingRecipe recipeById = session.TheGame.craftManager.GetRecipeById(definition.Did);
		string arg = Language.Get(recipeById.recipeName);
		string resourceTexture = session.TheGame.resourceManager.Resources[recipeById.productId].GetResourceTexture();
		FoundItemDialogInputData item = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), arg), resourceTexture, "Beat_FoundRecipe");
		session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
		onCleanupComplete();
	}

	protected override bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return ExplodeInPlace(session, camera, updateCollectionTimer, "Prefabs/FX/Fx_Glass_Break", "RecipeDropExplodeInPlace");
	}

	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenRecipeItem");
	}

	protected override void PlayRewardAmountTextAnim(Session session)
	{
	}

	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}
}
