using System.Linq;

namespace PipelineSimulator
{
	public class PipelineManager
	{
		#region properties

		public ColorManager ColorManager
		{
			get;
			set;
		} = new ColorManager();

		public PipelineSet ForwardingPipeline
		{
			get;
			set;
		} = new PipelineSet();

		public PipelineSet HazardPipeline
		{
			get;
			set;
		} = new PipelineSet();

		public bool IsUnifiedMemory
		{
			get;
			set;
		}

		public PipelineSet StallPipeline
		{
			get;
			set;
		} = new PipelineSet();

		private PipelineSet HazardWithChanges
		{
			get;
			set;
		} = new PipelineSet();

		#endregion properties

		#region methods

		internal void AddNewInstruction(IPipelineInstruction newInstruction)
		{
			newInstruction.Initialize(HazardPipeline.PipelineInstructions.Count);
			CheckForStructuralHazard(newInstruction, true);
			HazardPipeline.AddNewPipelineInstruction(newInstruction);

			var stallInstruction = newInstruction.Copy();
			stallInstruction.Initialize(StallPipeline.PipelineInstructions.Count);
			CheckForStructuralHazard(stallInstruction, false);
			StallPipeline.AddNewPipelineInstruction(stallInstruction);
		}

		internal void ClearLists()
		{
			HazardPipeline.ClearLists();
			ForwardingPipeline.ClearLists();
			StallPipeline.ClearLists();
		}

		private void CheckForStructuralHazard(IPipelineInstruction newInstruction, bool isHazard)
		{
			var lastInstructions = (isHazard ? HazardPipeline : StallPipeline).PipelineInstructions.Where(p => p.Instruction != "").TakeLast(3);
			var instruction = lastInstructions.FirstOrDefault();
			if (instruction != null)
			{
				if (IsUnifiedMemory && CheckForStructuralHazards(instruction, newInstruction))
				{
					if (isHazard)
					{
						var color = ColorManager.GetColor();
						newInstruction.SetHazard("Structural", "IF", color);
						instruction.SetHazard("Structural", "DMEM", color);
					}
					else
					{
						var bubble = new BubblePipelineInstruction() { Instruction = "" };
						bubble.Initialize(StallPipeline.PipelineInstructions.Count);
						StallPipeline.AddNewPipelineInstruction(bubble);
						newInstruction.ClearAll();
						newInstruction.Initialize(newInstruction.Row = StallPipeline.PipelineInstructions.Count);
					}
				}
			}
		}

		private bool CheckForStructuralHazards(IPipelineInstruction originalInstructions, IPipelineInstruction newInstruction)
		{
			var result = false;

			var newIF = newInstruction.InstructionBlocks.First(p => p.Stage == Constants.PipelineStages.InstructionFetch);

			if (originalInstructions.InstructionBlocks.Count > newIF.Index)
			{
				if (originalInstructions.InstructionBlocks.FirstOrDefault(p => p.Index == newIF.Index)?.Stage == Constants.PipelineStages.Memory)
				{
					result = true;
				}
			}

			return result;
		}

		#endregion methods
	}
}