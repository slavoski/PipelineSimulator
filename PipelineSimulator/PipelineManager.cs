using System.Linq;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class PipelineManager
	{
		#region member variables

		private int _forwardPipelineIndex = 0;
		private int _stallPipelineIndex = 0;

		#endregion member variables

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
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(newInstruction, true);
			}
			CheckForDataHazard(newInstruction, true);
			HazardPipeline.AddNewPipelineInstruction(newInstruction);

			var stallInstruction = newInstruction.Copy();
			stallInstruction.Initialize(_stallPipelineIndex);
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(stallInstruction, false);
			}
			CheckForDataHazard(stallInstruction, false);
			AddInstructionToStallPipeline(stallInstruction);

			var forwardInstruction = newInstruction.Copy();
			forwardInstruction.Initialize(_forwardPipelineIndex);
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(forwardInstruction, false);
			}
			CheckForDataHazard(forwardInstruction, false, true);
			AddInstructionToForwardingPipeline(forwardInstruction);
		}

		internal void ClearLists()
		{
			HazardPipeline.ClearLists();
			ForwardingPipeline.ClearLists();
			StallPipeline.ClearLists();
			_stallPipelineIndex = 0;
			_forwardPipelineIndex = 0;
		}

		private void AddInstructionToForwardingPipeline(IPipelineInstruction forwardInstruction)
		{
			_forwardPipelineIndex++;
			ForwardingPipeline.AddNewPipelineInstruction(forwardInstruction);
		}

		private void AddInstructionToStallPipeline(IPipelineInstruction stallInstruction)
		{
			_stallPipelineIndex++;
			StallPipeline.AddNewPipelineInstruction(stallInstruction);
		}

		private void CheckForDataHazard(IPipelineInstruction newInstruction, bool isHazard, bool isForwarding = false)
		{
			var lastInstructions = (isHazard ? HazardPipeline : isForwarding ? ForwardingPipeline : StallPipeline).PipelineInstructions.Where(p => p.Instruction != "").TakeLast(3);
			var color = ColorManager.GetColor();
			foreach (var instruction in lastInstructions.Reverse())
			{
				if (isHazard)
				{
					if (CheckForSameRegister(instruction, newInstruction))
					{
						instruction.SetHazard("Data", PipelineStages.WB.ToString(), color);
						newInstruction.SetHazard("Data", PipelineStages.ID.ToString(), color);
					}
				}
				else
				{
					newInstruction.CheckForDataHazard(instruction, isForwarding, ref isForwarding ? ref _forwardPipelineIndex : ref _stallPipelineIndex);
				}
			}
		}

		private bool CheckForSameRegister(IPipelineInstruction pipelineInstruction, IPipelineInstruction newInstruction) =>
			string.Equals(pipelineInstruction.Destination, newInstruction.Source)
			|| (string.Equals(pipelineInstruction.Destination, newInstruction.Source2));

		private void CheckForStructuralHazard(IPipelineInstruction newInstruction, bool isHazard)
		{
			var lastInstructions = (isHazard ? HazardPipeline : StallPipeline).PipelineInstructions.Where(p => p.Instruction != "").TakeLast(3);
			var instruction = lastInstructions.FirstOrDefault();
			if (instruction != null)
			{
				if (CheckForStructuralHazards(instruction, newInstruction))
				{
					if (isHazard)
					{
						var color = ColorManager.GetColor();
						newInstruction.SetHazard("Structural", PipelineStages.IF.ToString(), color);
						instruction.SetHazard("Structural", PipelineStages.DMEM.ToString(), color);
					}
					else
					{
						var bubble = new BubblePipelineInstruction() { Instruction = "" };
						bubble.Initialize(StallPipeline.PipelineInstructions.Count);
						StallPipeline.AddNewPipelineInstruction(bubble);
						newInstruction.ClearAll();
						newInstruction.Initialize(StallPipeline.PipelineInstructions.Count);
					}
				}
			}
		}

		private bool CheckForStructuralHazards(IPipelineInstruction originalInstructions, IPipelineInstruction newInstruction)
		{
			var result = false;

			var newIF = newInstruction.InstructionBlocks.First(p => p.Stage == PipelineStages.IF);

			if (originalInstructions.InstructionBlocks.Count > newIF.Index)
			{
				if (originalInstructions.InstructionBlocks.FirstOrDefault(p => p.Index == newIF.Index)?.Stage == Constants.PipelineStages.DMEM)
				{
					result = true;
				}
			}

			return result;
		}

		#endregion methods
	}
}