namespace PipelineSimulator
{
	public class Constants
	{
		public enum PipelineStages
		{
			Blank,
			IF,
			IF_Finished,
			ID,
			ID_Finished,
			EX,
			EX_Finished,
			DMEM,
			DMEM_Finished,
			WB,
			WB_Finished,
			Bubble,
			Stall,
		}
	}
}