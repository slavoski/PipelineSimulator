using MvvmHelpers;

namespace PipelineSimulator
{
	public interface IPipelineInstruction
	{
		public string Destination
		{
			get;
			set;
		}

		public ObservableRangeCollection<Block> ForwardingInstructionSet
		{
			get;
			set;
		}

		public int ForwardingValueAvailable
		{
			get;
			set;
		}

		public int ForwardingValueNeeded
		{
			get;
			set;
		}

		public ObservableRangeCollection<Block> HazardInstructionSet
		{
			get;
			set;
		}

		public string Instruction
		{
			get;
			set;
		}

		public ObservableRangeCollection<Block> ReorderedForwardingInstructionSet
		{
			get;
			set;
		}

		public int Row
		{
			get;
			set;
		}

		public string Source
		{
			get;
			set;
		}

		public string Source2
		{
			get;
			set;
		}

		public ObservableRangeCollection<Block> StallInstructionSet
		{
			get;
			set;
		}

		public int ValueAvailable
		{
			get;
			set;
		}

		public int ValueNeeded
		{
			get;
			set;
		}

		public void Initialize()
		{
		}
	}
}