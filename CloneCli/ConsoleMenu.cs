using System;
using System.Collections.Generic;
using System.Linq;

namespace CloneCli
{
public class ConsoleMenuItem
	{
		public int Index { get; set; }
		private bool _selected;
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
			}
		}
		private string _text;
		public string Text 
		{ 
			get
			{
				var indicator = " ";
				if (_selected)
				{
					indicator = "*";
				}
				return _text.Replace("[VALUE]", indicator);
			}
		}
		
		public ConsoleMenuItem(string text)
		{
			_text = "[[VALUE]] " + Index + " " + text;
		}
		
		public ConsoleMenuItem(string text, bool selected)
		{
			_text = "[[VALUE]] - " + text;
			_selected = true;
		}
	}
	
	
	
	
	
	public class ConsoleMenu
	{
		private int nextIndex = 0;
		private List<ConsoleMenuItem> _items = new List<ConsoleMenuItem>();
		public List<ConsoleMenuItem> Items
		{
			get
			{
				return _items;
			}
		}
		public int SelectedIndex
		{
			get
			{
				var selectedItem =
					(from ConsoleMenuItem item in _items
					where (item.Selected == true)
					select item).SingleOrDefault();
				if (selectedItem != null) {
					return selectedItem.Index;
				}
				else
				{
					return -1;
				}
			}
		}
		public ConsoleMenuItem SelectedItem
		{
			get
			{
				var selectedItem =
					(from ConsoleMenuItem item in _items
						where (item.Selected == true)
						select item).SingleOrDefault();
				return selectedItem;
			}
		}
		public void Add(ConsoleMenuItem menuItem)
		{
			menuItem.Index = nextIndex;
			_items.Add(menuItem);
			nextIndex++;
		}
		//public void Remove(MenuItem menuItem)
		//{
		//	_items.Remove(menuItem);
		//}
		public int ItemCount
		{
			get
			{
				return _items.Count;
			}
		}
		public void Select(int itemNo)
		{
			//selectedItem = SelectedItem;
			if (SelectedIndex != itemNo)
			{
				SelectedItem.Selected = false;
				_items[itemNo].Selected = true;
			}
		}
		public void Select(ConsoleMenuItem item)
		{
			if (SelectedItem != item)
			{
				SelectedItem.Selected = false;
				item.Selected = true;
			}
		}
		public void SelectNext()
		{
			if (SelectedIndex < ItemCount)
			{
				SelectedItem.Selected = false;
				Items[SelectedIndex + 1].Selected = true;
			}
		}
		public void SelectPrev()
		{
			if (SelectedIndex > 0)
			{
				SelectedItem.Selected = false;
				Items[SelectedIndex - 1].Selected = true;
			}
		}
		public void Draw()
		{
			if (this.ItemCount > 0)
			{
				Console.Clear();
				foreach (ConsoleMenuItem menuItem in Items)
				{
					Console.WriteLine(menuItem.Text);
				}
			}
		}
	}
}

