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
			new SolidColorBrush(Colors.Tomato)
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