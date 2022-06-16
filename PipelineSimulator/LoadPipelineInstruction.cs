using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class LoadPipelineInstruction : PipelineInstruction
	{
		#region constructor / destructor

		public LoadPipelineInstruction()
		{
			ValueAvailable = PipelineStages.WB;
			ValueNeeded = PipelineStages.ID;
			ForwardingValueAvailable = PipelineStages.DMEM_Finished;
			ForwardingValueNeeded = PipelineStages.DMEM;
		}

		#endregion constructor / destructor

		#region methods

		public override IPipelineInstruction Copy()
		{
			return new LoadPipelineInstruction()
			{
				Command = this.Command,
				Destination = this.Destination,
				ForwardingValueAvailable = this.ForwardingValueAvailable,
				Hazard = this.Hazard,
				Instruction = this.Instruction,
				Source = this.Source,
				Source2 = this.Source2,
				ValueAvailable = this.ValueAvailable,
				ValueNeeded = this.ValueNeeded,
			};
		}

		#endregion methods
	}
}