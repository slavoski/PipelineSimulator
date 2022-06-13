using MvvmHelpers;
using System.Windows.Media;

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

		public int ForwardingValueAvailable
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

		public void ClearAll();

		public IPipelineInstruction Copy();

		public void Initialize(int row);

		public void InsertStall();

		public void SetHazard(string hazard, string block, SolidColorBrush colorBrush);

		public void SetupSolutions();

		#endregion methods
	}
}