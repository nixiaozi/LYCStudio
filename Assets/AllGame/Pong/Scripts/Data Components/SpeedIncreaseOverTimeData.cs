using Unity.Entities;

namespace Pong
{
	[GenerateAuthoringComponent]
	public struct SpeedIncreaseOverTimeData : IComponentData
	{
		public float increasePerSecond;
	}
}