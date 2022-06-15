using MvvmHelpers;
using System.Collections.Generic;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class BubblePipelineInstruction : PipelineInstruction
	{
		#region constructor / destructor

		public BubblePipelineInstruction()
		{
			ValueNeeded = PipelineStages.Bubble;
			ForwardingValueAvailable = PipelineStages.Bubble;
		}

		#endregion constructor / destructor

		#region methods

		public override void Initialize(int row)
		{
			Row = row;
			for (int i = 0; i < Row; i++)
			{
				InstructionBlocks.Add(new Block("", -1, true));
				InstructionBlocks.Add(new Block("", -1));
			}
		}

		public override void SetupSolutions()
		{
			SetupStall();
		}

		private List<Block> BubbleRows()
		{
			List<Block> blockList = new List<Block>() { new Block(PipelineStages.Bubble.ToString(), -1) };

			for (int i = 0; i < 4; ++i)
			{
				blockList.Add(new Block("", -1) { Stage = PipelineStages.Bubble });
				blockList.Add(new Block(PipelineStages.Bubble.ToString(), -1) {Stage = PipelineStages.Bubble });
			}
			return blockList;
		}

		private void SetupDefaultBackground(ObservableRangeCollection<Block> _collection)
		{
			foreach (var instruction in _collection)
			{
				instruction.SetDefaultBackground();
			}
		}

		private void SetupForwarding()
		{
			SetupDefaultBackground(InstructionBlocks);
		}

		private void SetupReorderedForwarding()
		{
			SetupDefaultBackground(InstructionBlocks);
		}

		private void SetupStall()
		{
			InstructionBlocks.AddRange(BubbleRows());
			SetupDefaultBackground(InstructionBlocks);
		}

		#endregion methods
	}
}