using System.Collections.Generic;
using System.Windows.Media;

namespace PipelineSimulator
{
	public class ColorManager
	{
		private List<SolidColorBrush> colorsToChoose = new List<SolidColorBrush>()
		{
			new SolidColorBrush(Colors.Red),
			new SolidColorBrush(Colors.Magenta),
			new SolidColorBrush(Colors.Yellow),
			new SolidColorBrush(Colors.Orange),
			new SolidColorBrush(Colors.Orchid),
			new SolidColorBrush(Colors.Tomato),
			new SolidColorBrush(Colors.BlueViolet),
			new SolidColorBrush(Colors.Brown),
			new SolidColorBrush(Colors.BurlyWood),
			new SolidColorBrush(Colors.Chartreuse),
			new SolidColorBrush(Colors.Chocolate),
			new SolidColorBrush(Colors.Coral),
			new SolidColorBrush(Colors.DarkGoldenrod),
			new SolidColorBrush(Colors.Crimson),
			new SolidColorBrush(Colors.DarkGreen),
			new SolidColorBrush(Colors.DarkKhaki),
			new SolidColorBrush(Colors.DarkMagenta),
			new SolidColorBrush(Colors.DarkOliveGreen),
			new SolidColorBrush(Colors.DarkOrange),
			new SolidColorBrush(Colors.DarkOrchid),
			new SolidColorBrush(Colors.DarkRed),
			new SolidColorBrush(Colors.DarkSalmon),
			new SolidColorBrush(Colors.DarkSeaGreen),
			new SolidColorBrush(Colors.DarkSlateBlue),
			new SolidColorBrush(Colors.DarkViolet),
			new SolidColorBrush(Colors.DeepPink),
			new SolidColorBrush(Colors.Firebrick),
		};

		private int index;

		public void ClearIndex()
		{
			index = -1;
		}

		public SolidColorBrush GetColor()
		{
			index++;
			if (index > colorsToChoose.Count - 1)
			{
				index = 0;
			}

			return colorsToChoose[index];
		}
	}
}