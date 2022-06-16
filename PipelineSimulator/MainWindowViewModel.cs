using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProcessorSimulator;
using System;
using System.Linq;

namespace PipelineSimulator.VM
{
	public class MainWindowViewModel : BaseViewModel
	{
		#region member variables

		private string _mainFile = "";

		#endregion member variables

		#region properties

		public PipelineManager AllPipelines
		{
			get;
			set;
		} = new PipelineManager();

		public ObservableRangeCollection<CommandDescription> CommandDescriptions
		{
			get;
			set;
		} = new ObservableRangeCollection<CommandDescription>();

		public string MainFile
		{
			get => _mainFile;
			set
			{
				_mainFile = value;
				OnPropertyChanged(nameof(MainFile));
			}
		}

		public SnackbarMessageQueue SnackBoxMessage
		{
			get;
			set;
		} = new SnackbarMessageQueue();

		#endregion properties

		#region Commands

		public Command ParseCommands
		{
			get;
			set;
		}

		#endregion Commands

		#region constructor / destructor

		public MainWindowViewModel()
		{
			InitializeCommandDescriptions();
			ParseCommands = new Command(() => ParseAllCommands());
		}

		#endregion constructor / destructor

		#region methods

		public void InitializeCommandDescriptions()
		{
			CommandDescriptions = new ObservableRangeCollection<CommandDescription>()
			{
				new CommandDescription() { Name="add", Example="add $t1,$t2,$t3" },
				new CommandDescription() { Name="sub", Example="sub $t1,$t2,$t3" },
				new CommandDescription() { Name="lw", Example="lw $t1,-100($t2)" },
				new CommandDescription() { Name="sw", Example="sw $t1,-100($t2)" },
			};
		}

		private void BuildCommand(string command, string[] parameters, string instruction)
		{
			IPipelineInstruction newInstruction = null;

			switch (command)
			{
				case "add":
					{
						newInstruction = new ArithmeticPipelineInstruction()
						{
							Command = command,
							Destination = GetValidRegister(parameters[0]),
							Source = GetValidRegister(parameters[1]),
							Source2 = GetValidRegister(parameters[2]),
							Instruction = instruction,
						};
					}
					break;

				case "sub":
					{
						newInstruction = new ArithmeticPipelineInstruction()
						{
							Command = command,
							Destination = GetValidRegister(parameters[0]),
							Source = GetValidRegister(parameters[1]),
							Source2 = GetValidRegister(parameters[2]),
							Instruction = instruction,
						};
					}
					break;

				case "lw":
					{
						newInstruction = new LoadPipelineInstruction()
						{
							Command = command,
							Destination = GetValidRegister(parameters[0]),
							Source = GetValidLoadRegister(parameters[1]),
							Instruction = instruction,
						};
					}
					break;

				case "sw":
					{
						newInstruction = new StorePipelineInstruction()
						{
							Command = command,
							Destination = GetValidLoadRegister(parameters[1]),
							Source = GetValidRegister(parameters[0]),
							Instruction = instruction,
						};
					}
					break;

				default:
					{
						SnackBoxMessage.Enqueue($"Bad Parsing: Command Not Recognized: {command}");
					}
					break;
			}

			if (newInstruction != null)
			{
				AllPipelines.AddNewInstruction(newInstruction);
			}
		}

		private void ClearAll()
		{
			AllPipelines.ClearLists();
		}

		private string[] GetParameters(string line, int index) =>
			line.Substring(index + 1, line.Length - index - 1).Split(new char[] { ' ', ',' }).Where(p => !string.IsNullOrEmpty(p)).ToArray();

		private string GetValidLoadRegister(string inputParameter)
		{
			string result;

			if (3 <= inputParameter.Length)
			{
				var index = inputParameter.IndexOf('$');
				var reg = inputParameter.Substring(index, 3);

				if (reg != null && !Char.IsDigit(reg[1]) && Char.IsDigit(reg[2]))
				{
					result = inputParameter;
				}
				else
				{
					SnackBoxMessage.Enqueue($"Bad Register: {inputParameter} not recognized");
					throw new ArgumentException();
				}
			}
			else
			{
				SnackBoxMessage.Enqueue($"Bad Register: {inputParameter} not recognized");
				throw new ArgumentException();
			}
			return result;
		}

		private string GetValidRegister(string inputParameter)
		{
			string result;
			if (3 == inputParameter.Length && string.Equals(inputParameter[0], '$'))
			{
				result = inputParameter;
			}
			else
			{
				SnackBoxMessage.Enqueue($"Bad Register: {inputParameter} not recognized");
				throw new ArgumentException();
			}
			return result;
		}

		private void ParseAllCommands()
		{
			ClearAll();

			var instructions = MainFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in instructions)
			{
				ParseCommandLine(line);
			}

			AllPipelines.AddColors();
			AllPipelines.CheckForDataHazardOfHazardPipeline();
		}

		private void ParseCommandLine(string line)
		{
			line = line.Trim();
			var indexOfFirstSpace = line.IndexOf(' ');

			if (indexOfFirstSpace > 0)
			{
				var command = line.Substring(0, indexOfFirstSpace);
				var parameters = GetParameters(line, indexOfFirstSpace);

				BuildCommand(command, parameters, line);
			}
		}

		#endregion methods
	}
}