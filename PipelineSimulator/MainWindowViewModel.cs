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

		public ObservableRangeCollection<CommandDescription> CommandDescriptions
		{
			get;
			set;
		} = new ObservableRangeCollection<CommandDescription>();

		public ObservableRangeCollection<IPipelineInstruction> Instructions
		{
			get;
			set;
		} = new ObservableRangeCollection<IPipelineInstruction>();

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

		private void BuildCommand(string command, string[] parameters, string instruction, int row)
		{
			var isNewCommand = false;

			switch (command)
			{
				case "add":
					{
						isNewCommand = true;
						Instructions.Add(new ArithmeticPipelineInstruction()
						{
							Destination = GetValidRegister(parameters[0], row),
							Source = GetValidRegister(parameters[1], row),
							Source2 = GetValidRegister(parameters[2], row),
							Instruction = instruction,
							Row = row,
						});
					}
					break;

				case "sub":
					{
						isNewCommand = true;
						Instructions.Add(new ArithmeticPipelineInstruction()
						{
							Destination = GetValidRegister(parameters[0], row),
							Source = GetValidRegister(parameters[1], row),
							Source2 = GetValidRegister(parameters[2], row),
							Instruction = instruction,
							Row = row,
						});
					}
					break;

				case "lw":
					{
						isNewCommand = true;
					}
					break;

				case "sw":
					{
						isNewCommand = true;
					}
					break;

				default:
					{
						SnackBoxMessage.Enqueue($"Bad Parsing: Command Not Recognized: {command}");
					}
					break;
			}

			if (isNewCommand)
			{
				Instructions.Last().Initialize();
			}
		}

		private string[] GetParameters(string line, int index)
		{
			return line.Substring(index + 1, line.Length - index - 1).Split(new char[] { ' ', ',' });
		}

		private string GetValidRegister(string inputParameter, int row)
		{
			string result = "";
			if (3 == inputParameter.Length && string.Equals(inputParameter[0], '$'))
			{
				result = inputParameter;
			}
			else
			{
				SnackBoxMessage.Enqueue($"Bad Register: Line: {row},  Register: {inputParameter} not recognized");
				throw new ArgumentException();
			}
			return result;
		}

		private void ParseAllCommands()
		{
			Instructions.Clear();
			int row = 0;

			try
			{
				foreach (var line in MainFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					ParseCommandLine(line, row);
					row++;
				}
			}
			catch
			{
			}
		}

		private void ParseCommandLine(string line, int row)
		{
			line = line.Trim();
			var indexOfFirstSpace = line.IndexOf(' ');

			if (indexOfFirstSpace > 0)
			{
				var command = line.Substring(0, indexOfFirstSpace);
				var parameters = GetParameters(line, indexOfFirstSpace);

				BuildCommand(command, parameters, line, row);
			}
		}

		#endregion methods
	}
}