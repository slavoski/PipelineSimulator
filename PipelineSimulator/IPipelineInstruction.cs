using MvvmHelpers;
using System.Windows.Media;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public interface IPipelineInstruction
	{
		public string Command
		{
			get;
			set;
		}

		public string Destination
		{
			get;
			set;
		}

		public PipelineStages ForwardingValueAvailable
		{
			get;
			set;
		}

		public PipelineStages ForwardingValueNeeded
		{
			get;
			set;
		}

		public string Hazard
		{
			get;
			set;
		}

		public string Instruction
		{
			get;
			set;
		}

		public ObservableRangeCollection<Block> InstructionBlocks
		{
			get;
			set;
		}

		public ObservableRangeCollection<string> Instructions
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

		public int Stalls
		{
			get;
			set;
		}

		public PipelineStages ValueAvailable
		{
			get;
			set;
		}

		public PipelineStages ValueNeeded
		{
			get;
			set;
		}

		#region methods

		public bool CheckForDataHazard(IPipelineInstruction pipelineInstruction, bool isForwarding, ref int rowIndex);

		public void ClearAll();

		public IPipelineInstruction Copy();

		public Block GetValueAvailableBlock(bool isForwarding);

		public Block GetValueNeededBlock(bool isForwarding);

		public void Initialize(int row);

		public void SetHazard(string hazard, string block, SolidColorBrush colorBrush);

		public void SetupSolutions();

		#endregion methods
	}
}