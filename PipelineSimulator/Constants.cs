namespace PipelineSimulator
{
	public class Constants
	{
		public enum PipelineStages
		{
			Blank,
			InstructionFetch,
			IF_Finished,
			InstructionDecode,
			ID_Finished,
			EX,
			EX_Finished,
			Memory,
			Memory_Finished,
			WriteBack,
			WriteBack_Finished,
			Bubble,
			Stall,
		}
	}
}