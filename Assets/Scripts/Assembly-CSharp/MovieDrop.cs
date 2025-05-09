using System;
using System.Collections.Generic;
using UnityEngine;

public class MovieDrop : ItemDrop
{
	public MovieDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action callback)
		: base(position, fixedOffset, direction, definition, creationTime, callback)
	{
	}

	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("MovieCollected");
		session.TheGame.movieManager.UnlockMovie(definition.Did);
		MovieInfo movieInfoById = session.TheGame.movieManager.GetMovieInfoById(definition.Did);
		FoundMovieDialogInputData item = new FoundMovieDialogInputData(Language.Get("!!MOVIE_UNLOCKED_TITLE"), string.Format(Language.Get("!!MOVIE_UNLOCKED_DIALOG"), Language.Get(movieInfoById.CollectName)), "MovieIcon.png", movieInfoById.MovieFile, "Beat_FoundMovie");
		session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
		onCleanupComplete();
	}

	protected override bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return ExplodeInPlace(session, camera, updateCollectionTimer, "Prefabs/FX/Fx_Film_Roll", "MovieDropExplodeInPlace");
	}

	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenMovieItem");
	}

	protected override void PlayRewardAmountTextAnim(Session session)
	{
	}

	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}
}
