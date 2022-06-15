using MvvmHelpers;

namespace PipelineSimulator
{
	public class PipelineSet
	{
		#region member variables

		private int _row = 0;

		#endregion member variables

		#region properties

		public ObservableRangeCollection<IPipelineInstruction> PipelineInstructions
		{
			get;
			set;
		} = new ObservableRangeCollection<IPipelineInstruction>();

		public int Row
		{
			get => _row;
			set
			{
				_row = value;
			}
		}

		#endregion properties

		#region methods

		internal void AddBubble(IPipelineInstruction pipelineInstruction)
		{
			var bubble = new BubblePipelineInstruction() { Instruction = "" };
			bubble.Initialize(_row);
			AddNewPipelineInstruction(bubble);
			pipelineInstruction.ClearAll();
			pipelineInstruction.Initialize(_row);
		}

		internal void AddNewPipelineInstruction(IPipelineInstruction pipelineInstruction)
		{
			pipelineInstruction.SetupSolutions();
			pipelineInstruction.Row = _row;
			_row++;
			PipelineInstructions.Add(pipelineInstruction);
		}

		internal void ClearLists()
		{
			_row = 0;
			foreach (var pipelineInstruction in PipelineInstructions)
			{
				pipelineInstruction.ClearAll();
			}

			PipelineInstructions.Clear();
		}

		#endregion methods
	}
}