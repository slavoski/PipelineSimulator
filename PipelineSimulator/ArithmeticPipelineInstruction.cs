using MvvmHelpers;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class ArithmeticPipelineInstruction : PipelineInstruction
	{
		#region constructor / destructor

		public ArithmeticPipelineInstruction()
		{
			ValueAvailable = (int)PipelineStages.Memory;
			ValueNeeded = (int)PipelineStages.EX;
			ForwardingValueAvailable = (int)PipelineStages.EX_Finished;
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

		public override void Initialize(int row)
		{
			Row = row;

			row++;

			AddBlankLines();

			InstructionBlocks.Add(new Block("IF", row++) { IsHalfBackground = false, Stage = PipelineStages.InstructionFetch });

			AddStalls();

			InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.IF_Finished });
			InstructionBlocks.Add(new Block("ID", row++) { IsHalfBackground = true, IsHalfBlock = Visibility.Visible, Stage = PipelineStages.InstructionDecode });
			InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.ID_Finished });
			InstructionBlocks.Add(new Block("ALU", row++) { IsHalfBackground = false, Stage = PipelineStages.EX });
			InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.EX_Finished });
			InstructionBlocks.Add(new Block("DMEM", row++) { IsHalfBackground = false, Stage = PipelineStages.Memory });
			InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.Memory_Finished });
			InstructionBlocks.Add(new Block("WB", row++) { IsHalfBackground = true, IsHalfBlock = Visibility.Visible, Stage = PipelineStages.WriteBack });

			SetupDefaultBackground(InstructionBlocks);
		}

		public override void SetHazard(string hazard, string block, SolidColorBrush colorBrush)
		{
			var selectedBlock = InstructionBlocks.FirstOrDefault(p => string.Equals(p.Name, block));

			if (selectedBlock != null)
			{
				Hazard += hazard;
				selectedBlock.SetBackground(colorBrush);
				selectedBlock.SetForegroundBlack();
			}
		}

		public void SetupDefaultBackground(ObservableRangeCollection<Block> _collection)
		{
			foreach (var instruction in _collection)
			{
				instruction.SetDefaultBackground();
			}
		}

		private void AddBlankLines()
		{
			for (int i = 0; i < Row; i++)
			{
				InstructionBlocks.Add(new Block("", -1, true));
				InstructionBlocks.Add(new Block("", -1));
			}
		}

		private void AddStalls()
		{
			for (int i = 0; i < Stalls; i++)
			{
				InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.Stall });
				InstructionBlocks.Add(new Block("Stall", -1) { IsHalfBackground = false, Stage = PipelineStages.Stall });
			}
		}

		#endregion methods
	}
}