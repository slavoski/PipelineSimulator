using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	internal class StorePipelineInstruction : PipelineInstruction
	{
		#region constructor / destructor

		public StorePipelineInstruction()
		{
			ValueAvailable = PipelineStages.WB;
			ValueNeeded = PipelineStages.ID;
			ForwardingValueAvailable = PipelineStages.Blank;
			ForwardingValueNeeded = PipelineStages.DMEM;
		}

		#endregion constructor / destructor

		#region methods

		public override IPipelineInstruction Copy()
		{
			return new StorePipelineInstruction()
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