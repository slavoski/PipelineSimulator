namespace PipelineSimulator
{
	public class Constants
	{
		public enum PipelineStages
		{
			InstructionFetch,
			DecodeInstruction,
			ALU,
			Memory,
			WriteBack
		}
	}
}