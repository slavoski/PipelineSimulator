using MvvmHelpers;
using System.Windows;
using System.Windows.Media;

namespace PipelineSimulator
{
	public class Block : BaseViewModel
	{
		#region member variables

		private readonly SolidColorBrush _mainColor = (SolidColorBrush)Application.Current.Resources["MaterialDesignCardBackground"];
		private SolidColorBrush _background;
		private SolidColorBrush _halfBackground = new SolidColorBrush(Colors.AliceBlue);
		private int _height = 30;
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

		public bool IsHalfBackground
		{
			get;
			set;
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

		public Block(string name, bool isEmpty = false)
		{
			_name = name;
			Background = _mainColor;
			if (string.IsNullOrEmpty(name) && !isEmpty)
			{
				Width = 10;
				Height = 50;
			}
		}

		#endregion constructor

		#region methods

		public bool IsEnd
		{
			get;
			set;
		}

		public bool IsStart
		{
			get;
			set;
		}

		public void SetBackground(SolidColorBrush _background)
		{
			if (IsHalfBackground)
			{
				HalfBackground = _background;
			}
			else
			{
				Background = _background;
			}
		}

		#endregion methods
	}
}