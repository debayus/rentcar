using System;
namespace Mahas.Helpers
{
	public class MahasSelectListItem
	{
		public MahasSelectListItem()
		{
            Text = "";
        }

        public MahasSelectListItem(string text, Object value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; set; }
        public object? Value { get; set; }
    }
}

