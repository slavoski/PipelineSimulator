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

		#endregion properties

		#region methods

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
			foreach(var pipelineInstruction in PipelineInstructions)
			{
				pipelineInstruction.ClearAll();
			}

			PipelineInstructions.Clear();
		}

		#endregion methods
	}
}