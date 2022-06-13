using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using MvvmHelpers.Commands;
using ProcessorSimulator;
using System;

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
					}
					break;

				case "sw":
					{
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

		private string[] GetParameters(string line, int index)
		{
			return line.Substring(index + 1, line.Length - index - 1).Split(new char[] { ' ', ',' });
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