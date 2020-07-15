using Unity.Entities;


namespace Pong
{
	[GenerateAuthoringComponent]
	public struct PaddleMovementData : IComponentData
	{
		public int direction;
		public float speed;
	}
}