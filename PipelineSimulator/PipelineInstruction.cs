using MvvmHelpers;
using System.Windows.Media;

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

		public int ForwardingValueAvailable
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

		#endregion properties

		#region methods

		public virtual void ClearAll()
		{
			InstructionBlocks.Clear();
			Instructions.Clear();
		}

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

		public virtual void Initialize(int row)
		{
		}

		public virtual void InsertStall()
		{
		}

		public virtual void SetHazard(string hazard, string block, SolidColorBrush colorBrush)
		{
		}

		public virtual void SetupSolutions()
		{
		}

		#endregion methods
	}
}