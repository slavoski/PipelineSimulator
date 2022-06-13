using MvvmHelpers;
using System.Windows;
using System.Windows.Media;
using static PipelineSimulator.Constants;

namespace PipelineSimulator
{
	public class Block : BaseViewModel
	{
		#region member variables

		private readonly SolidColorBrush _blackForegroundColor = new SolidColorBrush(Colors.Black);
		private readonly SolidColorBrush _halfColor = new SolidColorBrush(Colors.CadetBlue);
		private readonly SolidColorBrush _mainColor = (SolidColorBrush)Application.Current.Resources["MaterialDesignCardBackground"];
		private readonly SolidColorBrush _mainForegroundColor = (SolidColorBrush)Application.Current.Resources["MaterialDesignDarkForeground"];
		private SolidColorBrush _background;
		private SolidColorBrush _foreground = (SolidColorBrush)Application.Current.Resources["MaterialDesignDarkForeground"];
		private SolidColorBrush _halfBackground = (SolidColorBrush)Application.Current.Resources["MaterialDesignCardBackground"];
		private int _height = 30;
		private Visibility _isHalfBlock = Visibility.Collapsed;
		private SolidColorBrush _leftHalfBackground = (SolidColorBrush)Application.Current.Resources["MaterialDesignCardBackground"];
		private string _name;
		private int _width = 60;

		#endregion member variables

		#region Properties

		public SolidColorBrush Background
		{
			get => _background;
			set
			{
				_background = value;
				OnPropertyChanged(nameof(Background));
			}
		}

		public SolidColorBrush Foreground
		{
			get => _foreground;
			set
			{
				_foreground = value;
				OnPropertyChanged(nameof(Foreground));
			}
		}

		public SolidColorBrush HalfBackground
		{
			get => _halfBackground;
			set
			{
				_halfBackground = value;
				OnPropertyChanged(nameof(HalfBackground));
			}
		}

		public int Height
		{
			get => _height;
			set
			{
				_height = value;
				OnPropertyChanged(nameof(Height));
			}
		}

		public int Index
		{
			get;
			set;
		}

		public bool IsHalfBackground
		{
			get;
			set;
		}

		public Visibility IsHalfBlock
		{
			get => _isHalfBlock;
			set
			{
				_isHalfBlock = value;
				OnPropertyChanged(nameof(IsHalfBlock));
			}
		}

		public SolidColorBrush LeftHalfBackground
		{
			get => _leftHalfBackground;
			set
			{
				_leftHalfBackground = value;
				OnPropertyChanged(nameof(LeftHalfBackground));
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public PipelineStages Stage
		{
			get;
			set;
		}

		public int Width
		{
			get => _width;
			set
			{
				_width = value;
				OnPropertyChanged(nameof(Width));
			}
		}

		#endregion Properties

		#region constructor

		public Block(string name, int index, bool isEmpty = false)
		{
			_name = name;
			Index = index;
			Background = _mainColor;
			if (string.IsNullOrEmpty(name) && !isEmpty)
			{
				Width = 10;
				Height = 50;
			}
		}

		#endregion constructor

		#region methods

		public void SetBackground(SolidColorBrush _background)
		{
			if (IsHalfBackground)
			{
				if (string.Equals(Name, "WB"))
				{
					LeftHalfBackground = _background;
				}
				else
				{
					HalfBackground = _background;
				}
			}
			else
			{
				Background = _background;
			}
		}

		public void SetDefaultBackground()
		{
			if (IsHalfBackground)
			{
				SetBackground(_halfColor);
			}
			else
			{
				SetBackground(_mainColor);
			}

			SetForeground(_mainForegroundColor);
		}

		public void SetForeground(SolidColorBrush _newColor)
		{
			Foreground = _newColor;
		}

		public void SetForegroundBlack()
		{
			Foreground = _blackForegroundColor;
		}

		#endregion methods
	}
}