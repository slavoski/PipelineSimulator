﻿using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class ArithmeticPipelineInstruction : PipelineInstruction
	{
		#region constructor / destructor

		public ArithmeticPipelineInstruction()
		{
			ValueAvailable = PipelineStages.WB;
			ValueNeeded = PipelineStages.ID;
			ForwardingValueAvailable = PipelineStages.EX_Finished;
			ForwardingValueNeeded = PipelineStages.EX;
		}

		#endregion constructor / destructor

		#region methods

		public override IPipelineInstruction Copy()
		{
			return new ArithmeticPipelineInstruction()
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