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
			ValueAvailable = PipelineStages.WB;
			ValueNeeded = PipelineStages.ID;
			ForwardingValueAvailable = PipelineStages.EX_Finished;
			ForwardingValueNeeded = PipelineStages.EX;
		}

		#endregion constructor / destructor

		#region methods

		public override bool CheckForDataHazard(IPipelineInstruction pipelineInstruction, bool isForwarding, ref int rowIndex)
		{
			if (string.Equals(pipelineInstruction.Destination, Source) || string.Equals(pipelineInstruction.Destination, Source2))
			{
				var instructionValueAvailable = pipelineInstruction.GetValueAvailableBlock(isForwarding);
				var valueNeededBlock = GetValueNeededBlock(isForwarding);

				var diff = instructionValueAvailable.Index - valueNeededBlock.Index;

				if (diff > Stalls)
				{
					rowIndex -= Stalls;
					ClearAll();
					Stalls = diff;
					Initialize(rowIndex);
					rowIndex += diff;
				}
			}

			return false;
		}

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

		public override Block GetValueAvailableBlock(bool isForwarding)
		{
			return InstructionBlocks.First(p => string.Equals(isForwarding ? ForwardingValueAvailable : ValueAvailable, p.Stage));
		}

		public override Block GetValueNeededBlock(bool isForwarding)
		{
			return InstructionBlocks.First(p => string.Equals(isForwarding ? ForwardingValueNeeded : ValueNeeded, p.Stage));
		}

		public override void Initialize(int row)
		{
			Row = row;

			row++;

			AddBlankLines();

			InstructionBlocks.Add(new Block(PipelineStages.IF.ToString(), row++) { IsHalfBackground = false, Stage = PipelineStages.IF });

			AddStalls();

			row += Stalls;

			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.IF_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.ID.ToString(), row++) { IsHalfBackground = true, IsHalfBlock = Visibility.Visible, Stage = PipelineStages.ID });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.ID_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.EX.ToString(), row++) { IsHalfBackground = false, Stage = PipelineStages.EX });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.EX_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.DMEM.ToString(), row++) { IsHalfBackground = false, Stage = PipelineStages.DMEM });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.WB_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.WB.ToString(), row++) { IsHalfBackground = true, IsHalfBlock = Visibility.Visible, Stage = PipelineStages.WB });

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
				InstructionBlocks.Add(new Block(PipelineStages.Stall.ToString(), -1) { IsHalfBackground = false, Stage = PipelineStages.Stall });
			}
		}

		#endregion methods
	}
}