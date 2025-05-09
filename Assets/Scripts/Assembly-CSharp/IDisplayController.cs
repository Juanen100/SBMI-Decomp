using System.Collections.Generic;
using UnityEngine;

public interface IDisplayController
{
	bool Visible { get; set; }

	float Alpha { get; set; }

	Color Color { get; set; }

	bool IsDestroyed { get; }

	QuadHitObject HitObject { get; }

	string MaterialName { get; }

	Vector3 Position { get; set; }

	Transform Transform { get; }

	Vector3 BillboardScaling { get; set; }

	Vector3 Scale { get; set; }

	Vector3 Forward { get; }

	Vector3 Up { get; }

	float Width { get; }

	float Height { get; }

	int LevelOfDetail { get; set; }

	int NumberOfLevelsOfDetail { get; }

	int MaxLevelOfDetail { get; }

	bool isVisible { get; }

	string DefaultDisplayState { get; set; }

	bool isPerspectiveInArt { get; set; }

	DisplayControllerFlags Flags { get; set; }

	IDisplayController Clone(DisplayControllerManager dcm);

	IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false);

	IDisplayController CloneAndSetVisible(DisplayControllerManager dcm);

	void AddDisplayState(Dictionary<string, object> dict);

	bool Intersects(Ray ray);

	void DisplayState(string state);

	void ChangeMesh(string state, string hitMeshName);

	void UpdateMaterialOrTexture(string material);

	void SetMaskPercentage(float pct);

	string GetDisplayState();

	void Billboard(BillboardDelegate billboard);

	void OnUpdate(Camera sceneCamera, ParticleSystemManager psm);

	void Destroy();

	void AttachGUIElementToTarget(SBGUIElement element, string target);
}
