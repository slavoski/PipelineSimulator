using System.Linq;
using System.Windows.Media;
using static PipelineSimulator.Constants;

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

		#endregion properties

		#region methods

		public void CheckForDataHazardOfHazardPipeline()
		{
			for (int i = 0; i < HazardPipeline.PipelineInstructions.Count; ++i)
			{
				var currentPipeline = HazardPipeline.PipelineInstructions[i];
				var color = ColorManager.GetColor();

				var descendant = GetDescendant(i + 1, HazardPipeline);
				var descendant2 = GetDescendant(i + 2, HazardPipeline);
				var descendant3 = GetDescendant(i + 3, HazardPipeline);

				if (descendant != null && SetHazardColor(currentPipeline, descendant, color))
				{
					continue;
				}
				if (descendant2 != null && SetHazardColor(currentPipeline, descendant2, color))
				{
					continue;
				}
				if (descendant3 != null && SetHazardColor(currentPipeline, descendant3, color))
				{
					continue;
				}
			}

			//int registers = CheckForDependency(instruction, newInstruction);

			//if (registers != 0)
			//{
			//	instruction.SetHazard("Data", PipelineStages.WB, color, 3);
			//	newInstruction.SetHazard("Data", PipelineStages.ID, color, registers);

			//	break;
			//}
		}

		internal void AddColors()
		{
			for (int i = 0; i < ForwardingPipeline.PipelineInstructions.Count; ++i)
			{
				var currentPipeline = ForwardingPipeline.PipelineInstructions[i];
				var color = ColorManager.GetColor();

				for (int j = 1; j < 4; j++)
				{
					var index = i + j;
					if (index >= ForwardingPipeline.PipelineInstructions.Count)
						break;

					var descendant = ForwardingPipeline.PipelineInstructions[index];
					var registers = CheckForDependency(currentPipeline, descendant);
					if (registers != 0)
					{
						descendant.SetHazard("", descendant.ForwardingValueNeeded, color, registers);
						currentPipeline.SetHazard("", currentPipeline.ForwardingValueAvailable, color);
					}

					if (string.Equals(currentPipeline.Destination, descendant.Destination))
					{
						break;
					}
				}
			}
		}

		internal void AddNewInstruction(IPipelineInstruction newInstruction)
		{
			AddInstructionToHazardPipeline(newInstruction);
			AddInstructionToStallPipeline(newInstruction.Copy());
			AddInstructionToForwardingPipeline(newInstruction.Copy());
		}

		internal void ClearLists()
		{
			HazardPipeline.ClearLists();
			ForwardingPipeline.ClearLists();
			StallPipeline.ClearLists();
		}

		private void AddInstructionToForwardingPipeline(IPipelineInstruction forwardInstruction)
		{
			forwardInstruction.Initialize(ForwardingPipeline.Row);
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(forwardInstruction, false, true);
			}
			CheckForDataHazard(forwardInstruction, true);
			ForwardingPipeline.AddNewPipelineInstruction(forwardInstruction);
		}

		private void AddInstructionToHazardPipeline(IPipelineInstruction hazardInstruction)
		{
			hazardInstruction.Initialize(HazardPipeline.PipelineInstructions.Count);
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(hazardInstruction, true);
			}
			HazardPipeline.AddNewPipelineInstruction(hazardInstruction);
		}

		private void AddInstructionToStallPipeline(IPipelineInstruction stallInstruction)
		{
			stallInstruction.Initialize(StallPipeline.Row);
			if (IsUnifiedMemory)
			{
				CheckForStructuralHazard(stallInstruction, false);
			}
			CheckForDataHazard(stallInstruction);
			StallPipeline.AddNewPipelineInstruction(stallInstruction);
		}

		private void CheckForDataHazard(IPipelineInstruction newInstruction, bool isForwarding = false)
		{
			var lastInstructions = (isForwarding ? ForwardingPipeline : StallPipeline).PipelineInstructions.Where(p => p.Instruction != "").TakeLast(3);

			foreach (var instruction in lastInstructions.Reverse())
			{
				int row = isForwarding ? ForwardingPipeline.Row : StallPipeline.Row;
				newInstruction.CheckForDataHazard(instruction, isForwarding, ref row);

				if (isForwarding)
				{
					ForwardingPipeline.Row = row;
				}
				else
				{
					StallPipeline.Row = row;
				}
			}
		}

		private int CheckForDependency(IPipelineInstruction origPipeline, IPipelineInstruction newPipeline)
		{
			var reg = 0;

			if (origPipeline.Destination == newPipeline.Source)
			{
				reg = 1;
			}

			if (origPipeline.Destination == newPipeline.Source2)
			{
				reg += 2;
			}

			return reg;
		}

		private bool CheckForSameRegister(IPipelineInstruction pipelineInstruction, IPipelineInstruction newInstruction) =>
			string.Equals(pipelineInstruction.Destination, newInstruction.Source)
			|| (string.Equals(pipelineInstruction.Destination, newInstruction.Source2));

		private void CheckForStructuralHazard(IPipelineInstruction newInstruction, bool isHazard, bool isForwarding = false)
		{
			var lastInstructions = (isHazard ? HazardPipeline : isForwarding ? ForwardingPipeline : StallPipeline).PipelineInstructions.Where(p => p.Instruction != "").TakeLast(3);
			var instruction = lastInstructions.FirstOrDefault();
			if (instruction != null)
			{
				if (CheckStructuralRegisters(instruction, newInstruction))
				{
					if (isHazard)
					{
						var color = ColorManager.GetColor();
						newInstruction.SetHazard("Structural", PipelineStages.IF, color);
						instruction.SetHazard("Structural", PipelineStages.DMEM, color);
					}
					else
					{
						if (isForwarding)
						{
							ForwardingPipeline.AddBubble(newInstruction);
						}
						else
						{
							StallPipeline.AddBubble(newInstruction);
						}
					}
				}
			}
		}

		private bool CheckStructuralRegisters(IPipelineInstruction originalInstructions, IPipelineInstruction newInstruction)
		{
			var result = false;

			var newIF = newInstruction.InstructionBlocks.First(p => p.Stage == PipelineStages.IF);
			var origDMEM = originalInstructions.InstructionBlocks.First(p => p.Stage == PipelineStages.DMEM);
			if (origDMEM.Index == newIF.Index)
			{
				result = true;
			}

			return result;
		}

		private IPipelineInstruction GetDescendant(int index, PipelineSet pipelineSet)
		{
			if (index < pipelineSet.PipelineInstructions.Count)
			{
				return pipelineSet.PipelineInstructions[index];
			}
			else
			{
				return null;
			}
		}

		private bool SetHazardColor(IPipelineInstruction original, IPipelineInstruction descendant, SolidColorBrush color)
		{
			int registers = CheckForDependency(original, descendant);
			bool isDestinationRegisterEqual = false;

			if (registers != 0)
			{
				original.SetHazard("Data", PipelineStages.WB, color, 3);
				descendant.SetHazard("Data", PipelineStages.ID, color, registers);
			}

			if (string.Equals(descendant.Destination, original.Destination))
			{
				isDestinationRegisterEqual = true;
			}

			return isDestinationRegisterEqual;
		}

		#endregion methods
	}
}