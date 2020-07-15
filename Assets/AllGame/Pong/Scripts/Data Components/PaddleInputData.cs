using Unity.Entities;
using UnityEngine;

namespace Pong
{
	[GenerateAuthoringComponent]
	public struct PaddleInputData : IComponentData
	{
		public KeyCode upKey;
		public KeyCode downKey;
	}
}