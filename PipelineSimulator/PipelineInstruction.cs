using MvvmHelpers;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class PipelineInstruction : BaseViewModel, IPipelineInstruction
	{
		#region member variables

		private string _hazard = string.Empty;

		#endregion member variables

		#region properties

		public string Command
		{
			get;
			set;
		} = string.Empty;

		public string Destination
		{
			get;
			set;
		} = string.Empty;

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
			get => _hazard;
			set
			{
				_hazard = value;
				OnPropertyChanged(nameof(Hazard));
			}
		}

		public string Instruction
		{
			get;
			set;
		} = string.Empty;

		public ObservableRangeCollection<Block> InstructionBlocks
		{
			get;
			set;
		} = new ObservableRangeCollection<Block>();

		public ObservableRangeCollection<string> Instructions
		{
			get;
			set;
		} = new ObservableRangeCollection<string>();

		public int Row
		{
			get;
			set;
		}

		public string Source
		{
			get;
			set;
		} = string.Empty;

		public string Source2
		{
			get;
			set;
		} = string.Empty;

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

		#endregion properties

		#region methods

		public virtual bool CheckForDataHazard(IPipelineInstruction pipelineInstruction, bool isForwarding, ref int rowIndex)
		{
			if (pipelineInstruction.ValueAvailable != PipelineStages.Blank && (string.Equals(pipelineInstruction.Destination, Source) || string.Equals(pipelineInstruction.Destination, Source2) || (string.Equals(pipelineInstruction.Destination, Destination) && this is StorePipelineInstruction)))
			{
				var instructionValueAvailable = pipelineInstruction.GetValueAvailableBlock(isForwarding);
				var valueNeededBlock = GetValueNeededBlock(isForwarding);

				var diff = instructionValueAvailable.Index - valueNeededBlock.Index;

				if ((diff > Stalls) || (!isForwarding && diff == 0 && valueNeededBlock.Stage != PipelineStages.ID))
				{
					if (diff == 0)
					{
						var value = (PipelineStages.WB - valueNeededBlock.Stage) / 2;
						diff += value;
					}

					rowIndex -= Stalls;
					ClearAll();
					Stalls = diff;
					Initialize(rowIndex);
					rowIndex += diff;
				}
			}

			return false;
		}

		public virtual void ClearAll()
		{
			Stalls = 0;
			InstructionBlocks.Clear();
			Instructions.Clear();
		}

		public virtual Block GetValueAvailableBlock(bool isForwarding)
		{
			return InstructionBlocks.First(p => p.Stage == (isForwarding ? ForwardingValueAvailable : ValueAvailable));
		}

		public virtual Block GetValueNeededBlock(bool isForwarding)
		{
			return InstructionBlocks.First(p => p.Stage == (isForwarding ? ForwardingValueNeeded : ValueNeeded));
		}

		public virtual void Initialize(int row)
		{
			Row = row;

			row++;

			AddBlankLines();

			InstructionBlocks.Add(new Block(PipelineStages.IF.ToString(), row++) { Stage = PipelineStages.IF });

			AddStalls();

			row += Stalls;

			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.IF_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.ID.ToString(), row++) { IsHalfBlock = Visibility.Visible, Stage = PipelineStages.ID });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.ID_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.EX.ToString(), row++) { Stage = PipelineStages.EX });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.EX_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.DMEM.ToString(), row++) { Stage = PipelineStages.DMEM });
			InstructionBlocks.Add(new Block("", row) { Stage = PipelineStages.DMEM_Finished });
			InstructionBlocks.Add(new Block(PipelineStages.WB.ToString(), row++) { IsHalfBlock = Visibility.Visible, Stage = PipelineStages.WB });

			SetupDefaultBackground(InstructionBlocks);
		}

		public virtual void SetHazard(string hazard, PipelineStages block, SolidColorBrush colorBrush, int registers)
		{
			var selectedBlock = InstructionBlocks.FirstOrDefault(p => string.Equals(p.Stage, block));

			if (selectedBlock != null)
			{
				if (!Hazard.Contains(hazard))
				{
					Hazard += hazard + "\n";
				}

				selectedBlock.SetBackground(colorBrush, registers);
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

		public virtual void SetupSolutions()
		{
		}

		private void AddBlankLines()
		{
			for (int i = 0; i < Row; i++)
			{
				InstructionBlocks.Add(new Block("", -1, true));
				InstructionBlocks.Add(new Block("", -1));
			}
		}

		#region methods

		public virtual IPipelineInstruction Copy()
		{
			return new PipelineInstruction()
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

		#endregion methods

		private void AddStalls()
		{
			for (int i = 0; i < Stalls; i++)
			{
				InstructionBlocks.Add(new Block("", -1) { Stage = PipelineStages.Stall });
				InstructionBlocks.Add(new Block(PipelineStages.Stall.ToString(), -1) { Stage = PipelineStages.Stall });
			}
		}

		#endregion methods
	}
}