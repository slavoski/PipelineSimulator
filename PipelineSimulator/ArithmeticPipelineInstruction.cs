using MvvmHelpers;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class ArithmeticPipelineInstruction : BaseViewModel, IPipelineInstruction
	{
		public string Destination
		{
			get;
			set;
		} = string.Empty;

		public ObservableRangeCollection<Block> ForwardingInstructionSet
		{
			get;
			set;
		} = new ObservableRangeCollection<Block>();

		public int ForwardingValueAvailable
		{
			get;
			set;
		} = (int)PipelineStages.Memory;

		public int ForwardingValueNeeded
		{
			get;
			set;
		} = (int)PipelineStages.ALU;

		public ObservableRangeCollection<Block> HazardInstructionSet
		{
			get;
			set;
		} = new ObservableRangeCollection<Block>();

		public string Instruction
		{
			get;
			set;
		} = string.Empty;

		public ObservableRangeCollection<Block> ReorderedForwardingInstructionSet
		{
			get;
			set;
		} = new ObservableRangeCollection<Block>();

		public int Row
		{
			get;
			set;
		}

		public string Source
		{
			get;
			set;
		} = string.Empty;

		public string Source2
		{
			get;
			set;
		} = string.Empty;

		public ObservableRangeCollection<Block> StallInstructionSet
		{
			get;
			set;
		} = new ObservableRangeCollection<Block>();

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

		#region methods

		public void Initialize()
		{
			for (int i = 0; i < Row; i++)
			{
				HazardInstructionSet.Add(new Block("", true));
				HazardInstructionSet.Add(new Block(""));
				ReorderedForwardingInstructionSet.Add(new Block("", true));
				ReorderedForwardingInstructionSet.Add(new Block(""));
				StallInstructionSet.Add(new Block("", true));
				StallInstructionSet.Add(new Block(""));
				ForwardingInstructionSet.Add(new Block("", true));
				ForwardingInstructionSet.Add(new Block(""));
			}
			SetupForwarding();
			SetupHazard();
			SetupReorderedForwarding();
			SetupStall();
		}

		private void SetupForwarding()
		{
		}

		private void SetupHazard()
		{
			HazardInstructionSet.Add(new Block("IF") { IsHalfBackground = false });
			HazardInstructionSet.Add(new Block(""));
			HazardInstructionSet.Add(new Block("ID") { IsHalfBackground = true });
			HazardInstructionSet.Add(new Block(""));

			Block block;
			if (Row == 0)
			{
				block = new Block("ALU") { IsHalfBackground = false, IsStart = true };
			}
			else
			{
				block = new Block("ALU") { IsHalfBackground = false };
			}
			HazardInstructionSet.Add(block);
			HazardInstructionSet.Add(new Block(""));

			if (Row == 1)
			{
				block = new Block("DMEM") { IsHalfBackground = false, IsEnd = true };
			}
			else
			{
				block = new Block("DMEM") { IsHalfBackground = false };
			}

			HazardInstructionSet.Add(block);
			HazardInstructionSet.Add(new Block(""));
			HazardInstructionSet.Add(new Block("WB") { IsHalfBackground = true });
		}

		private void SetupReorderedForwarding()
		{
		}

		private void SetupStall()
		{
		}

		#endregion methods
	}
}